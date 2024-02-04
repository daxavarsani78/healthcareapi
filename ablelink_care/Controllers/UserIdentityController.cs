using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.UserIdentity;
using ViewModels.ViewModels.RequestModel;
using ViewModels.ViewModels.ResponseModel;

namespace AblelinkCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserIdentityController : BaseController
    {
        private readonly IUserIdentityService _userIdentityService;

        public UserIdentityController(IUserIdentityService userIdentityService)
        {
            _userIdentityService = userIdentityService;
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> SignupNewUser([FromBody] UserSignup userSignup)
        {
            if (userSignup.Secret == "AbleLink@880459200")
            {
                var result = await _userIdentityService.UserSignupAsync(userSignup);

                if (result != null && result.Succeeded)
                {
                    return Ok(new ResponseBaseModel(200, "User created successfully."));
                }
            }
            return Unauthorized();
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> LoginUser([FromBody] UserSignin userSignin)
        {
            var result = await _userIdentityService.UserLoginAsync(userSignin);

            if (result != null)
            {
                return Ok(new ResponseBaseModel(200, new { token = result }, "User signin successfully."));
            }
            return Unauthorized();
        }

        //[HttpPost("update-password")]
        //public async Task<ActionResult> UserUpdatePasswordAsync([FromBody] ForgotPassword request)
        //{
        //    var result = await _userIdentityService.UserUpdatePasswordAsync(request);

        //    if (result != null)
        //    {
        //        return Ok(new ResponseBaseModel(200, "Password updated successfully."));
        //    }
        //    return Unauthorized();
        //}

        //[HttpPost("forgot-password-request")]
        //public async Task<ActionResult> UserUpdatePasswordAsync([FromBody] string request)
        //{
        //    var result = await _userIdentityService.UserSendLinkForUpdatePasswordAsync(request);

        //    if (result)
        //    {
        //        return Ok(new ResponseBaseModel(200, "Please check your email to update password."));
        //    }
        //    return Unauthorized();
        //}
    }
}
