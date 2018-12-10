using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
  public  class MailTemplatedto
    {
      public int TemplateId { get; set; }

      public string Subject { get; set; }

      public string TemplateName { get; set; }

      public string TemplateBody { get; set; }
    }
}
