//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Finex.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Claim
    {
        public Claim()
        {
            this.DocumentUploads = new HashSet<DocumentUpload>();
            this.ClaimReverseFeeds = new HashSet<ClaimReverseFeed>();
        }
    
        public int ClaimId { get; set; }
        public string ClaimNumber { get; set; }
        public Nullable<int> CustId { get; set; }
        public string CardNo { get; set; }
        public Nullable<int> CardTypeId { get; set; }
        public Nullable<System.DateTime> DateOfLoss { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string MerchantShop { get; set; }
        public Nullable<decimal> AmountOfLoss { get; set; }
        public Nullable<int> LossTypeId { get; set; }
        public string PassportNo { get; set; }
        public Nullable<System.DateTime> DateIntimationBank { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> CardBlockingDate { get; set; }
        public Nullable<System.DateTime> DateOfPuchase { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public Nullable<System.DateTime> DateOfLeaving { get; set; }
        public Nullable<System.DateTime> DateOfPOS { get; set; }
        public string PanNumber { get; set; }
        public string Comment { get; set; }
        public string SrNumber { get; set; }
        public Nullable<System.DateTime> DateLossToBank { get; set; }
        public Nullable<int> NoOfSecuredTrans { get; set; }
        public Nullable<int> NoOfUnsecuredTrans { get; set; }
        public Nullable<decimal> CustRelBankInYears { get; set; }
        public Nullable<int> NoOfCardIssueAccount { get; set; }
        public string WhoBlockCard { get; set; }
        public string RelBlockCard { get; set; }
        public string SmsDisputedTranaction { get; set; }
        public string RealiseMisuseOfCard { get; set; }
        public string LastUsedCard { get; set; }
        public string UseCardInAbroad { get; set; }
        public string WhereWasCustAtDisputed { get; set; }
        public string CardWithNow { get; set; }
        public string UploadDocComment { get; set; }
        public Nullable<decimal> SecuredTransactionAmount { get; set; }
        public Nullable<decimal> UnsecuredTranssactionAmount { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<int> PolicyId { get; set; }
        public string FileNo { get; set; }
        public Nullable<decimal> DailyWithdrawalLimit { get; set; }
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        public Nullable<decimal> ClaimAmount { get; set; }
        public string TimeOfReportingBank { get; set; }
        public string ApproveRejectComment { get; set; }
        public Nullable<System.DateTime> DateSinceIntimation { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string LabelCode { get; set; }
        public Nullable<int> ParentLossTypeId { get; set; }
        public string NiaTransactionNumber { get; set; }
        public Nullable<bool> IsSecuredTransaction { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public Nullable<int> TransactionTypeId { get; set; }
    
        public virtual CardTypeMaster CardTypeMaster { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual LossTypeMaster LossTypeMaster { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
        public virtual ICollection<DocumentUpload> DocumentUploads { get; set; }
        public virtual CityMaste CityMaste { get; set; }
        public virtual CountryMaster CountryMaster { get; set; }
        public virtual PolicyMaster PolicyMaster { get; set; }
        public virtual LossTypeMaster LossTypeMaster1 { get; set; }
        public virtual ICollection<ClaimReverseFeed> ClaimReverseFeeds { get; set; }
    }
}