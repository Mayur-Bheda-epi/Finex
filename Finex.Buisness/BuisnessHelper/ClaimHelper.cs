using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finex.Buisness.IBuisnessHelper;
using Finex.Data;
using Finex.Data.GenericRepository;
using Finex.Dto.Dtos;
using Finex.Data.UnitOfWork;

namespace Finex.Buisness.BuisnessHelper
{
    public class ClaimHelper : IClaimHelper
    {

        private readonly UnitOfWork _unitOfWork;


        /// <summary>
        /// Public constructor.
        /// </summary>
        public ClaimHelper()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<ClaimsDto> GetClaimListByUserId(int userId)
        {
            return
                _unitOfWork.ClaimRepository.GetMany(ch => ch.CreateUserId == userId && ch.IsActive == true).Select(cl => new ClaimsDto
                {
                    AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
                    CardNo = cl.CardNo,
                    FileNo = cl.FileNo,
                    ClaimAmount = cl.ClaimAmount != null ? cl.ClaimAmount.Value : 0,
                    CardTypeMasterDto = new CardTypeMasterDto
                    {
                        CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
                    },
                    ClaimNumber = cl.ClaimNumber,
                    DateIntimationBank = cl.DateIntimationBank != null ? cl.DateIntimationBank.Value : DateTime.MinValue,
                    CustId = cl.CustId != null ? cl.CustId.Value : 0,
                    ClaimId = cl.ClaimId,
                    DateOfLoss = cl.DateOfLoss != null ? cl.DateOfLoss.Value : DateTime.MinValue,
                    MerchantShop = cl.MerchantShop,
                    PassportNo = cl.PassportNo,
                    LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
                    LossTypeMasterDto = new LossTypeMasterDto
                    {
                        LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                    },
                    CustomersDto = new CustomersDto
                    {
                        CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                        CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                    },
                    StatusMasterDto = new StatusMasterDto
                    {
                        StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                    },
                    Comment = cl.Comment,

                    //DocumentTypeMasterDtos = _unitOfWork.DocumentTypeMasterRepository.GetMany(dtm => (!cl.DocumentUploads.Select(du => du.DocumentTypeId).ToList().Contains(dtm.DocumentTypeId)) && dtm.LossTypeId == claim.LossTypeId).Select(dum => new DocumentTypeMasterDto
                    //{
                    //    DocumentTypeName = dum.DocumentTypeName
                    //}).ToList()


                }).
                    ToList();
        }


        public List<CountryMasterDto> GetCountrys()
        {
            return _unitOfWork.CountryMasterRepository.GetAll().Select(cm => new CountryMasterDto
            {
                CountryId = cm.CountryId,
                CountryName = cm.CountryName

            }).ToList();
        }


        public List<StateMasterDto> GetStateByCountryId(int countryId)
        {
            return
                _unitOfWork.StateMasterRepository.GetMany(st => st.CountryId == countryId).Select(
                    st => new StateMasterDto
                    {
                        StateId = st.StateId,
                        StateName = st.StateName
                    }).ToList();
        }


        public List<CityMasterDto> GetCityByStateId(int stateId)
        {
            return _unitOfWork.CityMasterRepository.GetMany(ct => ct.StateId == stateId).Select(ct => new CityMasterDto
            {
                CityId = ct.CityId,
                CityName = ct.CityName
            }).ToList();
        }


        public bool AddClaim(ClaimsDto claimsDto)
        {
            try
            {
                var claim = new List<Claim>();
                var clm = new Claim
                {
                    CreateDate = claimsDto.CreateDate,
                    CountryId = claimsDto.CountryId,
                    CreateUserId = claimsDto.CreateUserId,
                    ParentLossTypeId = claimsDto.LossTypeIdParent,
                    LossTypeId = claimsDto.LossTypeId,
                    CityId = claimsDto.CityId,
                    UpdateDate = claimsDto.UpdateDate,
                    UpdateUserId = claimsDto.UpdateUserId,
                    CardTypeId = claimsDto.CardTypeId,
                    CardNo = claimsDto.CardNo,
                    MerchantShop = claimsDto.MerchantShop,
                    StatusId = claimsDto.StatusId,
                    SrNumber = claimsDto.SrNumber,
                    DateLossToBank = claimsDto.DateLossToBank,
                    Comment = claimsDto.Comment,
                    PolicyId = claimsDto.PolicyId > 0 ? claimsDto.PolicyId : (int?)null,

                    AccountNumber = claimsDto.AccountNumber,
                    //   DailyWithdrawalLimit = claimsDto.DailyWithdrawalLimit,
                    ClaimAmount = claimsDto.ClaimAmount,
                    TimeOfReportingBank = claimsDto.TimeOfReportingBank,
                    //DateSinceIntimation = claimsDto.DateSinceIntimation
                    DateSinceIntimation = DateTime.Now,
                    IsActive = true,
                    LabelCode = claimsDto.LabelCode
                };

                if (claimsDto.UserTypeId == 1)
                    clm.ClaimNumber = claimsDto.ClaimNumber;
                switch (claimsDto.LossTypeId)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 28:
                    case 35:
                        //  clm.AmountOfLoss = claimsDto.AmountOfLoss;
                        clm.DateOfLoss = claimsDto.DateOfLoss;
                        clm.PanNumber = claimsDto.PanNumber;
                        clm.CardBlockingDate = claimsDto.CardBlockingDate;
                        // if (claimsDto.LossTypeId == 1 || claimsDto.LossTypeId == 11 || claimsDto.LossTypeId == 13 || claimsDto.LossTypeId == 2 || claimsDto.LossTypeId == 12 || claimsDto.LossTypeId == 14)
                        if (claimsDto.LossTypeId == 11 || claimsDto.LossTypeId == 13 || claimsDto.LossTypeId == 12 || claimsDto.LossTypeId == 14)
                        {
                            //Sandeep
                            if (claimsDto.CardTypeId != 2)
                            {
                                clm.NoOfSecuredTrans = claimsDto.NoOfSecuredTrans;
                                clm.NoOfUnsecuredTrans = claimsDto.NoOfUnsecuredTrans;
                                clm.SecuredTransactionAmount = claimsDto.SecuredTransactionAmount;
                                clm.UnsecuredTranssactionAmount = claimsDto.UnsecuredTranssactionAmount;
                                clm.IsSecuredTransaction = claimsDto.IsSecuredTransaction;
                            }
                            //E Sandeep


                            //clm.NoOfSecuredTrans = claimsDto.NoOfSecuredTrans;
                            //clm.NoOfUnsecuredTrans = claimsDto.NoOfUnsecuredTrans;
                            //clm.SecuredTransactionAmount = claimsDto.SecuredTransactionAmount;
                            //clm.UnsecuredTranssactionAmount = claimsDto.UnsecuredTranssactionAmount;
                            //clm.IsSecuredTransaction = claimsDto.IsSecuredTransaction;
                            
                            clm.CustRelBankInYears = claimsDto.CustRelBankInYears;
                            clm.NoOfCardIssueAccount = claimsDto.NoOfCardIssueAccount;
                            clm.WhoBlockCard = claimsDto.WhoBlockCard;
                            clm.RelBlockCard = claimsDto.RelBlockCard;
                            clm.SmsDisputedTranaction = claimsDto.SmsDisputedTranaction;
                            clm.RealiseMisuseOfCard = claimsDto.RealiseMisuseOfCard;
                            clm.LastUsedCard = claimsDto.LastUsedCard;
                            clm.UseCardInAbroad = claimsDto.UseCardInAbroad;
                            clm.WhereWasCustAtDisputed = claimsDto.WhereWasCustAtDisputed;
                            clm.CardWithNow = claimsDto.CardWithNow;
                        }
                        break;

                    case 5:
                    case 16:
                    case 18:
                    case 20:
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                        //  clm.AmountOfLoss = claimsDto.AmountOfLoss;
                        clm.DateOfPOS = claimsDto.DateOfPOS;
                        clm.PanNumber = claimsDto.PanNumber;
                        clm.DateOfDeath = claimsDto.DateOfDeath;
                        break;
                    case 6:
                    case 15:
                        clm.DateOfPuchase = claimsDto.DateOfPuchase;
                        clm.DateIntimationBank = claimsDto.DateIntimationBank;
                        break;
                    case 7:
                    case 9:
                        clm.DateOfLoss = claimsDto.DateOfLoss;
                        clm.PanNumber = claimsDto.PanNumber;
                        break;
                    case 8:
                    case 34:
                        // clm.AmountOfLoss = claimsDto.AmountOfLoss;

                        clm.PanNumber = claimsDto.PanNumber;
                        break;
                    case 10:
                        clm.DateOfJoining = claimsDto.DateOfJoining;
                        clm.DateOfLeaving = claimsDto.DateOfLeaving;
                        clm.PanNumber = claimsDto.PanNumber;
                        break;


                }

                claim.Add(clm);
                var customer = new Customer
                {
                    CreateBy = claimsDto.CreateUserId,
                    CreateDate = claimsDto.CreateDate,
                    Email = claimsDto.CustomersDto.EmailId,
                    MobileNo = claimsDto.CustomersDto.MobileNumber,
                    Name = claimsDto.CustomersDto.CustomerName,
                    Address = claimsDto.CustomersDto.Address,
                    Claims = claim
                };
                _unitOfWork.CustomerRepository.Insert(customer);
                _unitOfWork.Save();
                claimsDto.ClaimId = _unitOfWork.ClaimRepository.GetFirst(cl => cl.CustId == customer.CustId).ClaimId;
                return true;
            }
            catch (Exception ex)
            {
                new Logging.Log.LogWriter().ErrorLogging("Error in save Claim: " + ex.Message);
                new Logging.Log.LogWriter().ErrorLogging("Error in save Claim: " + ex.InnerException.ToString());
                return false;
            }
        }


