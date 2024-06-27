using Domin.Models;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using MimeKit;
using MimeKit.Text;
using Services.Abstracts;
using Services.Localiz;
using System.Text;

namespace Services.Implementations
{
    public class MailService : IMailService
    {


        #region InJect
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public MailService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IStringLocalizer<SharedResources> localizer)
        {
            _userManager = userManager;
            _configuration = configuration;
            _localizer = localizer;
        }
        #endregion

        #region Functions

        public async Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = _localizer[SharedResourcesKeys.UserNotFound]
                };
            //Decoding Token
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            // Success Confirm
            var result = await _userManager.ConfirmEmailAsync(user, normalToken);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return new UserManagerResponseDTO
                {
                    Message = _localizer[SharedResourcesKeys.EmailConfirm],
                    IsSuccess = true,
                };
            }

            // Invaild Confirm
            return new UserManagerResponseDTO
            {
                IsSuccess = false,
                Message = _localizer[SharedResourcesKeys.EmailNotConfirm],
                Errors = result.Errors.Select(e => e.Description)
            };

        }

        public void SendConfirmEmail(string EmailSentTo, string url)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["WebSite:FromName"], _configuration["WebSite:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(EmailSentTo));
            email.Subject = "Account Confirm";
            var htmlPage = $"<h1>Confirm your email</h1><p>Please confirm your email by <a href='{url}'>Clicking here</a></p>";
            email.Body = new TextPart(TextFormat.Html) { Text = htmlPage };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);

            smtp.Authenticate(userName: _configuration["WebSite:FromEmail"], password: _configuration["WebSite:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendResetPassword(string EmailSentTo, string code)
        {


            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["WebSite:FromName"], _configuration["WebSite:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(EmailSentTo));
            email.Subject = "Reset Password";
            var htmlPage = $"<h1>Reset Password</h1><p> your Reset password Code => {code}</p>";
            email.Body = new TextPart(TextFormat.Html) { Text = htmlPage };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);

            smtp.Authenticate(userName: _configuration["WebSite:FromEmail"], _configuration["WebSite:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        #endregion


    }
}
