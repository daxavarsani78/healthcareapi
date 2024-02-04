using Data.Data;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ViewModels.RequestModel;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Common;

namespace Services.Services.UserIdentity
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly UserManager<Tbl_Users> _userManager;
        private readonly SignInManager<Tbl_Users> _signinManager;
        private readonly IConfiguration _configuration;
        private readonly AppDB _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentityService(IHttpContextAccessor httpContextAccessor, AppDB dbContext, UserManager<Tbl_Users> userManager, SignInManager<Tbl_Users> signInManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signinManager = signInManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> UserSignupAsync(UserSignup userSignup)
        {
            try
            {
                Tbl_Users user = new Tbl_Users()
                {
                    FirstName = userSignup.FirstName,
                    LastName = userSignup.LastName,
                    Email = userSignup.Email,
                    UserName = userSignup.FirstName + userSignup.LastName,
                    PhoneNumber = userSignup.PhoneNumber,
                    UPassword = userSignup.Password,
                    IsActive = true,
                };
                return await _userManager.CreateAsync(user, userSignup.Password);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public async Task<IdentityResult> UserUpdatePasswordAsync(ForgotPassword forgotPassword)
        //{
        //    Data.Entity.Tbl_Users? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == forgotPassword.userName && x.SecurityCode == forgotPassword.securityCode);
        //    if (user != null)
        //    {
        //        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        return await _userManager.ResetPasswordAsync(user, token, forgotPassword.password);
        //    }
        //    else
        //        throw new Exception("Something went wrong, please try again.");
        //}

        //public async Task<bool> UserSendLinkForUpdatePasswordAsync(string userName)
        //{
        //    Data.Entity.Tbl_Users? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }
        //    CommonService commonService = new CommonService();
        //    string code = commonService.GenerateNNumberRandomString(6);
        //    user.SecurityCode = code;
        //    await _userManager.UpdateAsync(user);
        //    //CommonHelper.SendMail("mailFrom??",request.Email,"Your inquiry has been successfully received",getEmailHtml(request.Name),true,"CC Admin mailid??");
        //    return true;
        //    //return SendEmail(user.Email, "Please find security code in email", code);
        //}

        public async Task<string?> UserLoginAsync(UserSignin userSigin)
        {
            if (userSigin.Otp != null && userSigin.Otp != 0)
            {
                Tbl_Users? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userSigin.Email && x.SecurityCode == userSigin.Otp);

                if (user == null)
                    throw new Exception("User not found.");

                var result = await _signinManager.PasswordSignInAsync(user.UserName, user.UPassword, false, false);

                if (!result.Succeeded)
                    throw new Exception("Something went wrong.");

                if (result.Succeeded)
                {
                    var authClaims = new List<Claim>
                {
                new Claim("Id", user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Email", user.Email),
                new Claim("PhoneNumber", user.PhoneNumber),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                       issuer: _configuration["JWT:ValidIssuer"],
                       audience: _configuration["JWT:ValidAudience"],
                       expires: DateTime.Now.AddDays(30),
                       claims: authClaims,
                       signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                       );
                    user.SecurityCode = null;
                    await _userManager.UpdateAsync(user);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            else
            {
                Tbl_Users? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userSigin.Email);

                if (user == null)
                    throw new Exception("User not found.");
                int code = CommonService.Generate6NumberRandomString();
                user.SecurityCode = code;
                await _userManager.UpdateAsync(user);
                CommonService.SendEmail("renishribadiya10@outlook.com", user.Email, "Admin Login OTP", code.ToString(), false);
                return "Code successfully send to email id";
            }
            return null;
        }
    }
}