        public List<CardTypeMasterDto> GetCardTypeMaster()
        {
            return _unitOfWork.CardTypeMasterRepository.GetAll().Select(ctm => new CardTypeMasterDto
            {
                CardTypeId = ctm.CardTypeId,
                CardTypeName = ctm.CardTypeName
            }).ToList();
        }

        public List<LossTypeMasterDto> GetLossTypeMaster()
        {
            return _unitOfWork.LossTypeMasterRepository.GetAll().Select(ltm => new LossTypeMasterDto
            {
                LossType = ltm.LossType,
                LossTypeId = ltm.LossTypeId
            }).ToList();
        }


        public List<LossTypeMasterDto> GetLossTypeByCardType(int cardTypeId)
        {
            return
                _unitOfWork.LossTypeMasterRepository.GetMany(ltm => ltm.CardTypeId == cardTypeId).Select(
                    ltm => new LossTypeMasterDto
                    {
                        LossTypeId = ltm.LossTypeId,
                        LossType = ltm.LossType

                    }).ToList();
        }


        public List<SummaryDto> GetSummaryClaim(int userId, int userTypeId)
        {
            return new QueryRepository().GetSummaryClaim(userId, userTypeId);
        }


        public List<DocumentTypeMasterDto> GetUploadDocumentMaster(int ltId)
        {
            return _unitOfWork.DocumentTypeMasterRepository.GetMany(dtm => dtm.LossTypeId == ltId).Select(dtm => new DocumentTypeMasterDto
            {
                DocumentTypeId = dtm.DocumentTypeId,
                DocumentTypeName = dtm.DocumentTypeName

            }).ToList();
        }


        public int AddUpdateUploadDocument(List<DocumentUploadsDto> documentUploadList, int idOfOther, int claimId)
        {
            try
            {
                foreach (var document in documentUploadList)
                {
                    var documentObj = _unitOfWork.DocumentUploadRepository.GetFirst(dur => dur.ClaimId == document.ClaimId && dur.CustId == document.CustomerId && dur.DocumentTypeId == document.DocumentTypeId);
                    if (documentObj != null)
                    {
                        documentObj.DocumentPath = document.DocumentPath;
                        documentObj.DateOfUpload = DateTime.Now;
                        _unitOfWork.DocumentUploadRepository.Update(documentObj);
                    }
                    else
                    {
                        _unitOfWork.DocumentUploadRepository.Insert(new DocumentUpload
                        {
                            ClaimId = document.ClaimId,
                            CustId = document.CustomerId,
                            DocumentTypeId = document.DocumentTypeId,
                            DocumentPath = document.DocumentPath,
                            DateOfUpload = DateTime.Now
                        });
                    }
                }
                _unitOfWork.Save();
                var claim = _unitOfWork.ClaimRepository.GetByID(claimId);
                if (claim != null)
                {
                    List<int> docIdPa = new List<int> { 26, 35, 27, 28 };
                    var docIds = claim.LossTypeId == 5 ? claim.DocumentUploads.Where(du => du.DocumentTypeId != null && docIdPa.Contains(du.DocumentTypeId.Value)).Select(du => du.DocumentTypeId).ToList() : claim.DocumentUploads.Where(du => du.DocumentTypeId != idOfOther).Select(du => du.DocumentTypeId).ToList();
                    if (claim.LossTypeId != 5)
                    {
                        var pendingList =
                            _unitOfWork.DocumentTypeMasterRepository.GetMany(
                                dtm =>
                                (!docIds.Contains(dtm.DocumentTypeId)) && dtm.LossTypeId == claim.LossTypeId &&
                                dtm.DocumentTypeName != "Other").Select(dum => new DocumentTypeMasterDto
                                {
                                    DocumentTypeName =
                                                                                           dum.DocumentTypeName
                                }).ToList();
                        claim.StatusId = pendingList.Count > 0 ? 7 : 8;

                        _unitOfWork.ClaimRepository.Update(claim);
                        _unitOfWork.Save();
                        return pendingList.Count > 0 ? 7 : 8;
                    }
                    else
                    {
                        if (docIds.Count == 4)
                        {
                            claim.StatusId = 8;

                            _unitOfWork.ClaimRepository.Update(claim);
                            _unitOfWork.Save();
                            return 8;
                        }
                    }

                }

                return 0;


            }
            catch
            {
                return 0;
            }
        }


