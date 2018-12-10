using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class ClaimsDto : CommonDto
    {

        public int ClaimId { get; set; }

        public string ClaimNumber { get; set; }

        public string NiaTransactionNumber { get; set; }

        public int CustId { get; set; }

        public string TransactionSecureOrNot { get; set; }

        public CustomersDto CustomersDto { get; set; }

        [Required(ErrorMessage = "Card No Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Card No. must be a number")]
        [StringLength(16, ErrorMessage = "Card Number length should be 16")]
        public string CardNo { get; set; }

        public int CardTypeId { get; set; }

        public CardTypeMasterDto CardTypeMasterDto { get; set; }

        public DateTime? DateOfLoss { get; set; }

        public string Comment { get; set; }

        public int? CountryId { get; set; }

        public string CountryName { get; set; }

        public int? StateId { get; set; }

        public string StateName { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public string MerchantShop { get; set; }

        public decimal? AmountOfLoss { get; set; }

        public int LossTypeId { get; set; }

        public int? LossTypeIdParent { get; set; }

        public LossTypeMasterDto LossTypeMasterDto { get; set; }

        public LossTypeMasterDto LossTypeMasterParentDto { get; set; }

        public string PassportNo { get; set; }

        public DateTime? DateIntimationBank { get; set; }

        public int StatusId { get; set; }

        public StatusMasterDto StatusMasterDto { get; set; }

        public List<DocumentUploadsDto> DocumentUploadsDtos { get; set; }

        public List<DocumentTypeMasterDto> DocumentTypeMasterDtos { get; set; }

        public string PanNumber { get; set; }

        public DateTime? CardBlockingDate { get; set; }

        public DateTime? DateOfPOS { get; set; }

        public DateTime? DateOfPuchase { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public DateTime? DateOfLeaving { get; set; }

        [Required(ErrorMessage = "Date of Reporting Loss to Bank is required")]
        public DateTime DateLossToBank { get; set; }

        //[Required(ErrorMessage = "Service Request Number is required")]
        public string SrNumber { get; set; }

        public int UserTypeId { get; set; }

        public int? NoOfSecuredTrans { get; set; }

        public int? NoOfUnsecuredTrans { get; set; }

        public decimal CustRelBankInYears { get; set; }

        public int? NoOfCardIssueAccount { get; set; }

        public string WhoBlockCard { get; set; }

        public string RelBlockCard { get; set; }

        public string SmsDisputedTranaction { get; set; }

        public string RealiseMisuseOfCard { get; set; }

        public string LastUsedCard { get; set; }

        public string UseCardInAbroad { get; set; }

        public string WhereWasCustAtDisputed { get; set; }

        public string CardWithNow { get; set; }

        public decimal? SecuredTransactionAmount { get; set; }

        public decimal? UnsecuredTranssactionAmount { get; set; }

        public DateTime? DateSinceIntimation { get; set; }

      //  [Required(ErrorMessage = "Account No Required")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Account No. must be a number")]
        [StringLength(15, ErrorMessage = "Account Number length should be 15")]
        public string AccountNumber { get; set; }

        public int PolicyId { get; set; }


        public decimal? DailyWithdrawalLimit { get; set; }

        public DateTime? DateOfDeath { get; set; }

        [Required(ErrorMessage ="Please enter claim amount")]
        public decimal? ClaimAmount { get; set; }

        public PolicyMasterDto PolicyMasterDto { get; set; }

        public string FileNo { get; set; }

        //[RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string TimeOfReportingBank { get; set; }

        public string LabelCode { get; set; }


        public bool? IsSecuredTransaction { get; set; }

        #region dropdownlist

        public List<CardTypeMasterDto> CardTypeMasterDtos { get; set; }
        public List<CountryMasterDto> CountryMasterDtos { get; set; }
        public List<LossTypeMasterDto> LossTypeMasterDtos { get; set; }
        public List<LossTypeMasterDto> LossTypeMasterParentDtos { get; set; }
        public List<StateMasterDto> StateMasterDtos { get; set; }
        public List<CityMasterDto> CityMasterDtos { get; set; }
        public List<PolicyMasterDto> PolicyMasterDtos { get; set; }


        #endregion


        public List<ClaimReverseFeedDto> ClaimReverseFeedDtoList { get; set; }

    }
}
