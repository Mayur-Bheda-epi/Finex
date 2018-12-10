using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finex.Dto.Dtos;

namespace Finex.Buisness.IBuisnessHelper
{
   public interface ICustomerHelper
    {

       CustomersDto GetCustomerById(int custId);

       int ValidateCustomerCredentials(OTPDto otpDto);

       ClaimsDto GetClaimByCustId(int custId);



    }
}
