using Core.Features.Apartments.Commands.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Validators
{
    public class EditApartmentValidators : AbstractValidator<EditApartmentDTO>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public EditApartmentValidators(IStringLocalizer<SharedResources> localizer)
        {

            ApplyValidationsRule();
            _localizer = localizer;
        }

        public void ApplyValidationsRule()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull]);

            RuleFor(x => x.Title)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                 .MinimumLength(4).WithMessage(_localizer[SharedResourcesKeys.Min4])
                 .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150]);

            RuleFor(x => x.Description)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                 .MinimumLength(4).WithMessage(_localizer[SharedResourcesKeys.Min4])
                 .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150]);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .MinimumLength(4).WithMessage(_localizer[SharedResourcesKeys.Min4])
                .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150]);

            RuleFor(x => x.gender)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .Must(CorrectGender).WithMessage(_localizer[SharedResourcesKeys.CorrectGender]);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .MinimumLength(3).WithMessage(_localizer[SharedResourcesKeys.Min4])
                .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150]);

            RuleFor(x => x.NewCoverImage)
              .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoNull])
              .Must(file => BeAValidSize(file, 1024 * 1024)).WithMessage(_localizer[SharedResourcesKeys.MaxSizePic]);

            RuleFor(x => x.ApartmentsImagesUrl)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoNull]);

            RuleFor(x => x.NewPics)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .ForEach(file =>
                {
                    file.Must(f => BeAValidSize(f, 1024 * 1024 * 1))
                    .WithMessage(_localizer[SharedResourcesKeys.MaxSizePic]);
                });

            RuleFor(x => x.NewVideo)
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
               .Must(file => BeAValidSize(file, 1024 * 1024 * 100)).WithMessage(_localizer[SharedResourcesKeys.MaxSizeVid]);
        }

        private bool CorrectGender(string g)
        {
            if (string.IsNullOrEmpty(g) || g.ToLower() != "male" || g.ToLower() != "female")
                return false;
            return true;
        }
        private bool BeAValidSize(IFormFile file, int FileMaxSize) // 1024 * 1024
        {
            return file == null || file.Length <= FileMaxSize;
        }
    }
}
