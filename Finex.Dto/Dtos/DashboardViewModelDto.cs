using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class DashboardViewModelDto
    {
       public List<StateMasterDto> StateMasterDtos { get; set; }
       public List<DashboardDto> DashboardDtos { get; set; }
    }
}
