using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finex.Api.Models
{
    public class RequestClaimReverseFeedDto
    {
        public string COMPANY { get; set; }
        public int FEEDID { get; set; }
        public string RECORDTYPE { get; set; }
        public string CLAIMNUMBER { get; set; }
        public string POLICYNUMBER { get; set; }
        public string CUSTOMERNAME { get; set; }
        public DateTime CLAIMINTIMATIONDATE { get; set; }
        public decimal CLAIMESTIMATEDAMOUNT { get; set; }
        public DateTime FILESUBDATE { get; set; }
        public decimal ASSESSEDAMOUNT { get; set; }
        public decimal APPROVEDAMOUNT { get; set; }
        public decimal PAIDAMOUNT { get; set; }
        public DateTime PAYMENTDATE { get; set; }
        public string PAYMENTMODE { get; set; }
        public string PAYEETYPE { get; set; }
        public string PAYEEPARTYCODE { get; set; }
        public string PAYEEPARTYNAME { get; set; }
        public string INVOICENUMBER { get; set; }
        public string CHEQUEORPAYMENTNO { get; set; }
        public DateTime ACCIDENTDATE { get; set; }
    }
}