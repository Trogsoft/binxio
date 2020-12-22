using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Users;
using Binxio.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly BinxioDb db;
        private readonly IXioMapper mapper;

        public UserRepository(BinxioDb db, IXioMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<XioResult<UserModel>> CreateUser(CreateUserModel model)
        {

            var extant = await UserExists(new SingleUserQueryModel { EmailAddress = model.EmailAddress, MicrosoftAccountId = model.MicrosoftAccountId });
            if (extant.IsSuccessful)
            {
                return new XioResult<UserModel>(false, "User already exists.");
            }

            var client = await db.Clients.SingleOrDefaultAsync(x => x.Id == model.ClientId);
            if (client == null)
                return new XioResult<UserModel>(false, "Client not found.");


            bool isClientAdmin = false;
            if (!await db.Users.AnyAsync(x => x.ClientId == model.ClientId))
                isClientAdmin = true;

            var newUser = db.Users.Add(new User
            {
                ClientId = model.ClientId,
                Created = DateTime.Now,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                IsClientAdmin = isClientAdmin,
                JobTitle = model.JobTitle,
                LastName = model.LastName,
                LastUsed = DateTime.Now,
                MicrosoftAccessToken = model.AccessToken,
                MicrosoftAccountId = model.MicrosoftAccountId,
                PreferredLanguage = model.PreferredLanguage,
                UrlPart = db.GetUrlPart<User>($"{model.FirstName} {model.LastName}")
            });

            await db.SaveChangesAsync();

            return new XioResult<UserModel>(true, mapper.Map<UserModel>(newUser.Entity));

        }

        public Task<XioResult<UserModel>> GetUser(SingleUserQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<XioResult<IEnumerable<UserPublicInfoModel>>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetClaimsPrincipal(UserPublicInfoModel model)
        {
            var user = db.Users.SingleOrDefault(x => x.Id == model.Id);
            if (user == null)
                throw new Exception("User not found.");

            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.NameIdentifier, model.UrlPart),
                 new Claim(ClaimTypes.Name, model.DisplayName),
                 new Claim(ClaimTypes.Email, user.EmailAddress)
            };
            var cid = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(cid);
        }

        public async Task<XioResult<UserPublicInfoModel>> UserExists(SingleUserQueryModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.EmailAddress))
            {
                var user = await db.Users.SingleOrDefaultAsync(x => x.EmailAddress == model.EmailAddress);
                if (user != null)
                    return new XioResult<UserPublicInfoModel>(true, mapper.Map<UserPublicInfoModel>(user));
            }

            if (model.MicrosoftAccountId != Guid.Empty)
            {
                var user = await db.Users.SingleOrDefaultAsync(x => x.MicrosoftAccountId == model.MicrosoftAccountId);
                if (user != null)
                    return new XioResult<UserPublicInfoModel>(true, mapper.Map<UserPublicInfoModel>(user));
            }

            return new XioResult<UserPublicInfoModel>(false);

        }
    }
}
