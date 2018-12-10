using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class UserMasterDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        public int UserTypeId { get; set; }

        
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid Mobile No")]
        [StringLength(10, ErrorMessage = "Mobile Number length should be 10")]
        public string Mobile { get; set; }

        public string UserType { get; set; }

         [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }

        public PasswordsDto PasswordsDto { get; set; }

        public List<UserTypesDto> UserTypesDtos { get; set; }
    }
}