        public ClaimsDto GetClaimByClaimId(int clid)
        {
            var claim = _unitOfWork.ClaimRepository.GetByID(clid);
            return new ClaimsDto
            {
                LossTypeMasterDto = new LossTypeMasterDto
                {
                    LossType = claim.LossTypeId != null ? claim.LossTypeMaster.LossType : "",
                },
                CustomersDto = new CustomersDto
                {
                    CustomerName = claim.CustId != null ? claim.Customer.Name : ""
                }
            };
        }


        public List<DocumentUploadsDto> GetDocumentUploadedbyClaimId(int clid)
        {
            return _unitOfWork.DocumentUploadRepository.GetMany(dur => dur.ClaimId == clid).Select(du => new DocumentUploadsDto
            {
                DocumentPath = du.DocumentPath,
                DocumentTypeName = du.DocumentTypeId != null ? du.DocumentTypeMaster.DocumentTypeName : "",
                DocumentUploadDate = du.DateOfUpload

            }).ToList();
        }


        public List<ClaimsDto> GetClaimSearch(ClaimSearchDto searchDto, int pageSize, int page)
        {
            return new QueryRepository().GetClaimSearch(searchDto, pageSize, page);
        }


        public ClaimsDto GetClaimDetail(int id)
        {
            var claim = _unitOfWork.ClaimRepository.GetByID(id);
            var docIds = claim.DocumentUploads.Select(du => du.DocumentTypeId).ToList();

            return new ClaimsDto
            {

                ClaimId = claim.ClaimId,
                CustId = claim.CustId != null ? claim.CustId.Value : 0,
                CardNo = claim.CardNo,
                CreateDate = claim.CreateDate.Value,
                LossTypeIdParent = claim.ParentLossTypeId != null ? claim.ParentLossTypeId.Value : 0,
                LossTypeId = claim.LossTypeId != null ? claim.LossTypeId.Value : 0,
                LabelCode = claim.LabelCode,
                CustomersDto = new CustomersDto
                {
                    CustomerName = claim.Customer.Name,
                    EmailId = claim.Customer.Email,
                    MobileNumber = claim.Customer.MobileNo,
                    Address = claim.Customer.Address

                },
                CardTypeMasterDto = new CardTypeMasterDto
                {
                    CardTypeName = claim.CardTypeMaster.CardTypeName
                },

                LossTypeMasterParentDto = new LossTypeMasterDto
                {
                    LossType = claim.LossTypeMaster1.LossType,
                    Code = claim.LossTypeMaster1.Code
                },
                LossTypeMasterDto = new LossTypeMasterDto
                {
                    LossType = claim.LossTypeMaster.LossType,
                    Code = claim.LossTypeMaster.Code
                },
                StatusId = claim.StatusId != null ? claim.StatusId.Value : 0,
                StatusMasterDto = new StatusMasterDto
                {
                    StatusName = claim.StatusId != null ? claim.StatusMaster.StatusName : ""
                },
                SrNumber = claim.SrNumber,
                MerchantShop = claim.MerchantShop,
                Comment = claim.Comment,
                DateLossToBank = claim.DateLossToBank != null ? claim.DateLossToBank.Value : DateTime.MinValue,

                AmountOfLoss = claim.AmountOfLoss != null ? claim.AmountOfLoss.Value : 0,
                // CardBlockingDate = claim.CardBlockingDate != null ? claim.CardBlockingDate.Value : DateTime.MinValue,
                CardBlockingDate = claim.CardBlockingDate,
                PanNumber = claim.PanNumber,
                // DateOfPuchase = claim.DateOfPuchase != null ? claim.DateOfPuchase.Value : DateTime.MinValue,
                DateOfPuchase = claim.DateOfPuchase,
                //DateOfPOS = claim.DateOfPOS != null ? claim.DateOfPOS.Value : DateTime.MinValue,
                DateOfPOS = claim.DateOfPOS,
                // DateOfLoss = claim.DateOfLoss != null ? claim.DateOfLoss.Value : DateTime.MinValue,
                DateOfLoss = claim.DateOfLoss,
                //DateOfLeaving = claim.DateOfLeaving != null ? claim.DateOfLeaving.Value : DateTime.MinValue,
                DateOfLeaving = claim.DateOfLeaving,
                //DateOfJoining = claim.DateOfJoining != null ? claim.DateOfJoining.Value : DateTime.MinValue,
                DateOfJoining = claim.DateOfJoining,
                //DateIntimationBank = claim.DateIntimationBank != null ? claim.DateIntimationBank.Value : DateTime.MinValue,
                DateIntimationBank = claim.DateIntimationBank,
                DateSinceIntimation = claim.DateSinceIntimation,
                ClaimNumber = claim.ClaimNumber,
                CountryName = claim.CountryId != null ? claim.CountryMaster.CountryName : "",
                StateName = claim.CityId != null ? claim.CityMaste.StateMaster.StateName : "",
                CityName = claim.CityId != null ? claim.CityMaste.CityName : "",
                NoOfSecuredTrans = claim.NoOfSecuredTrans != null ? claim.NoOfSecuredTrans.Value : 0,
                NoOfUnsecuredTrans = claim.NoOfUnsecuredTrans != null ? claim.NoOfUnsecuredTrans.Value : 0,
                SecuredTransactionAmount = claim.SecuredTransactionAmount != null ? claim.SecuredTransactionAmount.Value : 0,
                UnsecuredTranssactionAmount = claim.UnsecuredTranssactionAmount != null ? claim.UnsecuredTranssactionAmount.Value : 0,
                CustRelBankInYears = claim.CustRelBankInYears != null ? claim.CustRelBankInYears.Value : 0,
                NoOfCardIssueAccount = claim.NoOfCardIssueAccount != null ? claim.NoOfCardIssueAccount.Value : 0,
                WhoBlockCard = claim.WhoBlockCard,
                RelBlockCard = claim.RelBlockCard,
                SmsDisputedTranaction = claim.SmsDisputedTranaction,
                RealiseMisuseOfCard = claim.RealiseMisuseOfCard,
                LastUsedCard = claim.LastUsedCard,
                UseCardInAbroad = claim.UseCardInAbroad,
                WhereWasCustAtDisputed = claim.WhereWasCustAtDisputed,
                DailyWithdrawalLimit = claim.DailyWithdrawalLimit != null ? claim.DailyWithdrawalLimit.Value : 0,
                PolicyId = claim.PolicyId != null ? claim.PolicyId.Value : 0,
                AccountNumber = claim.AccountNumber,
                TimeOfReportingBank = claim.TimeOfReportingBank,
                DateOfDeath = claim.DateOfDeath,

                ClaimAmount = claim.ClaimAmount != null ? claim.ClaimAmount.Value : 0,
                PolicyMasterDto = new PolicyMasterDto
                {
                    PolicyId = claim.PolicyId != null ? claim.PolicyId.Value : 0,
                    PolicyNumber = claim.PolicyId != null ? claim.PolicyMaster.PolicyNumber : ""
                },
                CardWithNow = claim.CardWithNow,
                TransactionSecureOrNot = claim.IsSecuredTransaction != null && claim.IsSecuredTransaction.Value == true ? "Secured" : "Un Secured",
                NiaTransactionNumber = claim.NiaTransactionNumber,
                DocumentUploadsDtos = claim.DocumentUploads.Select(du => new DocumentUploadsDto
                {
                    DocumentTypeName = du.DocumentTypeMaster.DocumentTypeName,
                    DocumentPath = du.DocumentPath,
                    DocumentUploadDate = du.DateOfUpload
                }).ToList(),
                DocumentTypeMasterDtos = _unitOfWork.DocumentTypeMasterRepository.GetMany(dtm => (!docIds.Contains(dtm.DocumentTypeId)) && dtm.LossTypeId == claim.LossTypeId).Select(dum => new DocumentTypeMasterDto
                {
                    DocumentTypeName = dum.DocumentTypeName
                }).ToList(),
                ClaimReverseFeedDtoList = claim.ClaimReverseFeeds != null && claim.ClaimReverseFeeds.Count > 0 ? claim.ClaimReverseFeeds.Select(crf => new ClaimReverseFeedDto
                {
                    ACCIDENTDATE = crf.AccidentDate != null ? crf.AccidentDate.Value : DateTime.MinValue,
                    APPROVEDAMOUNT = crf.ApprovedAmount != null ? crf.ApprovedAmount.Value : 0,
                    ASSESSEDAMOUNT = crf.AssessedAmount != null ? crf.AssessedAmount.Value : 0,
                    CHEQUEORPAYMENTNO = crf.ChequePayementNumber,
                    CLAIMESTIMATEDAMOUNT = crf.ClaimEstimatedAmount != null ? crf.ClaimEstimatedAmount.Value : 0,
                    CLAIMINTIMATIONDATE = crf.ClaimIntimationDate != null ? crf.ClaimIntimationDate.Value : DateTime.MinValue,
                    COMPANY = crf.Company,
                    CUSTOMERNAME = crf.CustomerName,
                    FEEDID = crf.FeedId != null ? Convert.ToInt32(crf.FeedId) : 0,
                    FILESUBDATE = crf.FileSubDate != null ? crf.FileSubDate.Value : DateTime.Now,
                    INVOICENUMBER = crf.InvoiceNumber,
                    PAIDAMOUNT = crf.PaidAmount != null ? crf.PaidAmount.Value : 0,
                    PAYEEPARTYCODE = crf.PayeePartyCode,
                    PAYEEPARTYNAME = crf.PayeePartyName,
                    PAYEETYPE = crf.PayeeType,
                    PAYMENTDATE = crf.PaymentDate != null ? crf.PaymentDate.Value : DateTime.MinValue,
                    PAYMENTMODE = crf.PaymentMode,
                    RECORDTYPE = crf.RecordType
                }).ToList() : new List<ClaimReverseFeedDto>()

            };
        }


