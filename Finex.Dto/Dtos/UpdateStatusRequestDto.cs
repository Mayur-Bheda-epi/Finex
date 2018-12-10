using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class UpdateStatusRequestDto:CommonDto
    {
       public int ClaimId { get; set; }

       public int StatusId { get; set; }

       public string Comment { get; set; }
    }
}
