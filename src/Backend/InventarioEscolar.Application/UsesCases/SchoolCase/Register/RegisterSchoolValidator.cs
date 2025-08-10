using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public class RegisterSchoolValidator : AbstractValidator<SchoolDto>
    {
        public RegisterSchoolValidator()
        {
           SchoolValidationRules.Apply(this);
        }
    }
}
