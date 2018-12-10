using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class InsurerClaimDto
    {
       public List<StatusMasterDto> StatusMasterDtos { get; set; }

       public List<ClaimsDto> ClaimsDtos { get; set; }

       public PagedData<ClaimsDto> PagedData { get; set; }

       
    }
}
