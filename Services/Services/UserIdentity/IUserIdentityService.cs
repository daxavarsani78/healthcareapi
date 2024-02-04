using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ViewModels.RequestModel;

namespace Services.Services.UserIdentity
{
    public interface IUserIdentityService
    {
        Task<IdentityResult> UserSignupAsync(UserSignup userSignup);
        Task<string> UserLoginAsync(UserSignin userSignup);
        //Task<IdentityResult> UserUpdatePasswordAsync(ForgotPassword forgotPassword);
        //Task<bool> UserSendLinkForUpdatePasswordAsync(string userName);
    }
}
