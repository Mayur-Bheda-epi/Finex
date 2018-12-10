using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Finex.Dto.Dtos;

namespace Finex.Buisness.IBuisnessHelper
{
  public  interface IClaimHelper
    {
   


      List<ClaimsDto> GetClaimListByUserId(int userId);

      List<CountryMasterDto> GetCountrys();

      List<StateMasterDto> GetStateByCountryId(int countryId);

      List<CityMasterDto> GetCityByStateId(int stateId);

      bool AddClaim(ClaimsDto claimsDto);

      List<CardTypeMasterDto> GetCardTypeMaster();

      List<LossTypeMasterDto> GetLossTypeMaster();

      List<LossTypeMasterDto> GetLossTypeByCardType(int cardTypeId);

      List<SummaryDto> GetSummaryClaim(int userId,int userTypeId);

      List<DocumentTypeMasterDto> GetUploadDocumentMaster(int ltId);

      int AddUpdateUploadDocument(List<DocumentUploadsDto> documentUploadList, int idOfOther, int claimId);

      ClaimsDto GetClaimByClaimId(int clid);

      List<DocumentUploadsDto> GetDocumentUploadedbyClaimId(int clid);

      List<ClaimsDto> GetClaimSearch(ClaimSearchDto searchDto,int pageSize,int page);

      ClaimsDto GetClaimDetail(int id);

      ClaimsDto GetClaimByClaimIdForEdit(int clid);

      bool UpdateClaim(ClaimsDto claimsDto);

      void UpdateClaimNumber(string txtClaimNumber, int claimId,string comment,string fileNumber);

      List<ClaimsDto> GetClaimList(int pageSize, int pageNumber);

      string GenerateOtp(ClaimsDto claimsDto);

      string RegenrateOtp(int custId);

      void UpdateOtpToExpire(int p);

      List<PolicyMasterDto> GetPolicyMasterByCardTypeId(int cardTypeId);

      List<StatusMasterDto> GetStatus();

      List<ClaimsDto> UpdateStatusOfClaim(List<UpdateStatusRequestDto> updateStatusListRequesstList);

      List<ClaimsDto> GetClaimDetailSummary(List<int> claimIdsList);

      void ApproveReject(int claimId, int status,string approveRejectComment);

      MailTemplatedto GetMailTemplate(int templateId);

      void DeleteClaim(List<int> claimIdsList);

      List<SummaryDto> GetSummaryClaimByClaimIds(List<int> claimIdsList);

      List<DashboardDto> GetDashboard(int userId);

      int GetTotalClaimCount();

      int GetTotalClaimCountForSearch(ClaimSearchDto searchDto);



      List<ClaimsDto> GetClaimsForExport(List<int> claimIdsList, ClaimSearchDto claimSearchDto, bool isViewAll);
        List<LossTypeMasterDto> GetLossTypeParentList();
        List<LossTypeMasterDto> GetNatureOfLossTypeLossTypeId(int lossTypeId);
        List<PolicyMasterDto> GetPolicyByCardTypeIdAndLossTypeId(int cardTypeId, int lossTypeId,bool isSecured);
        bool UpdateClaimIntimation(int claimId, string claimNumber,string transactionNumber);
        bool UpdateClaimStatus(UpdateStatusRequestDto updateStatusRequestDto);
        List<ClaimsDto> GetClaimsByStatusId(List<int> statusId);
        bool UpdateDocumentCompletionStatus(List<int> claimIdList,int updateUserId);
        IntimationTransactionDto GetNewTransaction(string typeOfService);
        void UpdateTransaction(IntimationTransactionDto transaction);
        void UpdateNiaTransactionNumber(IntimationTransactionDto transaction);
    }
}
