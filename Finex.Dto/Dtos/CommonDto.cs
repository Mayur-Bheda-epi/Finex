using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
   public class CommonDto
    {
       public DateTime CreateDate { get; set; }

       public int CreateUserId { get; set; }

       public DateTime UpdateDate { get; set; }

       public int UpdateUserId { get; set; }
    }
}
