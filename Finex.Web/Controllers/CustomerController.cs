using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finex.Buisness.IBuisnessHelper;
using Finex.Dto.Dtos;
using Finex.Utility;
using System.Net.Mail;


namespace Finex.Web.Controllers
{
    public class CustomerController : Controller
    {

        private ICustomerHelper _iCustomerHelper;
        private IClaimHelper _iClaimHelper;


        public CustomerController()
        {
            _iCustomerHelper = DependencyResolver.GetCustomerInstance();
            _iClaimHelper = DependencyResolver.GetClaimInstance();

        }
        //
        // GET: /Customer/


        public ActionResult RegenrateOtp(string id)
        {
            var custId = Convert.ToInt32(Decryption.Decrypt(id, true));
            var otpNumber = _iClaimHelper.RegenrateOtp(custId);
            SendMailCustomer(custId, otpNumber);
            ViewBag.Incorrectlogin = "OTP successfully regenerated and sent to your registered Email Address.";
            var cust = _iCustomerHelper.GetCustomerById(custId);

            return View("Index", new OTPDto
            {
                CustomerId = cust.CustomerId,
                CustomerName = cust.CustomerName,
            });
        }

        private void SendMailCustomer(int custId, string otp)
        {
            try
            {
                var customerDto = _iCustomerHelper.GetCustomerById(custId);

                var msg = new MailMessage();
                //Sender e-mail address.
                msg.From = new MailAddress("");
                msg.Subject = "New Otp ";
                var body = "Dear Sir/Madam,<br><br> your new OTP : " + otp + "  It will expire after 48 hours <br><br>";



                body = body + "<br><br> Thanks";

                msg.Body = body;

                msg.To.Add(customerDto.EmailId);
                msg.IsBodyHtml = true;

                //your remote SMTP server IP.
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("", ""),
                    EnableSsl = true
                };
                
                smtp.Send(msg);
                msg = null;



            }
            catch (Exception ex)
            {

            }
        }
        public ActionResult Index(string id)
        {
            var custId = Convert.ToInt32(Decryption.Decrypt(id, true));
            var cust = _iCustomerHelper.GetCustomerById(custId);

            return View(new OTPDto
                            {
                                CustomerId = cust.CustomerId,
                                CustomerName = cust.CustomerName,
                            });
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LoginCustomer(OTPDto otpDto)
        {


            var reponseCode = _iCustomerHelper.ValidateCustomerCredentials(otpDto);
            switch (reponseCode)
            {
                case 0:
                    ViewBag.Incorrectlogin = "No Claim";
                    break;
                case 1:
                    ViewBag.Incorrectlogin = "Invalid card number";
                    break;
                case 2:
                    ViewBag.Incorrectlogin = "Invalid OTP";
                    break;
                case 3:
                    ViewBag.Incorrectlogin = "OTP has expired, please use the button above to reissue and OTP to be sent to your Email address and retry.";
                    break;
                case 4:
                    _iClaimHelper.UpdateOtpToExpire(otpDto.CustomerId);
                    return RedirectToAction("GetUploadDocumentForCustomer", new { cuId = Encryption.Encrypt(Convert.ToString(otpDto.CustomerId), true) });
            }
            return View("Index", otpDto);
        }

        public ActionResult GetUploadDocumentForCustomer(string cuId)
        {
            var custId = Convert.ToInt32(Decryption.Decrypt(cuId, true));
            var claim = _iCustomerHelper.GetClaimByCustId(custId);
            ViewBag.ClaimId = claim.ClaimId;
            ViewBag.LossTypeId = claim.LossTypeId;
            ViewBag.CustomerId = custId;
            var documentList = _iClaimHelper.GetUploadDocumentMaster(claim.LossTypeId);
            return View("UploadDocumentCustomer", documentList);
        }

        public ActionResult UploadDocumentCust(HttpPostedFileBase[] uploadFiles, FormCollection frm)
        {
            var hdnClaimId = frm.GetValue("hdnClaimId");
            var hdnLossType = frm.GetValue("hdnLossType");
            var hdnCustomer = frm.GetValue("hdnCustomer");
            var hdnDocumentTypeIds = frm.GetValues("hdnDocumentTypeIds");
            var documentUploadList = new List<DocumentUploadsDto>();
            var i = 0;
            var idOfOther = 0;
         
            foreach (var file in uploadFiles)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = hdnClaimId.AttemptedValue + "-" + hdnCustomer.AttemptedValue + "-" + hdnDocumentTypeIds[i] + "-" + file.FileName;
                    file.SaveAs(Server.MapPath("~/UploadDocuments/") + fileName);
                    documentUploadList.Add(new DocumentUploadsDto
                    {
                        ClaimId = Convert.ToInt32(hdnClaimId.AttemptedValue),
                        CustomerId = Convert.ToInt32(hdnCustomer.AttemptedValue),
                        DocumentPath = fileName,
                        DocumentTypeId = Convert.ToInt32(hdnDocumentTypeIds[i])
                    });
                }
                i++;
            }


            var statusId = _iClaimHelper.AddUpdateUploadDocument(documentUploadList, idOfOther, Convert.ToInt32(hdnClaimId.AttemptedValue));
            if (statusId == 8)
            {
                SendMailOfDocumentationComplete(Convert.ToInt32(hdnClaimId.AttemptedValue));
            }


            var documentList = _iClaimHelper.GetUploadDocumentMaster(Convert.ToInt32(hdnLossType.AttemptedValue));
            Session["SuccessMessage"] = "Document Uploaded successfully";
            ViewBag.hdnCustomer = hdnCustomer.AttemptedValue;
            return View("UploadDocumentCustomer", documentList);
        }

        private void SendMailOfDocumentationComplete(int claimId)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["url"].ToString() + "Insurer/Index?id=" +
                         Encryption.Encrypt(Convert.ToString(claimId), true);
            var template = _iClaimHelper.GetMailTemplate(5);
            var claimsDto = _iClaimHelper.GetClaimByClaimIdForEdit(claimId);
            try
            {
                var msg = new MailMessage();
                msg.From = new MailAddress("axiscardclaims@gmail.com");
                msg.Subject = template.Subject;
                var body = template.TemplateBody.Replace("@CustomerName", claimsDto.CustomersDto.CustomerName);
                body = body.Replace("@ClaimNumber", claimsDto.ClaimNumber);
                body = body.Replace("@url", url);
                msg.Body = body;
                
                msg.To.Add("sagar.chilamkurty@newindia.co.in,gaurav.pal@newindia.co.in,prachi.bhise@newindia.co.in");
                msg.CC.Add("anurag.rai@almondz.com,arjun.thakur@almondz.com");
                
                msg.IsBodyHtml = true;
                //your remote SMTP server IP.
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("axiscardclaims@gmail.com", "accessaxis"),
                    EnableSsl = true
                };
                //smtp.EnableSsl = true;
                smtp.Send(msg);
                msg = null;
            }
            catch (Exception ex)
            {

            }
        }

    }
}
