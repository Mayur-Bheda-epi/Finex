using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finex.Buisness.IBuisnessHelper;
using Finex.Data.UnitOfWork;
using Finex.Dto.Dtos;
using Finex.Utility;

namespace Finex.Buisness.BuisnessHelper
{
    public class UserHelper : IUserHelper
    {

        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public UserHelper()
        {
            _unitOfWork = new UnitOfWork();
        }




        public UserMasterDto GetLogin(UserLoginDto userDto)
        {
            var user = _unitOfWork.UserMasterRepository.GetFirst(us => us.UserName == userDto.UserName && us.IsActive == true);
            if (user != null)
            {
                var password = userDto.Password != string.Empty ? Encryption.Encrypt(userDto.Password, true) : "";
                var pass =
                    _unitOfWork.PasswordRepository.GetFirst(
                        pw => pw.UserId == user.UserId && pw.Password1 == password);
                if (pass != null)
                    return new UserMasterDto
                               {
                                   Email = user.Email,
                                   Mobile = user.Mobile,
                                   Name = user.Name,
                                   UserId = user.UserId,
                                   UserTypeId = user.UserTypeId != null ? user.UserTypeId.Value : 0

                               };
            }
            return null;

        }


        public List<UserMasterDto> GetUsers()
        {
            return
                _unitOfWork.UserMasterRepository.GetMany(us => us.IsActive == true).Select(um => new UserMasterDto
                                                                                                     {
                                                                                                         Email = um.Email,
                                                                                                         Mobile = um.Mobile,
                                                                                                         Name = um.Name,
                                                                                                         UserName = um.UserName,
                                                                                                         UserType = um.UserTypeId != null ? um.UserType.Type : "",
                                                                                                         UserId = um.UserId

                                                                                                     }).
                    ToList();
        }


        public List<UserTypesDto> GetUserTypes()
        {
            return _unitOfWork.UserTypeRepository.GetAll().Select(ut => new UserTypesDto
                                                                            {
                                                                                Type = ut.Type,
                                                                                UseTypeId = ut.UseTypeId
                                                                            }).ToList();
        }


        public void AddUser(UserMasterDto userMasterDto)
        {
            var user = new Finex.Data.UserMaster();
            user.IsActive = true;
            user.UserName = userMasterDto.UserName;
            user.UserTypeId = userMasterDto.UserTypeId;
            user.Mobile = userMasterDto.Mobile;
            user.Name = userMasterDto.Name;
            user.Email = userMasterDto.Email;
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;
            var passwordList = new List<Finex.Data.Password>();
            var password = new Finex.Data.Password();
            password.Password1 = Encryption.Encrypt(userMasterDto.PasswordsDto.Password, true);
            passwordList.Add(password);
            user.Passwords = passwordList;
            _unitOfWork.UserMasterRepository.Insert(user);
            _unitOfWork.Save();
        }


        public void DeleteUser(List<int> userId, int updateBy)
        {
            foreach (var useId in userId)
            {
                var user = _unitOfWork.UserMasterRepository.GetByID(useId);
                if (user != null)
                {
                    user.IsActive = false;
                    user.UpdateDate = DateTime.Now;
                    _unitOfWork.UserMasterRepository.Update(user);
                }
            }
            _unitOfWork.Save();
        }


        public UpdateUserMasterDto GetUserByUserId(int userId)
        {
            var user = _unitOfWork.UserMasterRepository.GetByID(userId);
            var userDto = new UpdateUserMasterDto();
            if (user != null)
            {
                userDto.Email = user.Email;
                userDto.Mobile = user.Mobile;
                userDto.Name = user.Name;
                userDto.PasswordsDto = new UpdatePasswordDto
                                           {
                                              Password  = user.Passwords.FirstOrDefault() != null ? user.Passwords.FirstOrDefault().Password1 : ""
                                           };
                userDto.UserName = user.UserName;
                userDto.UserId = user.UserId;
                userDto.UserTypeId = user.UserTypeId != null ? user.UserTypeId.Value : 0;
            }
            return userDto;
        }


        public void UpdateUser(UpdateUserMasterDto userMasterDto)
        {
            var user = _unitOfWork.UserMasterRepository.GetByID(userMasterDto.UserId);
            if(user != null)
            {
                
                user.UserTypeId = userMasterDto.UserTypeId;
                user.Mobile = userMasterDto.Mobile;
                user.Name = userMasterDto.Name;
                user.Email = userMasterDto.Email;
                
                user.UpdateDate = DateTime.Now;
                if(!string.IsNullOrEmpty(userMasterDto.PasswordsDto.Password))
                {
                    var password = _unitOfWork.PasswordRepository.GetFirst(pa => pa.UserId == userMasterDto.UserId);
                    if(password != null)
                    {
                        password.Password1 = Encryption.Encrypt(userMasterDto.PasswordsDto.Password, true);
                        _unitOfWork.PasswordRepository.Update(password);
                    }
                }
                _unitOfWork.UserMasterRepository.Update(user);
            }
            _unitOfWork.Save();
        }
    }
}
