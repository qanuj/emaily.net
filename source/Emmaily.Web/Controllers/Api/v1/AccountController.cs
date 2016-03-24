using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Data;
using Emaily.Services.Extensions;
using Emaily.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Emaily.Web.Controllers.Api.v1
{
    /// <summary>
    ///  Manages system accounts.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/account")]
    public class AccountController : BasicApiController
    {
        private readonly ApplicationUserManager _userManager; 
        private readonly IRepository<UserProfile> _userProfileRepository;

        public AccountController(ApplicationUserManager userManager, IRepository<UserProfile> userProfileRepository)
        {
            _userManager = userManager;
            _userProfileRepository = userProfileRepository;
        }


        /// <summary>
        /// Returns user account details
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("me")]
        public HttpResponseMessage My()
        {
            var claims = this._userManager.GetClaims(this.User.Identity.GetUserId()).Where(x => x.Type.Contains(":about"))
                        .Select(x => new { name = x.Type.Split(new[] { ':' })[0], config = x.Value.ToDynamic() });
            var ctx = HttpContext.Current.GetOwinContext().Authentication;
            var others = ctx.GetExternalAuthenticationTypes().Select(x => new { type = x.AuthenticationType, caption = x.Caption });
            return Ok(new { claims, others });
        }

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete, Route("me/{provider}/{key}")]
        public async Task<HttpResponseMessage> Delete(string provider, string key)
        {
            var result = await _userManager.RemoveLoginAsync(this.User.Identity.GetUserId(), new UserLoginInfo(provider, key));
            return Ok(result);
        }

        /// <summary>
        /// Returns current user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("me/profile")]
        public UserProfileViewModel Profile()
        {
            var user= _userManager.FindById(User.Identity.GetUserId());
            var profile = _userProfileRepository.ById(user.ProfileId??0);
            user.Profile = profile;
            return new UserProfileViewModel
            {
                Name = user.Profile?.Name,
                Picture = user.Profile?.Picture,
                Email = user.Email,
                PhoneNumber=user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed =  user.PhoneNumberConfirmed,
                Address = user.Profile?.Address,
                Birthday = user.Profile?.Birthday,
                Country = user.Profile?.Country,
                Postcode = user.Profile?.Postcode
            };
        }

        /// <summary>
        /// POST command to edit user profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>              
        [HttpPut, Route("me/profile")]
        public HttpResponseMessage Post(UserProfileEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindById(User.Identity.GetUserId());
                if(user.Profile==null) user.Profile=new UserProfile();

                if (user.Email != model.Email)
                {
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                }
                if (user.PhoneNumber != model.PhoneNumber)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.PhoneNumberConfirmed = false;
                }
                user.Profile.Picture = model.Picture;
                user.Profile.Name = model.Name;
                user.Profile.Birthday = model.Birthday;
                user.Profile.Country = model.Country;
                user.Profile.Address = model.Address;
                user.Profile.Postcode = model.Postcode;
                _userManager.Update(user);
                return Accepted(Profile());
            }
            return Bad(ModelState);
        }
        
    }
}
