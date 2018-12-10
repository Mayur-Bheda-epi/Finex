using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finex.Buisness.IBuisnessHelper;
using Finex.Dto.Dtos;
using System.Text;
using Finex.Utility;
using System.Net.Mail;
using Ionic.Zip;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Globalization;

namespace Finex.Web.Controllers
{
    [SessionExpireFilter]
    public class ClaimsController : Controller
    {

        private IClaimHelper _iClaimHelper;

        public ClaimsController()
        {
            _iClaimHelper = DependencyResolver.GetClaimInstance();
        }

        //
        // GET: /Claims/

        #region GeneratePdf



        public void GeneratePdf(string clid)
        {

            var clmid = Convert.ToInt32(Decryption.Decrypt(clid, true));
            var claim = _iClaimHelper.GetClaimDetail(Convert.ToInt32(clmid));

            string HTMLContent = "Dear " + claim.CustomersDto.CustomerName + ",<br>";
            HTMLContent = HTMLContent + "<p><font size='2'> This is in reference to " + claim.SrNumber + ", pertaining to the following disputed transactions on your</br> AXIS Bank Credit/Debit Card " + claim.CardNo + " with us:</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>As a valued customer, we assure you that the Bank is committed to resolving this matter at </br> the earliest keeping your best interest in mind.</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>Further, credit has been processed on your account pertaining to the above disputed</br> transaction(s), to ensure no further charges are levied.</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>Please note that the utilization of the credited amount will be restricted till the dispute is</br> permanently resolved. In case liability to pay is assigned to you, the credit will be reversed.</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>The following supporting documents will be needed for further investigation and hence you</br> are requested to keep them ready.</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>";
            var i = 1;
            foreach (var doc in claim.DocumentTypeMasterDtos)
            {
                HTMLContent = HTMLContent + i + " " + doc.DocumentTypeName + "<br>";
                i++;
            }

            HTMLContent = HTMLContent + "</font></p>";

            //HTMLContent = HTMLContent + "<p>  1. Completely filled Chargeback Dispute Form (sample form attached)<br>2. Copies of all passport pages (For international transactions only)<br>3. Copy of destroyed / cut card<br>4. Copy of FIR lodged with the nearest Police Station </p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>You will be intimated regarding submission of the same as the requirement arises.</br> Alternately, you may proactively submit the above at creditcards@axisbank.com or courier</br> us the documents at below address:</font></p> ";
            HTMLContent = HTMLContent + "<p>Axis Bank Ltd.<br>Phone Banking Center, 4th Floor<br>Gigaplex, Plot No. IT-5, MIDC <br>Airoli Knowledge Park,<br>Navi Mumbai – 400708</p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>We will endeavour to resolve this matter within 20 working days from the date of receipt of</br> complete documents.</p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>For any further assistance or clarifications, kindly call us on our 24-hour toll free customer</br> service help line numbers [1800-209-5577, 1800-103-5577 and </br> 91-22-27648000(chargeable and accessible outside India as well)] or send us email at</br> creditcards@axisbank.com</p> </font>";
            HTMLContent = HTMLContent + "<p><font size='2'>Assuring you of our best services at all times.</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>Sincerely,<br>Customer Service – Cards<br>AXIS Bank</font></p> ";
            HTMLContent = HTMLContent + "<p><font size='2'>This is a computer-generated statement and requires no signature</font></p> ";








            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + claim.CustomersDto.CustomerName + "Claim.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(HTMLContent));
            Response.End();
        }

        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }


        #endregion



