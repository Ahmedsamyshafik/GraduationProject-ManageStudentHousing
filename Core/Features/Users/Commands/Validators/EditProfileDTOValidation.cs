using Core.Features.Users.Commands.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Services.Abstracts;
using Services.Localiz;

namespace Core.Features.Users.Commands.Validators
{
    public class EditProfileDTOValidation : AbstractValidator<EditProfileDTO>
    {
        private readonly IAuthService _studentSevice;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public EditProfileDTOValidation(IAuthService studentSevice, IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationRules();
            _studentSevice = studentSevice;
            _localizer = localizer;
            // ApplyCustomValidationsRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .MinimumLength(2).WithMessage(_localizer[SharedResourcesKeys.Min2])
                .Must(x => !x.Contains(" ")).WithMessage(_localizer[SharedResourcesKeys.NoSpaceInName]);
            //Custom Validation don't repeat in DB

            RuleFor(x => x.Email)
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.EmailAddress]);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .Length(11).WithMessage(_localizer[SharedResourcesKeys.PhoneLenght])
                .Must(IsAllDigits).WithMessage(_localizer[SharedResourcesKeys.PhoneDigit]);
            //  .Matches(@"^\d{11}$").WithMessage("")
            ;

            RuleFor(x => x.Picture)
                .Must(file => BeAValidSize(file, 1024 * 1024 * 1)).WithMessage(_localizer[SharedResourcesKeys.MaxSizePic]);


        }
        private bool IsAllDigits(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return false;
            }

            if (phoneNumber.Length != 11)
            {
                return false;
            }

            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        private bool BeAValidSize(IFormFile file, int FileMaxSize) // 1024 * 1024
        {

            return file == null || file.Length <= FileMaxSize;
        }
        public void ApplyCustomValidationsRules()
        {
            //RuleFor(x => x.Name)
            //    .MustAsync(async (key, CancellationToken) => ! _studentSevice.NameIsExist(key))
            //  .WithMessage("This Name is already Exist ya ma3lem");



        }
    }
}
