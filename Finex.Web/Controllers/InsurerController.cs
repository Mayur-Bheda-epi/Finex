using System.Globalization;
using System.Text;
using Finex.Buisness.IBuisnessHelper;
using Finex.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Finex.Dto.Dtos;
using System.Net;
using System.Xml;
using System.IO;

namespace Finex.Web.Controllers
{
    public class InsurerController : Controller
    {
        //
        // GET: /Insurer/

        private IClaimHelper _iClaimHelper;

        public InsurerController()
        {
            _iClaimHelper = DependencyResolver.GetClaimInstance();
        }


        public ActionResult GetClaimForInsuer(int page = 1, string error = "")
        {
            ViewBag.AccountNo = "";
            ViewBag.Name = "";
            ViewBag.CardNo = "";
            ViewBag.MobileNumber = "";
            ViewBag.EmailId = "";
            if (error != "")
                ViewBag.Error = error;
            ViewBag.All = true;
            var claimInsurer = new InsurerClaimDto();
            claimInsurer.StatusMasterDtos = _iClaimHelper.GetStatus();
            var claimPageData = new PagedData<ClaimsDto>();
            claimInsurer.PagedData = claimPageData;
            claimPageData.Data = _iClaimHelper.GetClaimList(50, page);
            claimPageData.CurrentPage = page;
            var totalCount = _iClaimHelper.GetTotalClaimCount();
            claimPageData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)totalCount / 50));
            Session["InsurerclaimList"] = _iClaimHelper.GetClaimList(1, 50);
            return View("ClaimListForInsurer", claimInsurer);

        }

        public ActionResult ExportClaim(bool all, string acNo, string name, string crdNo, string mNo, string eml)
        {
            //if (Session["InsurerclaimList"] != null)
            //{
            var searchDto = new ClaimSearchDto
            {
                CardNo = crdNo,
                Name = name,

                AccountNo = acNo,
                EmailId = eml,
                MobileNo = mNo,
                UserId = Convert.ToInt32(Session["UserId"]),
                UserTypeId = 1
            };
            var claimList = _iClaimHelper.GetClaimsForExport(new List<int>(), searchDto, all);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border=`" + "1px" + "`b>");
            //code section for creating header column
            sb.Append("<tr>");
            sb.Append("<td><b><font face=Arial size=2>Claim Number</font></b></td>");
            sb.Append("<td><b><font face=Arial size=2>Claim Status</font></b></td>");
            sb.Append("<td><b><font face=Arial size=2>Claimant Name</font></b></td>");
            sb.Append("<td><b><font face=Arial size=2>Card Number</font></b></td>");
            sb.Append("<td><b><font face=Arial size=2>Card Type</font></b></td>");
            sb.Append("<td><b><font face=Arial size=2>Type of loss</font></b></td>");
            sb.Append("<td><b><font face=Arial size=2>Comments</font></b></td>");


            sb.Append("</tr>");

            //code for creating excel data
            foreach (var item in claimList)
            {
                sb.Append("<tr>");
                sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.ClaimNumber + "</font></td>");
                sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.StatusMasterDto.StatusName + "</font></td>");
                sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.CustomersDto.CustomerName + "</font></td>");
                sb.Append("<td style='mso-number-format:0'><font face=Arial size=" + "2px" + ">" + item.CardNo.ToString(CultureInfo.InvariantCulture) + " </font></td>");
                sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.CardTypeMasterDto.CardTypeName + "</font></td>");
                sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.LossTypeMasterDto.LossType + "</font></td>");
                sb.Append("<td><font face=Arial size=" + "2px" + ">" + " " + "</font></td>");


                sb.Append("</tr>");
            }
            sb.Append("</table>");
            HttpContext.Response.AddHeader("content-disposition",
                                           "attachment; filename=ClaimListInsurer_" + DateTime.Now.Year.ToString() + ".xls");
            this.Response.ContentType = "application/vnd.ms-excel";


            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());

            return File(buffer, "application/vnd.ms-excel");
            //}
            // return RedirectToAction("Index");
        }

        public ActionResult ApproveReject(FormCollection frm, string btnApproveReject)
        {
            var hdnClaimIdForStatus = frm.GetValue("hdnClaimIdForStatus");
            var txtCommentAppoveReject = frm.GetValue("txtCommentAppoveReject");
            var status = 0;
            var claimObj = _iClaimHelper.GetClaimDetail(Convert.ToInt32(hdnClaimIdForStatus.AttemptedValue));
            if (btnApproveReject == "Approve")
            {
                var transaction = _iClaimHelper.GetNewTransaction("Claim Processing");
                if (transaction != null)
                {
                    HttpWebRequest request = CreateWebRequest();
                    XmlDocument soapEnvelopeXml = new XmlDocument();

                    var transactionNumber = transaction.TransactionNumber;

                    var claimData = "DM_AXIS~~" + claimObj.ClaimNumber + "~~" + transactionNumber + "~~" + transaction.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss");

                    soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:typ=""http://iims.services/types/"">
   <soapenv:Header/>
   <soapenv:Body>
      <typ:processClaimTieUpElement>
         <typ:userCode>AXISUSER</typ:userCode>
         <typ:rolecode>SUPERUSER</typ:rolecode>
         <typ:PRetCode>0</typ:PRetCode>
         <typ:PRetErr></typ:PRetErr>
         <typ:stakeCode>POLICY-HOL</typ:stakeCode>
         <typ:claimData>" + claimData + "</typ:claimData>     </typ:processClaimTieUpElement>    </soapenv:Body> </soapenv:Envelope>");
                    try
                    {
                        using (Stream stream = request.GetRequestStream())
                        {
                            soapEnvelopeXml.Save(stream);
                        }
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                            {
                                string soapResult = rd.ReadToEnd();

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(soapResult);
                                var a = xmlDoc.LastChild;

                                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                                manager.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                                manager.AddNamespace("typ", "http://iims.services/types/");


                                string xpath = "env:Envelope/env:Body/typ:processClaimTieUpResponseElement";
                                var nodes = xmlDoc.SelectNodes(xpath, manager);
                                if (nodes != null && nodes.Count > 0)
                                {
                                    var nodesCount = nodes[0].ChildNodes;
                                    if (nodesCount.Count > 0)
                                    {
                                        var retCode = nodesCount[3].InnerText;
                                        if (retCode == "0")
                                        {
                                            status = 3;
                                            transaction.Message = "Success";
                                            _iClaimHelper.UpdateTransaction(transaction);


                                        }
                                        else
                                        {
                                            transaction.Message = "Exception occur while processing claim. Please try after some time";
                                            _iClaimHelper.UpdateTransaction(transaction);
                                            return RedirectToAction("GetClaimForInsuer", new { error = transaction.Message });
                                        }
                                    }
                                }


                            }
                        }
                    }

                    catch (Exception ex)
                    {

                    }
                }
                
                             
            }
            else
            {
                status = 5;
            }
            _iClaimHelper.ApproveReject(Convert.ToInt32(hdnClaimIdForStatus.AttemptedValue), status, txtCommentAppoveReject.AttemptedValue);
            
           // var claim = _iClaimHelper.GetClaimDetail(Convert.ToInt32(hdnClaimIdForStatus.AttemptedValue));
            //SendMailApprovalReject(claim, status);
            return View("Index", claimObj);

        }

        private void SendMailApprovalReject(ClaimsDto claim, int status)
        {
            try
            {
                if (status != 1)
                {

                    var template = _iClaimHelper.GetMailTemplate(status == 3 ? 1 : 2);
                    var msg = new MailMessage();
                    //Sender e-mail address.
                    msg.From = new MailAddress("axiscardclaims@gmail.com");
                    msg.Subject = template.Subject;
                    var body = template.TemplateBody.Replace("@CustomerName", claim.CustomersDto.CustomerName);
                    body = body.Replace("@SrNumber", claim.SrNumber);

                    msg.Body = body;
                    
                    msg.To.Add(
                        "anurag.rai@almondz.com,arjun.thakur@almondz.com");
                    
                    msg.IsBodyHtml = true;

                    //your remote SMTP server IP.
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        Credentials =
                                           new System.Net.NetworkCredential("axiscardclaims@gmail.com", "accessaxis"),
                        EnableSsl = true
                    };
                    
                    smtp.Send(msg);
                    msg = null;
                }


            }
            catch (Exception ex)
            {

            }
        }

        public ActionResult UpdateStatus(FormCollection frm, string btnUpdate)
        {
            if (btnUpdate == "update")
            {

                var ddlStatus = frm.GetValue("ddlStatus");
                var txtComment = frm.GetValue("txtComment");
                var chkClaimIds = frm.GetValues("chkClaimIds");
                var updateStatusListRequesstList = new List<UpdateStatusRequestDto>();
                for (var i = 0; i < chkClaimIds.Length; i++)
                {
                    updateStatusListRequesstList.Add(new UpdateStatusRequestDto
                    {
                        ClaimId = Convert.ToInt32(chkClaimIds[i]),
                        StatusId = Convert.ToInt32(ddlStatus.AttemptedValue),
                        UpdateDate = DateTime.Now,
                        UpdateUserId = Convert.ToInt32(Session["UserId"]),
                        Comment = txtComment.AttemptedValue
                    });
                }
                var claimList = _iClaimHelper.UpdateStatusOfClaim(updateStatusListRequesstList);
                foreach (var claimsDto in claimList)
                {
                    SendMailApprovalReject(claimsDto, Convert.ToInt32(ddlStatus.AttemptedValue));
                }
            }
            else if (btnUpdate == "Search")
            {
                var txtAccountNo = frm.GetValue("txtAccountNo");
                var txtName = frm.GetValue("txtName");
                var txtCardNumber = frm.GetValue("txtCardNumber");
                var txtEmailId = frm.GetValue("txtEmailId");
                var txtMobileNo = frm.GetValue("txtMobileNo");

                var searchDto = new ClaimSearchDto
                {
                    CardNo = txtCardNumber.AttemptedValue,
                    Name = txtName.AttemptedValue,
                    //  PanNo = txtPanNo.AttemptedValue,
                    AccountNo = txtAccountNo.AttemptedValue,
                    EmailId = txtEmailId.AttemptedValue,
                    MobileNo = txtMobileNo.AttemptedValue,
                    UserId = Convert.ToInt32(Session["UserId"]),
                    UserTypeId = 1
                };
                ViewBag.IsSearch = true;
                ViewBag.All = false;
                ViewBag.AccountNo = txtAccountNo.AttemptedValue;
                ViewBag.Name = txtName.AttemptedValue;
                ViewBag.CardNo = txtCardNumber.AttemptedValue;
                ViewBag.MobileNumber = txtMobileNo.AttemptedValue;
                ViewBag.EmailId = txtEmailId.AttemptedValue;
                var claimInsurer = new InsurerClaimDto();
                claimInsurer.StatusMasterDtos = _iClaimHelper.GetStatus();

                //claimInsurer.ClaimsDtos = claimSeachList;
                var claimPageData = new PagedData<ClaimsDto>();
                var claimSeachList = _iClaimHelper.GetClaimSearch(searchDto, 50, 1);
                claimPageData.Data = claimSeachList;
                claimPageData.CurrentPage = 1;
                var totalCount = _iClaimHelper.GetTotalClaimCountForSearch(searchDto);

                claimPageData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)totalCount / 50));
                claimInsurer.PagedData = claimPageData;
                Session["InsurerclaimList"] = claimSeachList;
                return View("ClaimListForInsurer", claimInsurer);
            }
            return RedirectToAction("GetClaimForInsuer");
        }

        public ActionResult Index(string id)
        {
            var clid = Decryption.Decrypt(id, true);
            return View(_iClaimHelper.GetClaimDetail(Convert.ToInt32(clid)));
        }

        public ActionResult SearchClaimPage(string acNo, string name, string crdNo, string mNo, string eml, int page = 1)
        {
            ViewBag.IsSearch = true;
            ViewBag.AccountNo = acNo;
            ViewBag.Name = name;
            ViewBag.CardNo = crdNo;
            ViewBag.MobileNumber = mNo;
            ViewBag.EmailId = eml;
            var searchDto = new ClaimSearchDto
            {
                CardNo = crdNo,
                Name = name,
                AccountNo = acNo,
                EmailId = eml,
                MobileNo = mNo,
                UserId = Convert.ToInt32(Session["UserId"]),
                UserTypeId = 1
            };
            var claimInsurer = new InsurerClaimDto();
            claimInsurer.StatusMasterDtos = _iClaimHelper.GetStatus();
            var claimPageData = new PagedData<ClaimsDto>();
            var claimSeachList = _iClaimHelper.GetClaimSearch(searchDto, 50, page);
            claimPageData.Data = claimSeachList;
            claimPageData.CurrentPage = page;
            var totalCount = _iClaimHelper.GetTotalClaimCountForSearch(searchDto);

            claimPageData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)totalCount / 50));
            claimInsurer.PagedData = claimPageData;
            return View("ClaimListForInsurer", claimInsurer);
        }


        public ActionResult SaveClaim(FormCollection frm)
        {
            var txtClaimNumber = frm.GetValue("txtClaimNumber").AttemptedValue;
            var txtUserName = frm.GetValue("txtUserName").AttemptedValue;
            var txtPassword = frm.GetValue("txtPassword").AttemptedValue;
            var hdnClaimId = frm.GetValue("hdnClaimId").AttemptedValue;
            var txtComment = frm.GetValue("txtComment").AttemptedValue;
            var txtFileNo = frm.GetValue("txtFileNo").AttemptedValue;
            if (txtUserName == "ramesh.k" && txtPassword == "abc@123")
            {
                _iClaimHelper.UpdateClaimNumber(txtClaimNumber, Convert.ToInt32(hdnClaimId), txtComment, txtFileNo);
                //   SendMailCustomer(Convert.ToInt32(hdnClaimId));
                ViewBag.Message = "Successfully updated";
            }
            else
            {
                ViewBag.Message = "Invalid Credentials";
            }
            return View("Index", _iClaimHelper.GetClaimDetail(Convert.ToInt32(hdnClaimId)));
        }

        private void SendMailCustomer(int claimId)
        {
            try
            {
                var claimsDto = _iClaimHelper.GetClaimDetail(claimId);
                var otp = _iClaimHelper.GenerateOtp(claimsDto);
                var template = _iClaimHelper.GetMailTemplate(3);
                var msg = new MailMessage();
                //Sender e-mail address.
                msg.From = new MailAddress("axiscardclaims@gmail.com");
                msg.Subject = template.Subject;
                var url = System.Configuration.ConfigurationManager.AppSettings["url"].ToString(CultureInfo.InvariantCulture) + "Customer/Index?id=" +
                          Encryption.Encrypt(Convert.ToString(claimsDto.CustId), true);
                //var body = "Dear " + claimsDto.CustomersDto.CustomerName + "<br><br> Your dispute number " + claimsDto.SrNumber + " has been opened and we’re looking into the details. Kindly <a href='" + System.Configuration.ConfigurationManager.AppSettings["url"].ToString() + "Customer/Index?id=" + Encryption.Encrypt(Convert.ToString(claimsDto.CustId), true) + "' > click here </a> and submit the <br><br>required documentation for our perusal. <br><br> your OTP : " + otp + "  It will expire after 48 hours <br><br> Kind regards,<br>Axis Bank";
                var body = template.TemplateBody.Replace("@CustomerName", claimsDto.CustomersDto.CustomerName);
                body = body.Replace("@SrNumber", claimsDto.SrNumber);
                body = body.Replace("@url", url);
                body = body.Replace("@otp", otp);

                msg.Body = body;
                msg.To.Add(claimsDto.CustomersDto.EmailId);
                msg.IsBodyHtml = true;
                //your remote SMTP server IP.
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    
                    Credentials = new System.Net.NetworkCredential("axiscardclaims@gmail.com", "accessaxis"),
                    EnableSsl = true
                };
                
                smtp.Send(msg);
                msg = null;
            }
            catch (Exception ex)
            {

            }
        }

        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://uatapps.newindia.co.in/B2B/Axis");
            string usernamePassword = "Axis" + ":" + "Axis#123";
            usernamePassword = Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)); // <--- here.
            CredentialCache mycache = new CredentialCache();
            webRequest.Credentials = mycache;
            webRequest.Headers.Add("Authorization", "Basic " + usernamePassword);
            webRequest.ContentType = "application/soap+xml";
            //webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public ActionResult IntimateClaim(string clmId)
        {

            

            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();

            var transaction = _iClaimHelper.GetNewTransaction("Intimation");
            if (transaction != null)
            {

                var transactionNumber = transaction.TransactionNumber;
                var claimId = Convert.ToInt32(Decryption.Decrypt(clmId, true));
                transaction.ClaimId = claimId;
                var claim = _iClaimHelper.GetClaimDetail(claimId);
                var dateofLoss = claim.DateOfLoss != null ? claim.DateOfLoss.Value : claim.DateSinceIntimation.Value;
                var claimData = "DM_AXIS~~";
                // claimData = claimData + claim.PolicyMasterDto.PolicyNumber + "~~" + claim.DateOfLoss.Value.ToString("dd/MM/yyyy") + "~~00:00:02" + "~~" + claim.DateSinceIntimation.Value.ToString("dd/MM/yyyy") + "~~" + claim.LossTypeMasterParentDto.Code + "~~" + claim.LossTypeMasterDto.Code + "~~";
                claimData = claimData + "13020046161300000016~~" + dateofLoss.ToString("dd/MM/yyyy") + "~~00:00:02" + "~~" + claim.DateSinceIntimation.Value.ToString("dd/MM/yyyy") + "~~" + claim.LossTypeMasterParentDto.Code + "~~" + claim.LossTypeMasterDto.Code + "~~";
                claimData = claimData + "File No:XX, Card No:" + claim.CardNo + ",Custormer name:" + claim.CustomersDto.CustomerName + "~~700091~~" + claim.ClaimAmount + "~~" + transactionNumber + "~~" + transaction.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss");

                soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:typ=""http://iims.services/types/"">
   <soapenv:Header/>
   <soapenv:Body>
      <typ:saveClaimGenericElement>
         <typ:userCode>AXISUSER</typ:userCode>
         <typ:rolecode>SUPERUSER</typ:rolecode>
         <typ:PRetCode>0</typ:PRetCode>
         <typ:stakeCode>POLICY-HOL</typ:stakeCode>
         <typ:PRetErr></typ:PRetErr>
         <typ:claimData>" + claimData + "</typ:claimData>       </typ:saveClaimGenericElement>    </soapenv:Body> </soapenv:Envelope>");
                try
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            string soapResult = rd.ReadToEnd();

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(soapResult);
                            var a = xmlDoc.LastChild;

                            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                            manager.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                            manager.AddNamespace("typ", "http://iims.services/types/");
                            //string query = "/ns1:OrderInfo/pricing";
                            //XmlNodeList nodeList = document.SelectNodes(query);

                            string xpath = "env:Envelope/env:Body/typ:saveClaimGenericResponseElement";
                            var nodes = xmlDoc.SelectNodes(xpath, manager);
                            if (nodes != null && nodes.Count > 0)
                            {
                                var nodesCount = nodes[0].ChildNodes;
                                if (nodesCount.Count > 0)
                                {
                                    var retCode = nodesCount[3].InnerText;
                                    if (retCode == "1" || retCode == "2")
                                    {
                                        var status = _iClaimHelper.UpdateClaimIntimation(claim.ClaimId, nodesCount[9].InnerText,transaction.TransactionNumber);
                                        transaction.Message = "Success";
                                        _iClaimHelper.UpdateTransaction(transaction);
                                    //  _iClaimHelper.UpdateNiaTransactionNumber(transaction);
                                        return RedirectToAction("GetClaimForInsuer");
                                    }
                                    else
                                    {
                                        transaction.Message = nodesCount[2].InnerText;
                                        _iClaimHelper.UpdateTransaction(transaction);
                                        return RedirectToAction("GetClaimForInsuer", new { error = nodesCount[2].InnerText });
                                    }

                                    

                                }
                            }


                        }
                    }
                }

                catch (Exception ex)
                {
                    return RedirectToAction("GetClaimForInsuer", new { error = ex.Message.ToString() });
                }
            }

            return RedirectToAction("GetClaimForInsuer", new { error = "Please try again transaction failed." });
        }


        public ActionResult ApproveRejectClaim(string claimId,string type ="a")
        {
            var clmId = Convert.ToInt32(Decryption.Decrypt(claimId, true));
            var claim = _iClaimHelper.GetClaimDetail(clmId);

            if (type == "a")
            {


                var transaction = _iClaimHelper.GetNewTransaction("Claim Processing");
                if (transaction != null)
                {
                    HttpWebRequest request = CreateWebRequest();
                    XmlDocument soapEnvelopeXml = new XmlDocument();

                    var transactionNumber = claim.NiaTransactionNumber;

                    var claimData = "DM_AXIS~~" + claim.ClaimNumber + "~~" + transactionNumber + "~~" + transaction.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss");

                    soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:typ=""http://iims.services/types/"">
   <soapenv:Header/>
   <soapenv:Body>
      <typ:processClaimTieUpElement>
         <typ:userCode>AXISUSER</typ:userCode>
         <typ:rolecode>SUPERUSER</typ:rolecode>
         <typ:PRetCode>0</typ:PRetCode>
         <typ:PRetErr></typ:PRetErr>
         <typ:stakeCode>POLICY-HOL</typ:stakeCode>
         <typ:claimData>" + claimData + "</typ:claimData>     </typ:processClaimTieUpElement>    </soapenv:Body> </soapenv:Envelope>");
                    try
                    {
                        using (Stream stream = request.GetRequestStream())
                        {
                            soapEnvelopeXml.Save(stream);
                        }
                        using (WebResponse response = request.GetResponse())
                        {
                            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                            {
                                string soapResult = rd.ReadToEnd();

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(soapResult);
                                var a = xmlDoc.LastChild;

                                XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
                                manager.AddNamespace("env", "http://schemas.xmlsoap.org/soap/envelope/");
                                manager.AddNamespace("typ", "http://iims.services/types/");


                                string xpath = "env:Envelope/env:Body/typ:processClaimTieUpResponseElement";
                                var nodes = xmlDoc.SelectNodes(xpath, manager);
                                if (nodes != null && nodes.Count > 0)
                                {
                                    var nodesCount = nodes[0].ChildNodes;
                                    if (nodesCount.Count > 0)
                                    {
                                        var retCode = nodesCount[3].InnerText;
                                        if (retCode == "0")
                                        {
                                            var status = _iClaimHelper.UpdateClaimStatus(new UpdateStatusRequestDto
                                            {
                                                ClaimId = claim.ClaimId,
                                                StatusId = 3,
                                                UpdateUserId = Convert.ToInt32(Session["UserId"]),
                                                UpdateDate = DateTime.Now

                                            }

                                            );
                                            transaction.Message = "Success";
                                            _iClaimHelper.UpdateTransaction(transaction);
                                        }
                                        else
                                        {
                                            transaction.Message = nodesCount[2].InnerText;
                                            _iClaimHelper.UpdateTransaction(transaction);
                                            return RedirectToAction("GetClaimForInsuer", new { error = nodesCount[2].InnerText });
                                        }
                                    }
                                }


                            }
                        }
                    }

                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    return RedirectToAction("GetClaimForInsuer", new { error = "Please try again transaction failed." });
                }
            }
            else
            {

                var status = _iClaimHelper.UpdateClaimStatus(new UpdateStatusRequestDto
                {
                    ClaimId = claim.ClaimId,
                    StatusId = 5,
                    UpdateUserId = Convert.ToInt32(Session["UserId"]),
                    UpdateDate = DateTime.Now

                }
                                        );
            }

            return RedirectToAction("GetClaimForInsuer");

        }
    }
}
