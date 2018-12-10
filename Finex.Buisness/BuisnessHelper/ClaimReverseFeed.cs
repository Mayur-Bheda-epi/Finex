using Finex.Buisness.IBuisnessHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finex.Dto.Dtos;
using Finex.Data.UnitOfWork;

namespace Finex.Buisness.BuisnessHelper
{
    public class ClaimReverseFeed : IClaimReverseHelper
    {
        private readonly UnitOfWork _unitOfWork;


        /// <summary>
        /// Public constructor.
        /// </summary>
        public ClaimReverseFeed()
        {
            _unitOfWork = new UnitOfWork();
        }

        public int GetClaimIdByClaimNumber(string cLAIMNUMBER)
        {
            var claim = _unitOfWork.ClaimRepository.GetFirst(cl => cl.ClaimNumber == cLAIMNUMBER);
            return claim != null ? claim.ClaimId : 0;
        }

        public ReverseResponse InsertClaimReverseFeed(ClaimReverseFeedDto requestClaimReverseFeedDto)
        {
            try
            {
                var reverseFeedObj = _unitOfWork.ClaimReverseFeedRepository.GetFirst(clr => clr.ClaimId == requestClaimReverseFeedDto.ClaimId && clr.InvoiceNumber.Trim() == requestClaimReverseFeedDto.INVOICENUMBER.Trim());
                if (reverseFeedObj == null)
                {

                    var reverseNew = new Data.ClaimReverseFeed
                    {

                        ApprovedAmount = requestClaimReverseFeedDto.APPROVEDAMOUNT,
                        AssessedAmount = requestClaimReverseFeedDto.ASSESSEDAMOUNT,
                        ChequePayementNumber = requestClaimReverseFeedDto.CHEQUEORPAYMENTNO,
                        ClaimEstimatedAmount = requestClaimReverseFeedDto.CLAIMESTIMATEDAMOUNT,
                        ClaimId = requestClaimReverseFeedDto.ClaimId,
                        
                        Company = requestClaimReverseFeedDto.COMPANY,
                        CustomerName = requestClaimReverseFeedDto.CUSTOMERNAME,
                        FeedId = Convert.ToString(requestClaimReverseFeedDto.FEEDID),
                       
                        InvoiceNumber = requestClaimReverseFeedDto.INVOICENUMBER,
                        PaidAmount = requestClaimReverseFeedDto.PAIDAMOUNT,
                        PayeePartyCode = requestClaimReverseFeedDto.PAYEEPARTYCODE,
                        PayeePartyName = requestClaimReverseFeedDto.PAYEEPARTYNAME,
                        PayeeType = requestClaimReverseFeedDto.PAYEETYPE,
                        
                        PaymentMode = requestClaimReverseFeedDto.PAYMENTMODE,
                        RecordType = requestClaimReverseFeedDto.RECORDTYPE
                    };

                    if (requestClaimReverseFeedDto.ACCIDENTDATE.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                    {
                        reverseNew.AccidentDate = requestClaimReverseFeedDto.ACCIDENTDATE;
                    }
                    if (requestClaimReverseFeedDto.CLAIMINTIMATIONDATE.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                    {
                        reverseNew.ClaimIntimationDate = requestClaimReverseFeedDto.ACCIDENTDATE;
                    }

                    if (requestClaimReverseFeedDto.FILESUBDATE.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                    {
                        reverseNew.FileSubDate = requestClaimReverseFeedDto.FILESUBDATE;
                    }

                    if (requestClaimReverseFeedDto.PAYMENTDATE.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                    {
                        reverseNew.PaymentDate = requestClaimReverseFeedDto.PAYMENTDATE;
                    }

                    _unitOfWork.ClaimReverseFeedRepository.Insert(reverseNew);

                    var claimObj = _unitOfWork.ClaimRepository.GetByID(requestClaimReverseFeedDto.ClaimId);
                    if (claimObj != null)
                    {
                        claimObj.StatusId = 1;
                        claimObj.UpdateDate = DateTime.Now;
                        _unitOfWork.ClaimRepository.Update(claimObj);
                    }

                    _unitOfWork.Save();
                    return new ReverseResponse
                    {
                        IsSuccess = true,
                        Message = "Successfull"
                    };
                }
                else
                {
                    return new ReverseResponse
                    {
                        IsSuccess = false,
                        Message = "Duplicate reverse feed for claim"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReverseResponse
                {
                    IsSuccess = false,
                    Message = ex.InnerException.Message.ToString()

                };
            }
        }
    }
}
