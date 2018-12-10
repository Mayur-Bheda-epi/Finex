using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class ReverseClaimFeedDto
    {
        public string Company { get; set; }
        public string FeedId { get; set; }
        public string RecordType { get; set; }
        public int ClaimId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ClaimIntimationDate { get; set; }
        public decimal ClaimEstimatedAmount { get; set; }
        public DateTime FileSubDate { get; set; }
        public decimal AssessedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
        public string PayeeType { get; set; }
        public string PayeePartyCode { get; set; }
        public string PayeePartyName { get; set; }
        public string InvoiceNumber { get; set; }
        public string ChequePayementNumber { get; set; }
        public DateTime AccidentDate { get; set; }
    }
}
