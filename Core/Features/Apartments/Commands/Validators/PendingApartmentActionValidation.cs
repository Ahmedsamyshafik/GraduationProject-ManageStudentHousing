using Core.Features.Apartments.Commands.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Validators
{
    public class PendingApartmentActionValidation : AbstractValidator<PendingApartmentAction>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public PendingApartmentActionValidation(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationRules();
            _localizer = localizer;
        }
        public void ApplyValidationRules()
        {
            RuleFor(x => x.ApartmentID)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);

            RuleFor(x => x.Accept)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);
        }
    }
}