        public ActionResult Index()
        {
            Session["SuccessMessage"] = null;
            ViewBag.AccountNo = "";
            ViewBag.Name = "";
            ViewBag.CardNo = "";
            ViewBag.MobileNumber = "";
            ViewBag.EmailId = "";
            ViewBag.ViewAll = "false";

            var claimPageData = new PagedData<ClaimsDto>();
            var claimList = new List<ClaimsDto>();
            claimPageData.Data = claimList;
            claimPageData.CurrentPage = 0;
            claimPageData.NumberOfPages = 0;



            //if (Convert.ToInt32(Session["UserTypeId"]) == 1)
            //{
            //    claimList = _iClaimHelper.GetClaimList();
            //}
            //else
            //{
            //    claimList = _iClaimHelper.GetClaimListByUserId(Convert.ToInt32(Session["UserId"]));
            //}

            Session["claimList"] = claimPageData.Data;
            return View(claimPageData);
        }
        public ActionResult ViewAll(int page = 1)
        {
            ViewBag.AccountNo = "";
            ViewBag.Name = "";
            ViewBag.CardNo = "";
            ViewBag.MobileNumber = "";
            ViewBag.EmailId = "";
            ViewBag.ViewAll = "true";
            var claimPageData = new PagedData<ClaimsDto>();
            var claimList = new List<ClaimsDto>();
            //if (Convert.ToInt32(Session["UserTypeId"]) == 1)
            //{
            claimList = _iClaimHelper.GetClaimList(50, page);
            //claimList = _iClaimHelper.GetClaimList(20, page);
            //}
            //else
            //{
            //    claimList = _iClaimHelper.GetClaimListByUserId(Convert.ToInt32(Session["UserId"]));
            //}

            claimPageData.Data = claimList;
            claimPageData.CurrentPage = page;
            var totalCount = _iClaimHelper.GetTotalClaimCount();

            claimPageData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)totalCount / 50));
            Session["claimList"] = claimList;
            return View("Index", claimPageData);
        }

        public ActionResult SearchSummaryClaim(FormCollection frm, string btnSearchSummary)
        {
            var isViewAll = frm.GetValue("hdnViewAll");
            if (btnSearchSummary == "Summary")
            {
                var summarydetail = new SummaryDetailDto();
                var chkClaim = frm.GetValues("chkClaim");
                if (chkClaim != null && chkClaim.Length > 0)
                {
                    var claimIdsList = new List<int>();
                    for (int i = 0; i < chkClaim.Length; i++)
                    {
                        claimIdsList.Add(Convert.ToInt32(Decryption.Decrypt(chkClaim[i], true)));
                    }
                    summarydetail.ClaimsDtos = _iClaimHelper.GetClaimDetailSummary(claimIdsList);
                    summarydetail.SummaryDtos = _iClaimHelper.GetSummaryClaimByClaimIds(claimIdsList);
                }
                else
                {
                    summarydetail.SummaryDtos = _iClaimHelper.GetSummaryClaim(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["UserTypeId"]));
                }

                return View("SummayClaim", summarydetail);
            }

            else if (btnSearchSummary == "Pdf")
            {
                var chkClaim = frm.GetValues("chkClaim");
                if (chkClaim != null && chkClaim.Length > 0)
                {
                    var claimIdsList = new List<int>();
                    for (int i = 0; i < chkClaim.Length; i++)
                    {
                        GeneratePdf(chkClaim[i]);
                    }
                }

            }
            else if (btnSearchSummary == "Export" || btnSearchSummary == "CSV")
            {
                //if (Session["claimList"] != null)
                //{
                //  var claimList = (List<ClaimsDto>)Session["claimList"];
                var claimList = new List<ClaimsDto>();
                var chkClaim = frm.GetValues("chkClaim");
                var claimIdsList = new List<int>();
                if (chkClaim != null)
                {

                    for (int i = 0; i < chkClaim.Length; i++)
                    {
                        claimIdsList.Add(Convert.ToInt32(Decryption.Decrypt(chkClaim[i], true)));
                    }
                    // claimList = claimList.Where(cl => claimIdsList.Contains(cl.ClaimId)).ToList();
                }
                var claimSearchDto = new ClaimSearchDto();

                if (claimIdsList.Count == 0)
                {

                    var hdnAccountNo = frm.GetValue("hdnAccountNo");
                    var hdnName = frm.GetValue("hdnName");
                    var hdnCardNumber = frm.GetValue("hdnCardNumber");
                    var hdnEmailId = frm.GetValue("hdnEmailId");
                    var hdnMobileNo = frm.GetValue("hdnMobileNo");


                    claimSearchDto.CardNo = hdnCardNumber.AttemptedValue;
                    claimSearchDto.Name = hdnName.AttemptedValue;

                    claimSearchDto.AccountNo = hdnAccountNo.AttemptedValue;
                    claimSearchDto.EmailId = hdnEmailId.AttemptedValue;
                    claimSearchDto.MobileNo = hdnMobileNo.AttemptedValue;
                    claimSearchDto.UserId = Convert.ToInt32(Session["UserId"]);
                    claimSearchDto.UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

                }

                claimList = _iClaimHelper.GetClaimsForExport(claimIdsList, claimSearchDto, Convert.ToBoolean(isViewAll.AttemptedValue));

                if (btnSearchSummary == "Export")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table border=`" + "1px" + "`b>");
                    //code section for creating header column
                    sb.Append("<tr>");
                    sb.Append("<td><b><font face=Arial size=2>Claim Number</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Claim Status</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Claimant Name</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Card Number</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Account Number</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Sr Number</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Card Type</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Type of loss</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Label Code</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Date of Loss</font></b></td>");
                    //sb.Append("<td><b><font face=Arial size=2>Date of Intimation</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Date Since Intimation</font></b></td>");

                    sb.Append("<td><b><font face=Arial size=2>File Number</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>No of Secured Transaction</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>No Of unsecured transaction</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Claim Amount</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Place Of Loss</font></b></td>");
                    sb.Append("<td><b><font face=Arial size=2>Comments</font></b></td>");



                    sb.Append("</tr>");

                    //code for creating excel data
                    foreach (var item in claimList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='mso-number-format:\\@'><font face=Arial size=" + "2px" + ">" + item.ClaimNumber + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.StatusMasterDto.StatusName + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.CustomersDto.CustomerName + "</font></td>");
                        sb.Append("<td style='mso-number-format:\\@'><font face=Arial size=" + "2px" + ">" + item.CardNo + "</font></td>");
                        sb.Append("<td style='mso-number-format:\\@'><font face=Arial size=" + "2px" + ">" + item.AccountNumber + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.SrNumber + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.CardTypeMasterDto.CardTypeName + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.LossTypeMasterDto.LossType + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.LabelCode + "</font></td>");

                        sb.Append("<td style='mso-number-format:\\@'><font face=Arial size=" + "2px" + ">" + (item.DateOfLoss != null && item.DateOfLoss.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateOfLoss.Value.ToString("dd-MMM-yyyy") : string.Empty) + "</font></td>");
                        //sb.Append("<td><font face=Arial size=" + "2px" + ">" + (item.DateIntimationBank != null && item.DateIntimationBank.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateIntimationBank.Value.ToShortDateString() : string.Empty) + "</font></td>");
                        sb.Append("<td style='mso-number-format:\\@'><font face=Arial size=" + "2px" + ">" + (item.DateSinceIntimation != null && item.DateSinceIntimation.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateSinceIntimation.Value.ToString("dd-MMM-yyyy") : string.Empty) + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.FileNo + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + Convert.ToString(item.NoOfSecuredTrans) + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + Convert.ToString(item.NoOfUnsecuredTrans) + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.ClaimAmount + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.CityName + " " + item.StateName + " " + item.CountryName + "</font></td>");
                        sb.Append("<td><font face=Arial size=" + "2px" + ">" + " " + "</font></td>");


                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    HttpContext.Response.AddHeader("content-disposition",
                                                   "attachment; filename=ClaimList_" + DateTime.Now.Year.ToString() + ".xls");
                    this.Response.ContentType = "application/vnd.ms-excel";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());

                    return File(buffer, "application/vnd.ms-excel");
                }
                else
                {

                    StringWriter sw = new StringWriter();

                    sw.WriteLine("\"Claim Number\",\"Claim Status\",\"Claimant Name\",\"Card Number\",\"Account Number\",\"Sr Number\",\"Card Type\",\"Type of loss\",\"Date of Loss\",\"Date Since Intimation\",\"File Number\",\"No of Secured Transaction\",\"No Of unsecured transaction\",\"Claim Amount\",\"Comments\"");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=ClaimList.csv");
                    Response.ContentType = "text/csv";

                    foreach (var item in claimList)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\"",
                                                   item.ClaimNumber,
                                                   item.StatusMasterDto.StatusName,
                                                   item.CustomersDto.CustomerName,
                                                    item.CardNo,
                                                   item.AccountNumber,
                                                   item.SrNumber,
                                                   item.CardTypeMasterDto.CardTypeName,
                                                   item.LossTypeMasterDto.LossType,
                                                   (item.DateOfLoss != null && item.DateOfLoss.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateOfLoss.Value.ToShortDateString() : string.Empty),
                                                   (item.DateSinceIntimation != null && item.DateSinceIntimation.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateSinceIntimation.Value.ToShortDateString() : string.Empty),
                                                   item.FileNo,
                                                   Convert.ToString(item.NoOfSecuredTrans),
                                                   Convert.ToString(item.NoOfUnsecuredTrans),
                                                   item.ClaimAmount,
                                                   item.Comment
                                                   ));
                    }

                    //Response.Write(sw.ToString());

                    //Response.End();


                    //string csv = string.Concat(from item in claimList
                    //                           select item.ClaimNumber + ","
                    //                           + item.StatusMasterDto.StatusName + ","
                    //                           + item.CustomersDto.CustomerName + ","
                    //                           + item.CardNo + ","
                    //                           + item.AccountNumber + ","
                    //                           + item.SrNumber + ","
                    //                           + item.CardTypeMasterDto.CardTypeName + ","
                    //                           + item.LossTypeMasterDto.LossType + ","
                    //                           + (item.DateOfLoss != null && item.DateOfLoss.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateOfLoss.Value.ToShortDateString() : string.Empty) + ","
                    //                                                                          + (item.DateSinceIntimation != null && item.DateSinceIntimation.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString() ? item.DateSinceIntimation.Value.ToShortDateString() : string.Empty) + ","
                    //                                                                                                                         + item.FileNo + ","
                    //                                                                                                                                                                        + Convert.ToString(item.NoOfSecuredTrans) + ","
                    //                                                                                                                                                                        + Convert.ToString(item.NoOfUnsecuredTrans) + ","
                    //                                                                                                                                                                        + item.ClaimAmount + ","
                    //                                                                                                                                                                        + item.Comment + "\n");
                    //return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report.csv");
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sw.ToString());

                    return File(buffer, "text/csv");
                }
            }
            else if (btnSearchSummary == "Delete")
            {
                var chkClaim = frm.GetValues("chkClaim");
                if (chkClaim != null && chkClaim.Length > 0)
                {
                    var claimIdsList = new List<int>();
                    for (int i = 0; i < chkClaim.Length; i++)
                    {
                        claimIdsList.Add(Convert.ToInt32(Decryption.Decrypt(chkClaim[i], true)));
                    }
                    _iClaimHelper.DeleteClaim(claimIdsList);
                    //var claimList = _iClaimHelper.GetClaimList(20,1);


                    //Session["claimList"] = claimList;
                    //return View("Index", claimList);

                    if (Convert.ToBoolean(isViewAll.AttemptedValue))
                        return RedirectToAction("ViewAll", new { page = 1 });
                }
            }
            //  var txtPanNo = frm.GetValue("txtPanNo");
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
                UserTypeId = Convert.ToInt32(Session["UserTypeId"])
            };

            var claimPageData = new PagedData<ClaimsDto>();

            var claimSeachList = _iClaimHelper.GetClaimSearch(searchDto, 50, 1);
            //var claimSeachList = _iClaimHelper.GetClaimSearch(searchDto, 20, 1);
            claimPageData.Data = claimSeachList;
            claimPageData.CurrentPage = 1;
            var totalCount = _iClaimHelper.GetTotalClaimCountForSearch(searchDto);

            claimPageData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)totalCount / 50));

            ViewBag.IsSearch = true;
            ViewBag.ViewAll = txtAccountNo.AttemptedValue != string.Empty || txtName.AttemptedValue != string.Empty || txtCardNumber.AttemptedValue != string.Empty || txtMobileNo.AttemptedValue != string.Empty || txtEmailId.AttemptedValue != string.Empty ? "false" : "true";
            ViewBag.AccountNo = txtAccountNo.AttemptedValue;
            ViewBag.Name = txtName.AttemptedValue;
            ViewBag.CardNo = txtCardNumber.AttemptedValue;
            ViewBag.MobileNumber = txtMobileNo.AttemptedValue;
            ViewBag.EmailId = txtEmailId.AttemptedValue;
            Session["claimList"] = claimSeachList;
            return View("Index", claimPageData);

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
                UserTypeId = Convert.ToInt32(Session["UserTypeId"])
            };

            var claimPageData = new PagedData<ClaimsDto>();
            var claimSeachList = _iClaimHelper.GetClaimSearch(searchDto, 50, page);
            //var claimSeachList = _iClaimHelper.GetClaimSearch(searchDto, 20, page);
            claimPageData.Data = claimSeachList;
            claimPageData.CurrentPage = page;
            var totalCount = _iClaimHelper.GetTotalClaimCountForSearch(searchDto);

            claimPageData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)totalCount / 50));
            return View("Index", claimPageData);
        }


        public ActionResult GetClaimDetail(string id)
        {
            var clid = Decryption.Decrypt(id, true);
            return View("ClaimDetail", _iClaimHelper.GetClaimDetail(Convert.ToInt32(clid)));
        }


        public ActionResult GetDashboard()
        {
            var dashboardViewModel = new DashboardViewModelDto();
            dashboardViewModel.DashboardDtos =
                _iClaimHelper.GetDashboard(Convert.ToInt32(Session["UserTypeId"]) == 1
                                               ? 0
                                               : Convert.ToInt32(Session["UserId"]));
            return View("Dashboard", dashboardViewModel);
        }

        public ActionResult ExportClaim()
        {
            if (Session["claimList"] != null)
            {
                var claimList = (List<ClaimsDto>)Session["claimList"];

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
                    sb.Append("<td style='mso-number-format:0'><font face=Arial size=" + "2px" + ">" + item.CardNo + "</font></td>");
                    sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.CardTypeMasterDto.CardTypeName + "</font></td>");
                    sb.Append("<td><font face=Arial size=" + "2px" + ">" + item.LossTypeMasterDto.LossType + "</font></td>");
                    sb.Append("<td><font face=Arial size=" + "2px" + ">" + " " + "</font></td>");


                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                HttpContext.Response.AddHeader("content-disposition",
                                               "attachment; filename=ClaimList_" + DateTime.Now.Year.ToString() + ".xls");
                this.Response.ContentType = "application/vnd.ms-excel";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());

                return File(buffer, "application/vnd.ms-excel");
            }
            return RedirectToAction("Index");
        }

        //public ActionResult GetLossTypeColumns(int cardtypeid,int id, int claimId = 0)
        public ActionResult GetLossTypeColumns(int id, int claimId = 0)
        {
            if (claimId > 0)
            {
                return PartialView("_AddClaimPartial", new LossTypeIdDto
                {

                    //CardTypeId = cardtypeid,
                    LossTypeId = id,
                    ClaimsDto =
                                                                   claimId > 0
                                                                       ? _iClaimHelper.GetClaimByClaimIdForEdit(claimId)
                                                                       : null
                });
            }
            return PartialView("_AddClaimPartial", new LossTypeIdDto
            {
                //CardTypeId = cardtypeid,
                LossTypeId = id,

            });
        }

        public ActionResult AddClaim()
        {
            var claimDto = new ClaimsDto
            {
                DateIntimationBank = DateTime.Now,
                DateOfLoss = DateTime.Now,
                DateLossToBank = DateTime.Now,
                CountryMasterDtos = _iClaimHelper.GetCountrys(),
                CardTypeMasterDtos = _iClaimHelper.GetCardTypeMaster(),

                //changes for NIA integration 
                //LossTypeMasterDtos = _iClaimHelper.GetLossTypeMaster(),
                LossTypeMasterParentDtos = _iClaimHelper.GetLossTypeParentList(),
                LossTypeMasterDtos = new List<LossTypeMasterDto>(),
                StateMasterDtos = new List<StateMasterDto>(),
                CityMasterDtos = new List<CityMasterDto>(),
                PolicyMasterDtos = new List<PolicyMasterDto>()
            };
            return View("AddEditClaim", claimDto);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveClaims(ClaimsDto claimsDto, FormCollection frm)
        {
            try
            {
                var ddlCardType = frm.GetValue("ddlCardType");
                var ddlCountry = frm.GetValue("ddlCountry");
                var ddlState = frm.GetValue("ddlState");
                var ddlCity = frm.GetValue("ddlCity");
                var ddlLossTypeParent = frm.GetValue("ddlLossTypeParent");
                var ddlLossType = frm.GetValue("ddlLossType");
                var txtClaimNumber = frm.GetValue("txtClaimNumber");
                var ddlPolicyNumber = frm.GetValue("ddlPolicyNumber");
                var IsSecuredTransaction = frm.GetValue("IsSecuredTransaction");


                var userTypeId = Convert.ToInt32(Session["UserTypeId"]);
                claimsDto.ClaimNumber = userTypeId == 1 && txtClaimNumber != null ? txtClaimNumber.AttemptedValue : "";
                claimsDto.UserTypeId = userTypeId;
                claimsDto.StatusId = 9;
                claimsDto.CountryId = ddlCountry != null && Convert.ToInt32(ddlCountry.AttemptedValue) > 0 ? Convert.ToInt32(ddlCountry.AttemptedValue) : (int?)null;
                claimsDto.StateId = ddlState != null && Convert.ToInt32(ddlState.AttemptedValue) > 0 ? Convert.ToInt32(ddlState.AttemptedValue) : (int?)null;
                claimsDto.CityId = ddlCity != null && Convert.ToInt32(ddlCity.AttemptedValue) > 0 ? Convert.ToInt32(ddlCity.AttemptedValue) : (int?)null;
                claimsDto.CardTypeId = ddlCardType != null ? Convert.ToInt32(ddlCardType.AttemptedValue) : 0;
                claimsDto.LossTypeIdParent = ddlLossTypeParent != null ? Convert.ToInt32(ddlLossTypeParent.AttemptedValue) : (int?)null;
                claimsDto.LossTypeId = ddlLossType != null ? Convert.ToInt32(ddlLossType.AttemptedValue) : 0;

                claimsDto.PolicyId = ddlPolicyNumber != null ? Convert.ToInt32(ddlPolicyNumber.AttemptedValue) : 0;
                claimsDto.CreateUserId = Convert.ToInt32(Session["UserId"]);
                claimsDto.UpdateUserId = Convert.ToInt32(Session["UserId"]);
                claimsDto.CreateDate = DateTime.Now;
                claimsDto.UpdateDate = DateTime.Now;

                var isSuccess = false;
                if (claimsDto.ClaimId > 0)
                {
                    isSuccess = _iClaimHelper.UpdateClaim(claimsDto);
                    //   SendMail(claimsDto);
                }
                else
                {
                    //System Date dependent Model Mapping MVC BUG
                    //var ACX = frm.GetValue("DateLossToBank");
                    //DateTime tmp = new DateTime();

                    //tmp = Convert.ToDateTime(ACX.AttemptedValue);
                    ////DateTime.TryParse(ACX.AttemptedValue, out tmp);
                    //if (tmp.CompareTo(new DateTime()) == 0)
                    //{
                    //    claimsDto.DateLossToBank = DateTime.Now;
                    //}
                    isSuccess = _iClaimHelper.AddClaim(claimsDto);
                    //if (isSuccess)
                    //    SendMail(claimsDto);
                }

                if (isSuccess)
                {

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                new Logging.Log.LogWriter().ErrorLogging("Error in saveclaim: " + ex.Message);
            }

            claimsDto.CardTypeMasterDtos = _iClaimHelper.GetCardTypeMaster();
            claimsDto.LossTypeMasterDtos = _iClaimHelper.GetLossTypeByCardType(claimsDto.CardTypeId);
            claimsDto.CountryMasterDtos = _iClaimHelper.GetCountrys();
            claimsDto.StateMasterDtos = _iClaimHelper.GetStateByCountryId(claimsDto.CountryId != null ? claimsDto.CountryId.Value : 0);
            claimsDto.CityMasterDtos = _iClaimHelper.GetCityByStateId(claimsDto.StateId != null ? claimsDto.StateId.Value : 0);
            claimsDto.PolicyMasterDtos = _iClaimHelper.GetPolicyMasterByCardTypeId(claimsDto.CardTypeId);
            return View("AddEditClaim", claimsDto);
        }

        private void SendMail(ClaimsDto claimsDto)
        {
            try
            {
                var policy = _iClaimHelper.GetPolicyMasterByCardTypeId(claimsDto.CardTypeId).FirstOrDefault();

                var amountOfLoss = claimsDto.ClaimAmount != null && claimsDto.ClaimAmount > 0 ? Convert.ToString(claimsDto.AmountOfLoss) : "";
                var url = System.Configuration.ConfigurationManager.AppSettings["url"].ToString() + "Insurer/Index?id=" +
                          Encryption.Encrypt(Convert.ToString(claimsDto.ClaimId), true);
                var template = _iClaimHelper.GetMailTemplate(4);

                var msg = new MailMessage();
                //Sender e-mail address.
                msg.From = new MailAddress("axiscardclaims@gmail.com");
                // msg.Subject = "Claim Intimation for " + claimsDto.CustomersDto.CustomerName + " under Policy No." + policy.PolicyNumber;
                var sub = template.Subject.Replace("@CustomerName", claimsDto.CustomersDto.CustomerName);
                sub = sub.Replace("@PolicyNumber", policy != null ? policy.PolicyNumber : "");
                msg.Subject = sub;
                //    var bodyT = "Dear Sir/Madam,<br><br> Kindly register the claim as mentioned below:<br><br> <table border='1'> <tr><td> Card No.</td> <td>@CardNo</td></tr> <tr> <td> Type of Card</td><td>@CardTypeName</td></tr><tr><td>Name of Card Holder </td><td>@CustomerName </td></tr><tr><td>Account Number</td> <td>@AccountNumber</td></tr><tr><td>Loss Amount </td><td>@amountOfLoss </td></tr><tr><td>Loss Type </td><td>@LossTypeName </td></tr></table> <br><br>You can find this claim on the portal by <a href='@url' > clicking here </a><br><br> Thanks";
                var card =
                  _iClaimHelper.GetCardTypeMaster().Where(cr => cr.CardTypeId == claimsDto.CardTypeId).FirstOrDefault();
                var lossTye =
                    _iClaimHelper.GetLossTypeMaster().Where(lt => lt.LossTypeId == claimsDto.LossTypeId).FirstOrDefault();
                var body = template.TemplateBody.Replace("@CardNo", claimsDto.CardNo);
                body = body.Replace("@CardTypeName", card != null ? card.CardTypeName : "");
                body = body.Replace("@CustomerName", claimsDto.CustomersDto.CustomerName);
                body = body.Replace("@AccountNumber", claimsDto.AccountNumber);
                body = body.Replace("@amountOfLoss", amountOfLoss);
                body = body.Replace("@LossTypeName", lossTye != null ? lossTye.LossType : "");
                body = body.Replace("@url", url);

                msg.Body = body;

                msg.To.Add("sagar.chilamkurty@newindia.co.in");

                msg.CC.Add("vaibhav.velle@almondz.com,omkar.arawari@almondz.com");

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
                new Logging.Log.LogWriter().ErrorLogging("Send Mail Claim: " + ex.Message);

            }
        }



        public ActionResult SummayClaim(string clid)
        {
            var summarydetail = new SummaryDetailDto();
            summarydetail.SummaryDtos = _iClaimHelper.GetSummaryClaim(Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["UserTypeId"]));
            return View(summarydetail);
        }

        public ActionResult UploadDocument(HttpPostedFileBase[] uploadFiles, FormCollection frm)
        {
            var hdnClaimId = frm.GetValue("hdnClaimId");
            var hdnLossType = frm.GetValue("hdnLossType");
            var hdnCustomer = frm.GetValue("hdnCustomer");
            var hdnDocumentTypeIds = frm.GetValues("hdnDocumentTypeIds");
            var hdnDocumentTypeName = frm.GetValues("hdnDocumentTypeName");
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
                    if (hdnDocumentTypeName != null && hdnDocumentTypeName[i] == "Other")
                    {
                        idOfOther = Convert.ToInt32(hdnDocumentTypeIds[i]);
                    }
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
            return View("UploadDocument", documentList);

            //return RedirectToAction("Index");
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
                msg.CC.Add("alpesh.m@almondz.com,shweta.poojary@almondz.com,anurag.rai@almondz.com,arjun.thakur@almondz.com,vaibhav.velle@almondz.com");

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

        public ActionResult ViewDocumentUploaded(string clid)
        {
            var clmid = Convert.ToInt32(Decryption.Decrypt(clid, true));
            ViewBag.Clmid = clmid;
            var claim = _iClaimHelper.GetClaimByClaimId(clmid);
            ViewBag.CustomerName = claim.CustomersDto.CustomerName;
            ViewBag.LossType = claim.LossTypeMasterDto.LossType;
            return View("ViewDownloadedDocument", _iClaimHelper.GetDocumentUploadedbyClaimId(clmid));
        }

        public void DownloadZip(FormCollection frm)
        {
            var clmid = frm.GetValue("hdnClmIdDownloadZip").AttemptedValue;
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");


                var documents = _iClaimHelper.GetDocumentUploadedbyClaimId(Convert.ToInt32(clmid));
                foreach (var documentUploadsDto in documents)
                {
                    string filePath = Server.MapPath("~/UploadDocuments/") + documentUploadsDto.DocumentPath;
                    zip.AddFile(filePath, "Files");
                }


                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("DocumentsZip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();

            }


        }




        public JsonResult GetStateByCountryId(int countryId)
        {
            try
            {
                var result = _iClaimHelper.GetStateByCountryId(countryId);
                IList<SelectListItem> data = new List<SelectListItem>();
                foreach (var state in result)
                {

                    data.Add(new SelectListItem()
                    {
                        Text = state.StateName,
                        Value = Convert.ToString(state.StateId),
                    });
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPolicyByCardTypeId(int cardTypeId)
        {
            try
            {
                var result = _iClaimHelper.GetPolicyMasterByCardTypeId(cardTypeId);
                IList<SelectListItem> data = new List<SelectListItem>();
                foreach (var policy in result)
                {
                    data.Add(new SelectListItem()
                    {
                        Text = policy.PolicyNumber,
                        Value = Convert.ToString(policy.PolicyId),
                    });
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPolicyByCardTypeIdAndLossTypeId(int cardTypeId, int lossTypeId, int IsSecured = 0)
        {
            try
            {
                var result = _iClaimHelper.GetPolicyByCardTypeIdAndLossTypeId(cardTypeId, lossTypeId, IsSecured == 0 ? false : true);
                IList<SelectListItem> data = new List<SelectListItem>();
                foreach (var policy in result)
                {
                    data.Add(new SelectListItem()
                    {
                        Text = policy.PolicyNumber,
                        Value = Convert.ToString(policy.PolicyId),
                    });
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }
        }




        public JsonResult GetCityByStateId(int stateId)
        {
            try
            {
                var result = _iClaimHelper.GetCityByStateId(stateId);
                IList<SelectListItem> data = new List<SelectListItem>();
                foreach (var city in result)
                {
                    data.Add(new SelectListItem()
                    {
                        Text = city.CityName,
                        Value = Convert.ToString(city.CityId),
                    });
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLossTypeByCardType(int cardTypeId)
        {
            try
            {
                var result = _iClaimHelper.GetLossTypeByCardType(cardTypeId);
                IList<SelectListItem> data = new List<SelectListItem>();
                foreach (var loss in result)
                {
                    data.Add(new SelectListItem()
                    {
                        Text = loss.LossType,
                        Value = Convert.ToString(loss.LossTypeId),
                    });
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNatureOfLossTypeLossTypeId(int lossTypeId)
        {
            try
            {
                var result = _iClaimHelper.GetNatureOfLossTypeLossTypeId(lossTypeId);
                IList<SelectListItem> data = new List<SelectListItem>();
                foreach (var loss in result)
                {
                    data.Add(new SelectListItem()
                    {
                        Text = loss.LossType,
                        Value = Convert.ToString(loss.LossTypeId),
                    });
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }
        }


        #region Upload Documents

        public ActionResult GetUploadDocument(string clid, string ltId, string csId, string i = "")
        {
            var clmId = Convert.ToInt32(Decryption.Decrypt(clid, true));
            var ltmId = Convert.ToInt32(Decryption.Decrypt(ltId, true));
            var csmId = Convert.ToInt32(Decryption.Decrypt(csId, true));

            ViewBag.ClaimId = clmId;
            ViewBag.LossTypeId = clmId;
            ViewBag.CustomerId = csmId;
            ViewBag.camFrom = i;
            ViewBag.EncyptClaimId = clid;
            var documentList = _iClaimHelper.GetUploadDocumentMaster(ltmId);
            return View("UploadDocument", documentList);

        }

        public ActionResult EditClaim(string clid)
        {
            var clmid = Convert.ToInt32(Decryption.Decrypt(clid, true));
            var claim = _iClaimHelper.GetClaimByClaimIdForEdit(clmid);
            claim.CardTypeMasterDtos = _iClaimHelper.GetCardTypeMaster();
            //changes for NIA integration
            //   claim.LossTypeMasterDtos = _iClaimHelper.GetLossTypeByCardType(claim.CardTypeId);
            claim.LossTypeMasterParentDtos = _iClaimHelper.GetLossTypeParentList();
            claim.LossTypeMasterDtos = _iClaimHelper.GetNatureOfLossTypeLossTypeId(claim.LossTypeIdParent.Value);
            claim.CountryMasterDtos = _iClaimHelper.GetCountrys();
            claim.StateMasterDtos = _iClaimHelper.GetStateByCountryId(claim.CountryId != null ? claim.CountryId.Value : 0);
            claim.CityMasterDtos = _iClaimHelper.GetCityByStateId(claim.StateId != null ? claim.StateId.Value : 0);
            claim.PolicyMasterDtos = _iClaimHelper.GetPolicyMasterByCardTypeId(claim.CardTypeId);
            return View("AddEditClaim", claim);
        }

        #endregion

        #region Update Document Completion Status

        public ActionResult UpdateDocumentStatus()
        {
            ViewBag.StatusId = 0;
            var statusList = new List<int>();
            statusList.Add(6);
            statusList.Add(7);
            var claimList = _iClaimHelper.GetClaimsByStatusId(statusList);
            return View("UpdateDocumentStatus", claimList);
        }

        public ActionResult SearchAndUpdateDocumentStatus(FormCollection frm, string btnSearchSummary)
        {
            var ddlStatus = frm.GetValue("ddlStatus").AttemptedValue;
            ViewBag.StatusId = ddlStatus;
            if (btnSearchSummary == "UpdateStatus")
            {
                var chkClaim = frm.GetValues("chkClaim");
                if (chkClaim != null)
                {
                    var claimIdList = new List<int>();
                    for (var i = 0; i < chkClaim.Length; i++)
                    {
                        claimIdList.Add(Convert.ToInt32(chkClaim[i]));
                    }

                    var isUpdate = _iClaimHelper.UpdateDocumentCompletionStatus(claimIdList, Convert.ToInt32(Session["UserId"]));
                }
            }
            var statusList = new List<int>();
            statusList.Add(Convert.ToInt32(ddlStatus));
            var claimList = _iClaimHelper.GetClaimsByStatusId(statusList);
            return View("UpdateDocumentStatus", claimList);
        }

        #endregion

    }
}
