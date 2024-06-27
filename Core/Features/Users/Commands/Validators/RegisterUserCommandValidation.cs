using Core.Features.Users.Commands.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Users.Commands.Validators
{
    public class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RegisterUserCommandValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }
        public void ApplyValidationRules()
        {

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .Must(x => !x.Contains(" ")).WithMessage(_localizer[SharedResourcesKeys.NoSpaceInName]);
            //not same name in db
            RuleFor(x => x.Phone)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
               .Length(11).WithMessage(_localizer[SharedResourcesKeys.PhoneLenght])
               .Must(IsAllDigits).WithMessage(_localizer[SharedResourcesKeys.PhoneDigit]);

            RuleFor(x => x.Email)
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
              .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.EmailAddress]);

            RuleFor(x => x.Gender)
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .Must(CorrectGender).WithMessage(_localizer[SharedResourcesKeys.CorrectGender]);

            RuleFor(x => x.UserType)
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
              .Must(CorrectUserType).WithMessage(_localizer[SharedResourcesKeys.CorrectUserType]);

            RuleFor(x => x.Password)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
              .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
              .MinimumLength(6).WithMessage(_localizer[SharedResourcesKeys.Min6])
              ;//make custom validation about numbers and nonAlphabitc characters

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .MinimumLength(6).WithMessage(_localizer[SharedResourcesKeys.Min6]);
            RuleFor(x => x).Custom((model, context) =>
            {
                if (model.Password != model.ConfirmPassword)
                {
                    context.AddFailure("ConfirmPassword", _localizer[SharedResourcesKeys.ConfirmPassword]);
                }
            });

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .MinimumLength(6).WithMessage(_localizer[SharedResourcesKeys.Min6]);
            //make custom validation about numbers and nonAlphabitc characters
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

        private bool CorrectGender(string g)
        {
            if (string.IsNullOrEmpty(g))
                return false;



            if (g.ToLower() != "female" && g.ToLower() != "male")
            {
                return false;
            }



            return true;
        }
        private bool CorrectUserType(string g)
        {
            if (string.IsNullOrEmpty(g) || g.ToLower() != "user" || g.ToLower() != "owner")
                return false;
            return true;
        }

    }
}
