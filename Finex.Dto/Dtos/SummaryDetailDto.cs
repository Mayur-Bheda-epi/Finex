using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class SummaryDetailDto
    {
       public List<SummaryDto> SummaryDtos { get; set; }

       public List<ClaimsDto> ClaimsDtos { get; set; }
    }
}
