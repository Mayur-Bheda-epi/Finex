using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finex.Buisness.IBuisnessHelper;
using Finex.Data.UnitOfWork;
using Finex.Dto.Dtos;

namespace Finex.Buisness.BuisnessHelper
{
    public class CustomerHelper : ICustomerHelper
    {
        private readonly UnitOfWork _unitOfWork;


        /// <summary>
        /// Public constructor.
        /// </summary>
        public CustomerHelper()
        {
            _unitOfWork = new UnitOfWork();
        }

        public CustomersDto GetCustomerById(int custId)
        {
            var cust = _unitOfWork.CustomerRepository.GetByID(custId);
            return new CustomersDto
                       {
                           CustomerId = cust.CustId,
                           CustomerName = cust.Name,
                           EmailId = cust.Email,
                           MobileNumber = cust.MobileNo

                       };
        }


        public int ValidateCustomerCredentials(OTPDto otpDto)
        {
            var claim = _unitOfWork.ClaimRepository.GetFirst(cr => cr.CustId == otpDto.CustomerId);
            if(claim != null)
            {
                var lastFourDigit = claim.CardNo.Length > 4 ? claim.CardNo.Substring(claim.CardNo.Length - 4, 4):"";
                if(lastFourDigit != otpDto.LastFourDigit.Trim())
                {
                    // invalid card number
                    return 1;
                }
                else
                {
                    var otp =
                        _unitOfWork.OTPRepository.GetFirst(
                            otr =>
                            otr.CustId == otpDto.CustomerId && otr.ExpireTime >= DateTime.Now && otr.IsUsed == false);
                    if(otp != null)
                    {
                        if(otp.OTP1 != otpDto.OTP)
                        {
                            // invalid otp
                            return 2;
                        }
                        
                    }
                    else
                    {
                        // Otp is expire
                        return 3;
                    }
                    // valid otp
                    return 4;

                }
            }
            return 0;
        }


        public ClaimsDto GetClaimByCustId(int custId)
        {
            var claim = _unitOfWork.ClaimRepository.GetFirst(cr => cr.CustId == custId);
            return new ClaimsDto
                       {
                           CardTypeId = claim.CardTypeId != null?claim.CardTypeId.Value:0,
                           LossTypeId = claim.LossTypeId != null?claim.LossTypeId.Value:0,
                           ClaimId = claim.ClaimId,
                           
                       };
        }


       
    }
}
