using Finex.Api.Models;
using Finex.Buisness.IBuisnessHelper;
using Finex.Dto.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Finex.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClaimApiController : ApiController
    {
        private IClaimReverseHelper _iClaimReverseHelper;
        public ClaimApiController()
        {
            _iClaimReverseHelper = DependencyResolve.GetClaimReverseInstance();
        }

        [HttpPost]
        [CustomAuthorize()]
        public HttpResponseMessage ClaimReverseFeed(RequestClaimReverseFeedDto requestClaimReverseFeedDto)
        {
            var claimId = _iClaimReverseHelper.GetClaimIdByClaimNumber(requestClaimReverseFeedDto.CLAIMNUMBER);
            var response = new ResponseReverseFeed();
            if (claimId > 0)
            {
                var claimReverseFeed = new ClaimReverseFeedDto
                {
                    ACCIDENTDATE = requestClaimReverseFeedDto.ACCIDENTDATE,
                    APPROVEDAMOUNT = requestClaimReverseFeedDto.APPROVEDAMOUNT,
                    ASSESSEDAMOUNT = requestClaimReverseFeedDto.ASSESSEDAMOUNT,
                    CHEQUEORPAYMENTNO = requestClaimReverseFeedDto.CHEQUEORPAYMENTNO,
                    CLAIMESTIMATEDAMOUNT = requestClaimReverseFeedDto.CLAIMESTIMATEDAMOUNT,
                    CLAIMINTIMATIONDATE = requestClaimReverseFeedDto.CLAIMINTIMATIONDATE,
                    ClaimId = claimId,
                    COMPANY = requestClaimReverseFeedDto.COMPANY,
                    CUSTOMERNAME = requestClaimReverseFeedDto.CUSTOMERNAME,
                    FEEDID = requestClaimReverseFeedDto.FEEDID,
                    FILESUBDATE = requestClaimReverseFeedDto.FILESUBDATE,
                    INVOICENUMBER = requestClaimReverseFeedDto.INVOICENUMBER,
                    PAIDAMOUNT = requestClaimReverseFeedDto.PAIDAMOUNT,
                    PAYEEPARTYCODE = requestClaimReverseFeedDto.PAYEEPARTYCODE,
                    PAYEEPARTYNAME = requestClaimReverseFeedDto.PAYEEPARTYNAME,
                    PAYEETYPE = requestClaimReverseFeedDto.PAYEETYPE,
                    PAYMENTDATE = requestClaimReverseFeedDto.PAYMENTDATE,
                    PAYMENTMODE = requestClaimReverseFeedDto.PAYMENTMODE,
                    RECORDTYPE = requestClaimReverseFeedDto.RECORDTYPE

                };
                var responseReverseFeed = _iClaimReverseHelper.InsertClaimReverseFeed(claimReverseFeed);
                response.IsSuccess = responseReverseFeed.IsSuccess;
                response.ResponseMsg = responseReverseFeed.Message;

            }
            else
            {
                response.IsSuccess = false;
                response.ResponseMsg = "Claim Number not exist";
            }

         //    response = new ResponseReverseFeed { ResponseMsg = "Claim Number not exist", IsSuccess = false };
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
