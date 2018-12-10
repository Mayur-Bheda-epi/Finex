using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Finex.Dto.Dtos;

namespace Finex.Data.GenericRepository
{
    public class QueryRepository
    {
        #region Private member variables...
        internal FinexAlmondzEntities Context;

        #endregion

        #region Public Constructor...
        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="context"></param>
        ///
        public QueryRepository()
        {
            Context = new FinexAlmondzEntities();
        }

        #endregion

        public List<SummaryDto> GetSummaryClaim(int userId, int userTypeId)
        {
            //if (userTypeId == 1)
            return Context.Claims.Where(ssm => ssm.IsActive == true).GroupBy(p => p.StatusMaster.StatusName)
               .Select(g => new SummaryDto { Status = g.Key, Count = g.Count() }).ToList();
            //return Context.Claims.Where(ssm => ssm.CreateUserId == userId && ssm.IsActive == true).GroupBy(p => p.StatusMaster.StatusName)
            //       .Select(g => new SummaryDto { Status = g.Key, Count = g.Count() }).ToList();
        }

        public List<ClaimsDto> GetClaimSearch(ClaimSearchDto searchDto, int pageSize, int page, bool isPaging = true)
        {
            IQueryable<ClaimsDto> outQuery;
            //if (searchDto.UserTypeId == 1)
            //{
            outQuery = from cl in Context.Claims
                       where cl.IsActive == true
                       select new ClaimsDto
                       {
                           AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
                           CardNo = cl.CardNo,
                           DateSinceIntimation = cl.DateSinceIntimation,
                           NoOfSecuredTrans = cl.NoOfSecuredTrans,
                           NoOfUnsecuredTrans = cl.NoOfUnsecuredTrans,
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
                           LabelCode = cl.LabelCode,
                           LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
                           LossTypeMasterDto = new LossTypeMasterDto
                           {
                               LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
                           },
                           CustomersDto = new CustomersDto
                           {
                               CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
                               CustomerName = cl.CustId != null ? cl.Customer.Name : "",
                               MobileNumber = cl.CustId != null ? cl.Customer.MobileNo : "",
                               EmailId = cl.CustId != null ? cl.Customer.Email : ""
                           },
                           StatusMasterDto = new StatusMasterDto
                           {
                               StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                           },
                           Comment = cl.Comment,
                           AccountNumber = cl.AccountNumber,
                           StatusId = cl.StatusId != null ? cl.StatusId.Value : 0

                       };
            //}
            //else
            //{
            //    outQuery = from cl in Context.Claims
            //               where cl.CreateUserId == searchDto.UserId && cl.IsActive == true
            //               select new ClaimsDto
            //               {
            //                   AmountOfLoss = cl.AmountOfLoss != null ? cl.AmountOfLoss.Value : 0,
            //                   CardNo = cl.CardNo,
            //                   FileNo = cl.FileNo,
            //                   ClaimAmount = cl.ClaimAmount != null ? cl.ClaimAmount.Value : 0,
            //                   CardTypeMasterDto = new CardTypeMasterDto
            //                   {
            //                       CardTypeName = cl.CardTypeId != null ? cl.CardTypeMaster.CardTypeName : ""
            //                   },
            //                   ClaimNumber = cl.ClaimNumber,
            //                   DateIntimationBank = cl.DateIntimationBank != null ? cl.DateIntimationBank.Value : DateTime.MinValue,
            //                   CustId = cl.CustId != null ? cl.CustId.Value : 0,
            //                   ClaimId = cl.ClaimId,
            //                   DateOfLoss = cl.DateOfLoss != null ? cl.DateOfLoss.Value : DateTime.MinValue,
            //                   MerchantShop = cl.MerchantShop,
            //                   PassportNo = cl.PassportNo,
            //                   LossTypeId = cl.LossTypeId != null ? cl.LossTypeId.Value : 0,
            //                   LossTypeMasterDto = new LossTypeMasterDto
            //                   {
            //                       LossType = cl.LossTypeId != null ? cl.LossTypeMaster.LossType : ""
            //                   },
            //                   CustomersDto = new CustomersDto
            //                   {
            //                       CustomerId = cl.CustId != null ? cl.CustId.Value : 0,
            //                       CustomerName = cl.CustId != null ? cl.Customer.Name : "",
            //                       MobileNumber = cl.CustId != null ? cl.Customer.MobileNo : "",
            //                       EmailId = cl.CustId != null ? cl.Customer.Email : ""
            //                   },
            //                   StatusMasterDto = new StatusMasterDto
            //                   {
            //                       StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
            //                   },
            //                   Comment = cl.Comment,
            //                   AccountNumber = cl.AccountNumber

            //               };
            //}


            if (!string.IsNullOrEmpty(searchDto.Name))
            {
                outQuery = outQuery.Where(x =>
                    x.CustomersDto.CustomerName.Contains(searchDto.Name));
            }
            if (!string.IsNullOrEmpty(searchDto.AccountNo))
            {
                outQuery = outQuery.Where(x => x.AccountNumber.Contains(searchDto.AccountNo));
            }
            if (!string.IsNullOrEmpty(searchDto.CardNo))
            {
                outQuery = outQuery.Where(x => x.CardNo.Contains(searchDto.CardNo));
            }
            if (!string.IsNullOrEmpty(searchDto.MobileNo))
            {
                outQuery = outQuery.Where(x => x.CustomersDto.MobileNumber.Contains(searchDto.MobileNo));
            }
            if (!string.IsNullOrEmpty(searchDto.EmailId))
            {
                outQuery = outQuery.Where(x => x.CustomersDto.EmailId.Contains(searchDto.EmailId));
            }
            if (isPaging)
                return outQuery.OrderBy(or => or.ClaimId).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            else
                return outQuery.ToList();
        }

