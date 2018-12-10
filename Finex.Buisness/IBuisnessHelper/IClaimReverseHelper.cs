using Finex.Dto.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Buisness.IBuisnessHelper
{
    public interface IClaimReverseHelper
    {
        ReverseResponse InsertClaimReverseFeed(ClaimReverseFeedDto requestClaimReverseFeedDto);
        int GetClaimIdByClaimNumber(string cLAIMNUMBER);
    }
}
