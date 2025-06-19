using FluentValidation;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public class RegisterSchoolValidator : AbstractValidator<SchoolDto>
    {
        public RegisterSchoolValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
             .MinimumLength(2).WithMessage(ResourceMessagesException.SCHOOL_NAME_TOOSHORT)
             .MaximumLength(100).WithMessage(ResourceMessagesException.SCHOOL_NAME_TOOLONG);

            RuleFor(x => x.Inep)
                .MaximumLength(20).WithMessage(ResourceMessagesException.SCHOOL_INEP_TOOLONG);

            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage(ResourceMessagesException.SCHOOL_ADDRESS_TOOLONG);

            RuleFor(x => x.City)
                .MaximumLength(30).WithMessage(ResourceMessagesException.SCHOOL_CITY_TOOLONG);
        }
    }
}
