using Core.Features.Users.Commands.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Users.Commands.Validators
{
    public class ChangePasswordUserCommandValidation : AbstractValidator<ChangePasswordUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ChangePasswordUserCommandValidation(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationRules();
            _localizer = localizer;
        }
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.EmailAddress])
                ;//Make White list of allowable addresses

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
                if (model.NewPassword != model.ConfirmPassword)
                {
                    context.AddFailure("ConfirmPassword", _localizer[SharedResourcesKeys.ConfirmPassword]);
                }
            });

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .MinimumLength(6).WithMessage(_localizer[SharedResourcesKeys.Min6]);
            //make custom validation about numbers and nonAlphabitc characters
        }
    }
}