        public List<SummaryDto> GetSummaryClaimByClaimIds(List<int> claimIdsList)
        {
            return Context.Claims.Where(ssm => claimIdsList.Contains(ssm.ClaimId)).GroupBy(p => p.StatusMaster.StatusName)
                  .Select(g => new SummaryDto { Status = g.Key, Count = g.Count() }).ToList();
        }

        public List<DashboardDto> GetDashBoard(int userId)
        {
            var resultSetDashboard = new List<DashboardDataDto>();
            if (userId > 0)
            {
                resultSetDashboard =
                    Context.Claims.Where(cl => cl.CreateUserId == userId).GroupBy(
                        x => new { x.LossTypeId, x.StatusId, x.StatusMaster.StatusName, x.LossTypeMaster.LossType }).
                        Select(
                            y =>
                            new DashboardDataDto
                            {
                                LossTypeId = y.Key.LossTypeId.Value,
                                LossTypeName = y.Key.LossType,
                                StatusId = y.Key.StatusId.Value,
                                Sum = y.Sum(t => t.ClaimAmount != null ? t.ClaimAmount.Value : 0),
                                Count = y.Count()
                            }).ToList();
            }
            else
            {
                resultSetDashboard =
                    Context.Claims.GroupBy(
                        x => new { x.LossTypeId, x.StatusId, x.StatusMaster.StatusName, x.LossTypeMaster.LossType }).
                        Select(
                            y =>
                            new DashboardDataDto
                            {
                                LossTypeId = y.Key.LossTypeId.Value,
                                LossTypeName = y.Key.LossType,
                                StatusId = y.Key.StatusId.Value,
                                Sum = y.Sum(t => t.ClaimAmount != null ? t.ClaimAmount.Value : 0),
                                Count = y.Count()
                            }).ToList();
            }

            var dashboardDtoList = new List<DashboardDto>();
            foreach (var dashboardObj in resultSetDashboard)
            {
                var isNew = false;
                var existingObj = dashboardDtoList.FirstOrDefault(ddl => ddl.LossTypeId == dashboardObj.LossTypeId);
                if (existingObj == null)
                {
                    isNew = true;
                    existingObj = new DashboardDto();
                    existingObj.LossType = dashboardObj.LossTypeName;
                    existingObj.LossTypeId = dashboardObj.LossTypeId;

                }
                switch (dashboardObj.StatusId)
                {
                    case 1:
                        existingObj.SettledAmount = dashboardObj.Sum;
                        existingObj.SettledNumber = dashboardObj.Count;
                        break;
                    case 3:
                        existingObj.ApprovadAmount = dashboardObj.Sum;
                        existingObj.ApprovedNumber = dashboardObj.Count;
                        break;
                    case 5:
                        existingObj.RejectedAmount = dashboardObj.Sum;
                        existingObj.RejectedNumber = dashboardObj.Count;
                        break;
                    case 6:
                        existingObj.InitiatedAmount = dashboardObj.Sum;
                        existingObj.InitiatedNumber = dashboardObj.Count;
                        break;
                    case 7:
                        existingObj.DocumentPendingAmount = dashboardObj.Sum;
                        existingObj.DocumentPendingNumber = dashboardObj.Count;
                        break;
                    case 8:
                        existingObj.DocumentCompletionAmount = dashboardObj.Sum;
                        existingObj.DocumentCompletionNumber = dashboardObj.Count;
                        break;





                }
                if (isNew)
                    dashboardDtoList.Add(existingObj);
            }
            return dashboardDtoList;
        }

        public int GetTotalClaimCountForSearch(ClaimSearchDto searchDto)
        {
            IQueryable<ClaimsDto> outQuery;

            outQuery = from cl in Context.Claims
                       where cl.IsActive == true
                       select new ClaimsDto
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
                               MobileNumber = cl.CustId != null ? cl.Customer.MobileNo : "",
                               EmailId = cl.CustId != null ? cl.Customer.Email : ""
                           },
                           StatusMasterDto = new StatusMasterDto
                           {
                               StatusName = cl.StatusId != null ? cl.StatusMaster.StatusName : ""
                           },
                           Comment = cl.Comment,
                           AccountNumber = cl.AccountNumber

                       };



            if (!string.IsNullOrEmpty(searchDto.Name))
            {
                outQuery = outQuery.Where(x =>
                    x.CustomersDto.CustomerName.Contains(searchDto.Name));
            }
            if (!string.IsNullOrEmpty(searchDto.AccountNo))
            {
                outQuery = outQuery.Where(x => x.AccountNumber.Contains(searchDto.AccountNo));
            }
            if (!string.IsNullOrEmpty(searchDto.CardNo))
            {
                outQuery = outQuery.Where(x => x.CardNo.Contains(searchDto.CardNo));
            }
            if (!string.IsNullOrEmpty(searchDto.MobileNo))
            {
                outQuery = outQuery.Where(x => x.CustomersDto.MobileNumber.Contains(searchDto.MobileNo));
            }
            if (!string.IsNullOrEmpty(searchDto.EmailId))
            {
                outQuery = outQuery.Where(x => x.CustomersDto.EmailId.Contains(searchDto.EmailId));
            }
            return outQuery.Count();
        }
    }
}