        public ClaimsDto GetClaimByClaimIdForEdit(int clid)
        {
            var claim = _unitOfWork.ClaimRepository.GetByID(clid);
            return new ClaimsDto
            {
                ClaimId = claim.ClaimId,
                CardNo = claim.CardNo,
                LossTypeIdParent = claim.ParentLossTypeId != null ? claim.ParentLossTypeId.Value : 0,
                LossTypeId = claim.LossTypeId != null ? claim.LossTypeId.Value : 0,
                CustomersDto = new CustomersDto
                {
                    CustomerName = claim.Customer.Name,
                    EmailId = claim.Customer.Email,
                    MobileNumber = claim.Customer.MobileNo,
                    Address = claim.Customer.Address
                },
                CustId = claim.CustId != null ? claim.CustId.Value : 0,
                CardTypeId = claim.CardTypeId != null ? claim.CardTypeId.Value : 0,
                Comment = claim.Comment,
                SrNumber = claim.SrNumber,
                MerchantShop = claim.MerchantShop,
                DateLossToBank = claim.DateLossToBank != null ? claim.DateLossToBank.Value : DateTime.MinValue,
                // AmountOfLoss = claim.AmountOfLoss != null ? claim.AmountOfLoss.Value : 0,
                AmountOfLoss = claim.AmountOfLoss,
                // CardBlockingDate = claim.CardBlockingDate != null ? claim.CardBlockingDate.Value : DateTime.MinValue,
                CardBlockingDate = claim.CardBlockingDate,
                PanNumber = claim.PanNumber,
                // DateOfPuchase = claim.DateOfPuchase != null ? claim.DateOfPuchase.Value : DateTime.MinValue,
                DateOfPuchase = claim.DateOfPuchase,
                // DateOfPOS = claim.DateOfPOS != null ? claim.DateOfPOS.Value : DateTime.MinValue,
                DateOfPOS = claim.DateOfPOS,
                // DateOfLoss = claim.DateOfLoss != null ? claim.DateOfLoss.Value : DateTime.MinValue,
                DateOfLoss = claim.DateOfLoss,
                // DateOfLeaving = claim.DateOfLeaving != null ? claim.DateOfLeaving.Value : DateTime.MinValue,
                DateOfLeaving = claim.DateOfLeaving,
                // DateOfJoining = claim.DateOfJoining != null ? claim.DateOfJoining.Value : DateTime.MinValue,
                DateOfJoining = claim.DateOfJoining,
                // DateIntimationBank = claim.DateIntimationBank != null ? claim.DateIntimationBank.Value : DateTime.MinValue,
                DateIntimationBank = claim.DateIntimationBank,
                DateSinceIntimation = claim.DateSinceIntimation,
                ClaimNumber = claim.ClaimNumber,
                CountryId = claim.CountryId != null ? claim.CountryId.Value : 0,
                StateId = claim.CityId != null ? claim.CityMaste.StateMaster.StateId : 0,
                CityId = claim.CityId != null ? claim.CityId.Value : 0,
                NoOfSecuredTrans = claim.NoOfSecuredTrans != null ? claim.NoOfSecuredTrans.Value : 0,
                NoOfUnsecuredTrans = claim.NoOfUnsecuredTrans != null ? claim.NoOfUnsecuredTrans.Value : 0,
                SecuredTransactionAmount = claim.SecuredTransactionAmount != null ? claim.SecuredTransactionAmount.Value : 0,
                UnsecuredTranssactionAmount = claim.UnsecuredTranssactionAmount != null ? claim.UnsecuredTranssactionAmount.Value : 0,
                CustRelBankInYears = claim.CustRelBankInYears != null ? claim.CustRelBankInYears.Value : 0,
                NoOfCardIssueAccount = claim.NoOfCardIssueAccount != null ? claim.NoOfCardIssueAccount.Value : 0,
                WhoBlockCard = claim.WhoBlockCard,
                RelBlockCard = claim.RelBlockCard,
                SmsDisputedTranaction = claim.SmsDisputedTranaction,
                RealiseMisuseOfCard = claim.RealiseMisuseOfCard,
                LastUsedCard = claim.LastUsedCard,
                UseCardInAbroad = claim.UseCardInAbroad,
                WhereWasCustAtDisputed = claim.WhereWasCustAtDisputed,
                CardWithNow = claim.CardWithNow,
                AccountNumber = claim.AccountNumber,
                PolicyId = claim.PolicyId != null ? claim.PolicyId.Value : 0,
                DailyWithdrawalLimit = claim.DailyWithdrawalLimit != null ? claim.DailyWithdrawalLimit.Value : 0,
                // ClaimAmount = claim.ClaimAmount != null ? claim.ClaimAmount.Value : 0,
                ClaimAmount = claim.ClaimAmount,
                TimeOfReportingBank = claim.TimeOfReportingBank,
                DateOfDeath = claim.DateOfDeath,
                LabelCode = claim.LabelCode,
                IsSecuredTransaction = claim.IsSecuredTransaction != null ? Convert.ToBoolean(claim.IsSecuredTransaction.Value) : false

            };
        }


