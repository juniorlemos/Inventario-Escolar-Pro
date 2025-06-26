using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public class UpdateSchoolValidator : AbstractValidator<UpdateSchoolDto>
    {
        public UpdateSchoolValidator()
        {
            SchoolValidationRules.Apply(this);
        }
    }
}
