using Core.Features.Apartments.Commands.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Validators
{
    public class AddCommentValidators : AbstractValidator<AddCommentDTO>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddCommentValidators(IStringLocalizer<SharedResources> localizer)
        {

            ApplyValidationsRules();
            ApplyCustomValidationsRules(); // may Add maximam 3 comments to validaion?
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

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);


        }
        public void ApplyCustomValidationsRules()
        {

        }
    }
}
