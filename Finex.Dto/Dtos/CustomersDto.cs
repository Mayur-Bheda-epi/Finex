using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class CustomersDto
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string CustomerName { get; set; }

      //  [Required(ErrorMessage = "Mobile Number is Required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid Mobile No")]
        [StringLength(10, ErrorMessage = "Mobile Number length should be 10")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Email Id is Required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailId { get; set; }

        public string Address { get; set; }
    }
}
