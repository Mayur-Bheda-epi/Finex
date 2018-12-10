using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class UpdatePasswordDto
    {
        public int PasswordId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
