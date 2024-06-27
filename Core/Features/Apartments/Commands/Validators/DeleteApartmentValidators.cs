using Core.Features.Apartments.Commands.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Validators
{
    public class DeleteApartmentValidators : AbstractValidator<DeleteApartmentDTO>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteApartmentValidators(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationsRules();
            _localizer = localizer;
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);


        }
    }
}
