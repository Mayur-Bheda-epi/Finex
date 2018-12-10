using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class ClaimSearchDto
    {
       public string Name { get; set; }

       public string CardNo { get; set; }

       public string PanNo { get; set; }

       public string AccountNo { get; set; }

       public string MobileNo { get; set; }

       public string EmailId { get; set; }

       public int UserId { get; set; }

       public int UserTypeId { get; set; }
    }
}
