using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class OTPDto
    {

        public int OTPId { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string LastFourDigit { get; set; }

        public string OTP { get; set; }

        public DateTime ExpireTime { get; set; }

        public bool IsUsed { get; set; }
    }
}
