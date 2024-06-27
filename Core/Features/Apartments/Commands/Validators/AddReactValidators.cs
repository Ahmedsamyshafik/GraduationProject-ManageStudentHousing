using Core.Features.Apartments.Commands.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Validators
{
    public class AddReactValidators : AbstractValidator<AddReactDTO>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddReactValidators(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationsRules();
            _localizer = localizer;
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);

            RuleFor(x => x.ApartmentID)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);
        }
    }
}
