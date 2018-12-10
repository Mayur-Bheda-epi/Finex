using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class DashboardDataDto
    {
       public int LossTypeId { get; set; }
       public string LossTypeName { get; set; }
       public int StatusId { get; set; }
       public decimal Sum { get; set; }
       public int Count { get; set; }

    }
}
