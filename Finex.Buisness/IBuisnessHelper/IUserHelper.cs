using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finex.Dto.Dtos;

namespace Finex.Buisness.IBuisnessHelper
{
  public interface IUserHelper
    {

       UserMasterDto  GetLogin(UserLoginDto userDto);

       List<UserMasterDto> GetUsers();

       List<UserTypesDto> GetUserTypes();

       void AddUser(UserMasterDto userMasterDto);

       void DeleteUser(List<int> userId, int updateBy);

       UpdateUserMasterDto GetUserByUserId(int userId);

       void UpdateUser(UpdateUserMasterDto userMasterDto);
    }
}
