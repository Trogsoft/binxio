using Binxio.Common;
using Binxio.Common.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Binxio.Abstractions
{
    public interface IUserRepository 
    {
        Task<XioResult<IEnumerable<UserPublicInfoModel>>> GetUsers();
        Task<XioResult<UserPublicInfoModel>> UserExists(SingleUserQueryModel model);
        Task<XioResult<UserModel>> GetUser(SingleUserQueryModel model);
        Task<XioResult<UserModel>> CreateUser(CreateUserModel model);
        ClaimsPrincipal GetClaimsPrincipal(UserPublicInfoModel model);
    }
}