        public bool UpdateClaim(ClaimsDto claimsDto)
        {
            try
            {
                var claim = _unitOfWork.ClaimRepository.GetByID(claimsDto.ClaimId);
                if (claim != null)
                {
                    claim.CreateDate = claimsDto.CreateDate;
                    claim.CountryId = claimsDto.CountryId;
                    claim.CreateUserId = claimsDto.CreateUserId;
                    claim.ParentLossTypeId = claimsDto.LossTypeIdParent;
                    claim.LossTypeId = claimsDto.LossTypeId;
                    claim.CityId = claimsDto.CityId;
                    claim.UpdateDate = claimsDto.UpdateDate;
                    claim.UpdateUserId = claimsDto.UpdateUserId;
                    claim.CardTypeId = claimsDto.CardTypeId;
                    claim.CardNo = claimsDto.CardNo;
                    claim.MerchantShop = claimsDto.MerchantShop;
                    // claim.StatusId = claimsDto.StatusId;
                    claim.SrNumber = claimsDto.SrNumber;
                    claim.DateLossToBank = claimsDto.DateLossToBank;
                    claim.Comment = claimsDto.Comment;
                    claim.PolicyId = claimsDto.PolicyId;
                    claim.AccountNumber = claimsDto.AccountNumber;
                    // claim.DailyWithdrawalLimit = claimsDto.DailyWithdrawalLimit;
                    claim.ClaimAmount = claimsDto.ClaimAmount;
                    claim.TimeOfReportingBank = claimsDto.TimeOfReportingBank;
                    claim.LabelCode = claimsDto.LabelCode;
                    // claim.DateSinceIntimation = claimsDto.DateSinceIntimation;
                    if (claimsDto.UserTypeId == 1)
                        claim.ClaimNumber = claimsDto.ClaimNumber;

                    switch (claimsDto.LossTypeId)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 28:
                        case 35:
                            claim.AmountOfLoss = claimsDto.AmountOfLoss;
                            claim.DateOfLoss = claimsDto.DateOfLoss;
                            claim.PanNumber = claimsDto.PanNumber;
                            claim.CardBlockingDate = claimsDto.CardBlockingDate;
                            //  if (claimsDto.LossTypeId == 1 || claimsDto.LossTypeId == 11 || claimsDto.LossTypeId == 13 || claimsDto.LossTypeId == 2 || claimsDto.LossTypeId == 12 || claimsDto.LossTypeId == 14)
                            if (claimsDto.LossTypeId == 11 || claimsDto.LossTypeId == 13 || claimsDto.LossTypeId == 12 || claimsDto.LossTypeId == 14)
                            {

                                //claim.NoOfSecuredTrans = claimsDto.NoOfSecuredTrans;
                                //claim.NoOfUnsecuredTrans = claimsDto.NoOfUnsecuredTrans;
                                //claim.SecuredTransactionAmount = claimsDto.SecuredTransactionAmount;
                                //claim.IsSecuredTransaction = claimsDto.IsSecuredTransaction;
                                //claim.UnsecuredTranssactionAmount = claimsDto.UnsecuredTranssactionAmount;
                                //Sandeep
                                if (claimsDto.CardTypeId != 2)
                                {
                                    claim.NoOfSecuredTrans = claimsDto.NoOfSecuredTrans;
                                    claim.NoOfUnsecuredTrans = claimsDto.NoOfUnsecuredTrans;
                                    claim.SecuredTransactionAmount = claimsDto.SecuredTransactionAmount;
                                    claim.IsSecuredTransaction = claimsDto.IsSecuredTransaction;
                                    claim.UnsecuredTranssactionAmount = claimsDto.UnsecuredTranssactionAmount;
                                }
                                //E Sandeep
                                claim.CustRelBankInYears = claimsDto.CustRelBankInYears;
                                claim.NoOfCardIssueAccount = claimsDto.NoOfCardIssueAccount;
                                claim.WhoBlockCard = claimsDto.WhoBlockCard;
                                claim.RelBlockCard = claimsDto.RelBlockCard;
                                claim.SmsDisputedTranaction = claimsDto.SmsDisputedTranaction;
                                claim.RealiseMisuseOfCard = claimsDto.RealiseMisuseOfCard;
                                claim.LastUsedCard = claimsDto.LastUsedCard;
                                claim.UseCardInAbroad = claimsDto.UseCardInAbroad;
                                claim.WhereWasCustAtDisputed = claimsDto.WhereWasCustAtDisputed;
                                claim.CardWithNow = claimsDto.CardWithNow;
                            }
                            break;

                        case 5:
                        case 16:
                        case 18:
                        case 20:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                            //claim.AmountOfLoss = claimsDto.AmountOfLoss;
                            claim.DateOfPOS = claimsDto.DateOfPOS;
                            claim.PanNumber = claimsDto.PanNumber;
                            claim.DateOfDeath = claimsDto.DateOfDeath;
                            break;
                        case 6:
                        case 15:
                            claim.DateOfPuchase = claimsDto.DateOfPuchase;
                            claim.DateIntimationBank = claimsDto.DateIntimationBank;
                            break;
                        case 7:
                        case 9:
                            claim.DateOfLoss = claimsDto.DateOfLoss;
                            claim.PanNumber = claimsDto.PanNumber;
                            break;
                        case 8:
                        case 34:
                            //  claim.AmountOfLoss = claimsDto.AmountOfLoss;

                            claim.PanNumber = claimsDto.PanNumber;
                            break;
                        case 10:
                            claim.DateOfJoining = claimsDto.DateOfJoining;
                            claim.DateOfLeaving = claimsDto.DateOfLeaving;
                            claim.PanNumber = claimsDto.PanNumber;
                            break;
                    }

                    _unitOfWork.ClaimRepository.Update(claim);
                    var customer = _unitOfWork.CustomerRepository.GetByID(claim.CustId);
                    if (customer != null)
                    {
                        customer.MobileNo = claimsDto.CustomersDto.MobileNumber;
                        customer.Name = claimsDto.CustomersDto.CustomerName;
                        customer.Email = claimsDto.CustomersDto.EmailId;
                        customer.Address = claimsDto.CustomersDto.Address;
                    }
                    _unitOfWork.CustomerRepository.Update(customer);
                    _unitOfWork.Save();

                }





                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public void UpdateClaimNumber(string txtClaimNumber, int claimId, string comment, string fileNumber)
        {
            var claim = _unitOfWork.ClaimRepository.GetByID(claimId);
            if (claim != null)
            {
                claim.ClaimNumber = txtClaimNumber;
                claim.Comment = comment;
                claim.FileNo = fileNumber;
                _unitOfWork.ClaimRepository.Update(claim);
                _unitOfWork.Save();
            }
        }


        public List<ClaimsDto> GetClaimList(int pageSize, int pageNumber)
        {
            return
                _unitOfWork.ClaimRepository.GetPaged(cl => cl.IsActive == true, pageSize, pageNumber).Select(cl => new ClaimsDto
                {
                    AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
                    CardNo = cl.CardNo,
                    FileNo = cl.FileNo,
                    ClaimAmount = cl.ClaimAmount != null ? cl.ClaimAmount.Value : 0,
                    CardTypeMasterDto = new CardTypeMasterDto
                    {
                        CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
                    },
                    ClaimNumber = cl.ClaimNumber,
                    DateIntimationBank = cl.DateIntimationBank != null ? cl.DateIntimationBank.Value : DateTime.MinValue,
                    CustId = cl.CustId != null ? cl.CustId.Value : 0,
                    ClaimId = cl.ClaimId,
                    DateOfLoss = cl.DateOfLoss != null ? cl.DateOfLoss.Value : DateTime.MinValue,
                    MerchantShop = cl.MerchantShop,
                    PassportNo = cl.PassportNo,
                    LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
                    LossTypeMasterDto = new LossTypeMasterDto
                    {
                        LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                    },
                    CustomersDto = new CustomersDto
                    {
                        CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                        CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                    },
                    StatusMasterDto = new StatusMasterDto
                    {
                        StatusId = cl.StatusId != null ? cl.StatusId.Value : 0,
                        StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                    },
                    UpdateDate = cl.UpdateDate != null ? cl.UpdateDate.Value : DateTime.MinValue


                }).
                    ToList();
        }


        public string GenerateOtp(ClaimsDto claimsDto)
        {
            var otpNumber = Convert.ToString(GenerateRandomNo());
            var otp = new OTP
            {
                CustId = claimsDto.CustId,
                ExpireTime = DateTime.Now.AddHours(48),
                IsUsed = false,
                OTP1 = otpNumber

            };
            _unitOfWork.OTPRepository.Insert(otp);
            _unitOfWork.Save();
            return otpNumber;
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }


        public string RegenrateOtp(int custId)
        {
            var otpNumber = Convert.ToString(GenerateRandomNo());
            var otpEarlier = _unitOfWork.OTPRepository.GetFirst(otr => otr.CustId == custId && otr.IsUsed == false);
            if (otpEarlier != null)
            {
                otpEarlier.IsUsed = true;
                otpEarlier.ExpireTime = DateTime.Now;
                _unitOfWork.OTPRepository.Update(otpEarlier);
            }
            var otp = new OTP
            {
                CustId = custId,
                ExpireTime = DateTime.Now.AddHours(48),
                IsUsed = false,
                OTP1 = otpNumber

            };

            _unitOfWork.OTPRepository.Insert(otp);
            _unitOfWork.Save();
            return otpNumber;
        }


        public void UpdateOtpToExpire(int custId)
        {
            var otpEarlier = _unitOfWork.OTPRepository.GetFirst(otr => otr.CustId == custId && otr.IsUsed == false);
            if (otpEarlier != null)
            {
                otpEarlier.IsUsed = true;
                otpEarlier.ExpireTime = DateTime.Now;
                _unitOfWork.OTPRepository.Update(otpEarlier);
                _unitOfWork.Save();
            }
        }


        public List<PolicyMasterDto> GetPolicyMasterByCardTypeId(int cardTypeId)
        {
            return
                _unitOfWork.PolicyMasterRepository.GetMany(pm => pm.CardTypeId == cardTypeId).Select(
                    pmt => new PolicyMasterDto
                    {
                        PolicyId = pmt.PolicyId,
                        PolicyNumber = pmt.PolicyNumber
                    }).ToList();
        }


        public List<StatusMasterDto> GetStatus()
        {
            return _unitOfWork.StatusMasterRepository.GetAll().Select(st => new StatusMasterDto
            {
                StatusId = st.StatusId,
                StatusName = st.StatusName

            }).ToList();
        }


        public List<ClaimsDto> UpdateStatusOfClaim(List<UpdateStatusRequestDto> updateStatusListRequesstList)
        {
            var claimList = new List<ClaimsDto>();
            foreach (var updateStatusRequestDto in updateStatusListRequesstList)
            {

                var claim = _unitOfWork.ClaimRepository.GetByID(updateStatusRequestDto.ClaimId);
                if (claim != null)
                {
                    claim.StatusId = updateStatusRequestDto.StatusId;
                    claim.UpdateDate = updateStatusRequestDto.UpdateDate;
                    claim.UpdateUserId = updateStatusRequestDto.UpdateUserId;
                    claim.ApproveRejectComment = updateStatusRequestDto.Comment;
                    claimList.Add(new ClaimsDto
                    {
                        SrNumber = claim.SrNumber,
                        CustomersDto = new CustomersDto
                        {
                            CustomerName = claim.CustId != null ? claim.Customer.Name : "",
                            EmailId = claim.CustId != null ? claim.Customer.Email : ""
                        }

                    });
                }
                _unitOfWork.ClaimRepository.Update(claim);
            }
            _unitOfWork.Save();
            return claimList;
        }


        public List<ClaimsDto> GetClaimDetailSummary(List<int> claimIdsList)
        {
            return
               _unitOfWork.ClaimRepository.GetMany(ch => claimIdsList.Contains(ch.ClaimId)).Select(cl => new ClaimsDto
               {

                   CardTypeMasterDto = new CardTypeMasterDto
                   {
                       CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
                   },

                   LossTypeMasterDto = new LossTypeMasterDto
                   {
                       LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                   },
                   CustomersDto = new CustomersDto
                   {
                       CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                       CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                   },

                   DocumentUploadsDtos = cl.DocumentUploads.Select(du => new DocumentUploadsDto
                   {
                       DocumentTypeName = du.DocumentTypeMaster.DocumentTypeName,
                       DocumentPath = du.DocumentPath
                   }).ToList(),
                   DocumentTypeMasterDtos = _unitOfWork.DocumentTypeMasterRepository.GetMany(dtm => (!cl.DocumentUploads.Select(du => du.DocumentTypeId).ToList().Contains(dtm.DocumentTypeId)) && dtm.LossTypeId == cl.LossTypeId).Select(dum => new DocumentTypeMasterDto
                   {
                       DocumentTypeName = dum.DocumentTypeName
                   }).ToList()


               }).
                   ToList();
        }


        public void ApproveReject(int claimId, int status, string approveRejectComment)
        {
            var claim = _unitOfWork.ClaimRepository.GetFirst(cl => cl.ClaimId == claimId);
            if (claim != null)
            {
                claim.StatusId = status;
                claim.ApproveRejectComment = approveRejectComment;
                _unitOfWork.ClaimRepository.Update(claim);
                _unitOfWork.Save();
            }
        }


        public MailTemplatedto GetMailTemplate(int templateId)
        {
            var template = _unitOfWork.MailTemplateRepository.GetByID(templateId);
            return new MailTemplatedto
            {
                TemplateId = template.TemplateId,
                Subject = template.Subject,
                TemplateBody = template.TemplateBody,
                TemplateName = template.TemplateName
            };
        }


        public void DeleteClaim(List<int> claimIdsList)
        {
            foreach (var clamId in claimIdsList)
            {
                var claim = _unitOfWork.ClaimRepository.GetByID(clamId);
                if (claim != null)
                {
                    claim.IsActive = false;
                    _unitOfWork.ClaimRepository.Update(claim);
                }
            }
            _unitOfWork.Save();
        }


        public List<SummaryDto> GetSummaryClaimByClaimIds(List<int> claimIdsList)
        {
            return new QueryRepository().GetSummaryClaimByClaimIds(claimIdsList);
        }


        public List<DashboardDto> GetDashboard(int userId)
        {
            return new QueryRepository().GetDashBoard(userId);
        }


        public int GetTotalClaimCount()
        {
            return _unitOfWork.ClaimRepository.GetCount(cl => cl.IsActive == true);
        }


        public int GetTotalClaimCountForSearch(ClaimSearchDto searchDto)
        {
            return new QueryRepository().GetTotalClaimCountForSearch(searchDto);
        }


        public List<ClaimsDto> GetClaimsForExport(List<int> claimIdsList, ClaimSearchDto claimSearchDto, bool isViewAll)
        {
            if (claimIdsList.Count > 0)
            {
                return
               _unitOfWork.ClaimRepository.GetMany(cl => claimIdsList.Contains(cl.ClaimId)).Select(cl => new ClaimsDto
               {
                   AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
                   CardNo = cl.CardNo,
                   SrNumber = cl.SrNumber,
                   AccountNumber = cl.AccountNumber,
                   FileNo = cl.FileNo,
                   DateSinceIntimation = cl.DateSinceIntimation,
                   NoOfSecuredTrans = cl.NoOfSecuredTrans,
                   NoOfUnsecuredTrans = cl.NoOfUnsecuredTrans,
                   LabelCode = cl.LabelCode,
                   ClaimAmount = cl.ClaimAmount != null ? cl.ClaimAmount.Value : 0,
                   CardTypeMasterDto = new CardTypeMasterDto
                   {
                       CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
                   },
                   ClaimNumber = cl.ClaimNumber,
                   DateIntimationBank = cl.DateIntimationBank != null ? cl.DateIntimationBank.Value : DateTime.MinValue,
                   CustId = cl.CustId != null ? cl.CustId.Value : 0,
                   ClaimId = cl.ClaimId,
                   DateOfLoss = cl.DateOfLoss != null ? cl.DateOfLoss.Value : DateTime.MinValue,
                   MerchantShop = cl.MerchantShop,
                   PassportNo = cl.PassportNo,
                   LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
                   LossTypeMasterDto = new LossTypeMasterDto
                   {
                       LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                   },
                   CustomersDto = new CustomersDto
                   {
                       CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                       CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                   },
                   StatusMasterDto = new StatusMasterDto
                   {
                       StatusId = cl.StatusId != null ? cl.StatusId.Value : 0,
                       StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                   }


               }).
                   ToList();
            }
            if (isViewAll)
            {
                return
               _unitOfWork.ClaimRepository.GetMany(cl => cl.IsActive == true).Select(cl => new ClaimsDto
               {
                   AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
                   CardNo = cl.CardNo,
                   AccountNumber = cl.AccountNumber,
                   FileNo = cl.FileNo,
                   DateSinceIntimation = cl.DateSinceIntimation,
                   NoOfSecuredTrans = cl.NoOfSecuredTrans,
                   NoOfUnsecuredTrans = cl.NoOfUnsecuredTrans,
                   ClaimAmount = cl.ClaimAmount != null ? cl.ClaimAmount.Value : 0,
                   LabelCode = cl.LabelCode,
                   CardTypeMasterDto = new CardTypeMasterDto
                   {
                       CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
                   },
                   ClaimNumber = cl.ClaimNumber,
                   DateIntimationBank = cl.DateIntimationBank != null ? cl.DateIntimationBank.Value : DateTime.MinValue,
                   CustId = cl.CustId != null ? cl.CustId.Value : 0,
                   ClaimId = cl.ClaimId,
                   DateOfLoss = cl.DateOfLoss != null ? cl.DateOfLoss.Value : DateTime.MinValue,
                   MerchantShop = cl.MerchantShop,
                   PassportNo = cl.PassportNo,
                   LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
                   LossTypeMasterDto = new LossTypeMasterDto
                   {
                       LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                   },
                   CustomersDto = new CustomersDto
                   {
                       CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                       CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                   },
                   StatusMasterDto = new StatusMasterDto
                   {
                       StatusId = cl.StatusId != null ? cl.StatusId.Value : 0,
                       StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                   }


               }).
                   ToList();
            }
            if (isViewAll == false && (!string.IsNullOrEmpty(claimSearchDto.Name) || !string.IsNullOrEmpty(claimSearchDto.MobileNo) || !string.IsNullOrEmpty(claimSearchDto.EmailId) || !string.IsNullOrEmpty(claimSearchDto.AccountNo) || !string.IsNullOrEmpty(claimSearchDto.CardNo)))
            {
                return new QueryRepository().GetClaimSearch(claimSearchDto, 0, 0, false);
            }
            return new List<ClaimsDto>();
        }

        public List<LossTypeMasterDto> GetLossTypeParentList()
        {
            return _unitOfWork.LossTypeMasterRepository.GetMany(ltmp => ltmp.IsNatureOfLoss == false).Select(ltmp => new LossTypeMasterDto
            {
                LossType = ltmp.LossType,
                LossTypeId = ltmp.LossTypeId

            }).ToList();
        }

        public List<LossTypeMasterDto> GetNatureOfLossTypeLossTypeId(int lossTypeId)
        {
            return _unitOfWork.LossTypeMasterRepository.GetMany(nol => nol.ParentLossTypeId == lossTypeId && nol.IsActive == true).Select(ltmp => new LossTypeMasterDto
            {
                LossType = ltmp.LossType,
                LossTypeId = ltmp.LossTypeId

            }).ToList();
        }

        public List<PolicyMasterDto> GetPolicyByCardTypeIdAndLossTypeId(int cardTypeId, int lossTypeId, bool isSecured)
        {
            var policyObj = _unitOfWork.CardLossPolicyMappingRepository.GetFirst(clpm => clpm.CardTypeId == cardTypeId && clpm.LossTypeId == lossTypeId && clpm.IsSecured == isSecured);
            return new List<PolicyMasterDto> {
                new PolicyMasterDto {
                    PolicyId = policyObj.PolicyNumberId.Value,
                    PolicyNumber=policyObj.PolicyMaster.PolicyNumber
                                    }

            };
        }

        public bool UpdateClaimIntimation(int claimId, string claimNumber, string transactionNumber)
        {
            var claim = _unitOfWork.ClaimRepository.GetByID(claimId);
            if (claim != null)
            {
                claim.ClaimNumber = claimNumber;
                claim.StatusId = 6;
                claim.NiaTransactionNumber = transactionNumber;
                claim.UpdateDate = DateTime.Now;
                _unitOfWork.ClaimRepository.Update(claim);
                _unitOfWork.Save();
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool UpdateClaimStatus(UpdateStatusRequestDto updateStatusRequestDto)
        {
            try
            {
                var claim = _unitOfWork.ClaimRepository.GetByID(updateStatusRequestDto.ClaimId);
                if (claim != null)
                {
                    claim.StatusId = updateStatusRequestDto.StatusId;
                    claim.UpdateUserId = updateStatusRequestDto.UpdateUserId;
                    claim.UpdateDate = updateStatusRequestDto.UpdateDate;
                    _unitOfWork.ClaimRepository.Update(claim);
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<ClaimsDto> GetClaimsByStatusId(List<int> statusId)
        {
            return
                _unitOfWork.ClaimRepository.GetMany(cl => cl.IsActive == true && statusId.Contains(cl.StatusId.Value)).Select(cl => new ClaimsDto
                {
                    AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
                    CardNo = cl.CardNo,
                    FileNo = cl.FileNo,
                    ClaimAmount = cl.ClaimAmount != null ? cl.ClaimAmount.Value : 0,
                    CardTypeMasterDto = new CardTypeMasterDto
                    {
                        CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
                    },
                    ClaimNumber = cl.ClaimNumber,
                    DateIntimationBank = cl.DateIntimationBank != null ? cl.DateIntimationBank.Value : DateTime.MinValue,
                    CustId = cl.CustId != null ? cl.CustId.Value : 0,
                    ClaimId = cl.ClaimId,
                    DateOfLoss = cl.DateOfLoss != null ? cl.DateOfLoss.Value : DateTime.MinValue,
                    MerchantShop = cl.MerchantShop,
                    PassportNo = cl.PassportNo,
                    LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
                    LossTypeMasterDto = new LossTypeMasterDto
                    {
                        LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                    },
                    CustomersDto = new CustomersDto
                    {
                        CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                        CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                    },
                    StatusMasterDto = new StatusMasterDto
                    {
                        StatusId = cl.StatusId != null ? cl.StatusId.Value : 0,
                        StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                    }


                }).
                    ToList();
        }

        public bool UpdateDocumentCompletionStatus(List<int> claimIdList, int updateUserId)
        {
            try
            {
                foreach (var claimId in claimIdList)
                {
                    var claim = _unitOfWork.ClaimRepository.GetByID(claimId);
                    if (claim != null)
                    {
                        claim.StatusId = 8;
                        claim.UpdateDate = DateTime.Now;
                        claim.UpdateUserId = updateUserId;
                        _unitOfWork.ClaimRepository.Update(claim);
                    }
                }
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IntimationTransactionDto GetNewTransaction(string typeOfService)
        {
            try
            {
                var intimationTransaction = new IntimationTransaction
                {
                    CreatedDate = DateTime.Now,
                    TypeOfService = typeOfService
                };
                _unitOfWork.IntimationTransactionRepository.Insert(intimationTransaction);
                _unitOfWork.Save();
                return new IntimationTransactionDto
                {
                    Id = intimationTransaction.Id,
                    TransactionNumber = "ATX" + intimationTransaction.Id,
                    CreatedDate = intimationTransaction.CreatedDate != null ? intimationTransaction.CreatedDate.Value : DateTime.Now

                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdateTransaction(IntimationTransactionDto transaction)
        {
            try
            {
                var trans = _unitOfWork.IntimationTransactionRepository.GetByID(transaction.Id);
                if (trans != null)
                {
                    trans.TransactionNumber = transaction.TransactionNumber;
                    trans.Messgae = transaction.Message;
                    trans.ClaimId = transaction.ClaimId;
                    _unitOfWork.IntimationTransactionRepository.Update(trans);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateNiaTransactionNumber(IntimationTransactionDto transaction)
        {
            try
            {
                var claim = _unitOfWork.ClaimRepository.GetByID(transaction.ClaimId);
                if (claim != null)
                {
                    claim.NiaTransactionNumber = transaction.TransactionNumber;
                    _unitOfWork.ClaimRepository.Update(claim);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
