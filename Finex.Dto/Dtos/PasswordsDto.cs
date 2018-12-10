using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class PasswordsDto
    {

        public int PasswordId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
