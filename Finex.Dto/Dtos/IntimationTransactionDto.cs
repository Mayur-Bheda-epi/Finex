using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
  public  class IntimationTransactionDto
    {
        public long Id { get; set; }
        public string TransactionNumber { get; set; }
        public string Message { get; set; }
        public string TypeOfService { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ClaimId { get; set; }
    }
}
