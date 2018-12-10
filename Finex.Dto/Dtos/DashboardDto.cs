using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class DashboardDto
    {
       public int LossTypeId { get; set; }

       public string LossType { get; set; }

       public int InitiatedNumber { get; set; }

       public decimal InitiatedAmount { get; set; }

       public int ApprovedNumber { get; set; }

       public decimal ApprovadAmount { get; set; }

       public int RejectedNumber { get; set; }

       public decimal RejectedAmount { get; set; }

       public int SettledNumber { get; set; }

       public decimal SettledAmount { get; set; }

       public int DocumentPendingNumber { get; set; }

       public decimal DocumentPendingAmount { get; set; }

       public int DocumentCompletionNumber { get; set; }

       public decimal DocumentCompletionAmount { get; set; }

    }
}
