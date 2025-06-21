using FluentValidation;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public class UpdateSchoolValidator : AbstractValidator<SchoolDto>
    {
        public UpdateSchoolValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ResourceMessagesException.SCHOOL_ID_INVALID);

            Include(new RegisterSchoolValidator());
        }
    }
}
