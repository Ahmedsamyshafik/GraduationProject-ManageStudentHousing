using Core.Features.Users.Commands.Models.ApartmentsRquests;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Users.Commands.Validators.ApartmentRequestsValidators
{
    public class RequestApartmentStudentCommandValidation : AbstractValidator<RequestApartmentStudentCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RequestApartmentStudentCommandValidation(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationsRule();
            _localizer = localizer;
        }
        public void ApplyValidationsRule()
        {
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);

            RuleFor(x => x.ApartmentID)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.idBigerThan0]);
        }
    }
}
