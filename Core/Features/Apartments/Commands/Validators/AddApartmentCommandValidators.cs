using Core.Features.Apartments.Commands.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Services.Localiz;

namespace Core.Features.Apartments.Commands.Validators
{
    public class AddApartmentCommandValidators : AbstractValidator<AddApartmentDTO>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        #region Fields

        #endregion

        #region CTOR
        public AddApartmentCommandValidators(IStringLocalizer<SharedResources> localizer)
        {
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _localizer = localizer;
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150])
                .MinimumLength(4).WithMessage(_localizer[SharedResourcesKeys.Min4]);

            RuleFor(x => x.Description)
                .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150]);

            RuleFor(x => x.Address)
                .MaximumLength(150).WithMessage(_localizer[SharedResourcesKeys.Max150]);

            RuleFor(x => x.RoyalDocument)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
                .Must(file => BeAValidSize(file, 1024 * 1024)).WithMessage(_localizer[SharedResourcesKeys.MaxSizePic]);

            RuleFor(x => x.CoverImage)
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
               .Must(file => BeAValidSize(file, 1024 * 1024)).WithMessage(_localizer[SharedResourcesKeys.MaxSizePic]);

            RuleFor(x => x.Pics)
           .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
           .Must(files => files != null && files.Count > 0).WithMessage(_localizer[SharedResourcesKeys.NoEmpty])
           .ForEach(file =>
           {
               file.Must(f => BeAValidSize(f, 1024 * 1024)).WithMessage(_localizer[SharedResourcesKeys.MaxSizePic]);
           });

            RuleFor(x => x.UserId)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NoNull])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NoEmpty]);

        }
        private bool BeAValidSize(IFormFile file, int FileMaxSize) // 1024 * 1024
        {
            return file == null || file.Length <= FileMaxSize;
        }
        public void ApplyCustomValidationsRules()
        {
            //RuleFor(x => x.Name).MustAsync(async (key, CancellationToken) => !await _studentSevice.IsNameExist(key))
            //  .WithMessage("This Name is already Exist ya ma3lem");
        }
        #endregion
    }
}
