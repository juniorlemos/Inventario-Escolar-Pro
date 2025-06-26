using FluentValidation;
using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.Services.Validators.Rules
{
    public static class SchoolValidationRules
    {
        public static void Apply<T>(AbstractValidator<T> validator) where T : ISchoolBaseDto
        {
            validator.RuleFor(x => x.Name)
          .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
          .MinimumLength(2).WithMessage(ResourceMessagesException.SCHOOL_NAME_TOOSHORT)
          .MaximumLength(100).WithMessage(ResourceMessagesException.SCHOOL_NAME_TOOLONG);

            validator.RuleFor(x => x.Inep)
                .MaximumLength(20).WithMessage(ResourceMessagesException.SCHOOL_INEP_TOOLONG);

            validator.RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage(ResourceMessagesException.SCHOOL_ADDRESS_TOOLONG);

            validator.RuleFor(x => x.City)
                .MaximumLength(30).WithMessage(ResourceMessagesException.SCHOOL_CITY_TOOLONG);
        }
    }
}