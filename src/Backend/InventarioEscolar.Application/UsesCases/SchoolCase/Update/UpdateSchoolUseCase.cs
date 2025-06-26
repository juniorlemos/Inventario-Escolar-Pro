using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public class UpdateSchoolUseCase(
       IUnitOfWork unitOfWork,
       IValidator<UpdateSchoolDto> validator,
       ISchoolReadOnlyRepository schoolReadOnlyRepository,
       ISchoolUpdateOnlyRepository schoolUpdateOnlyRepository) : IUpdateSchoolUseCase
    {
        public async Task Execute(long id, UpdateSchoolDto schoolDto)
        {
            await Validate(schoolDto);

            var school = await schoolReadOnlyRepository.GetById(id);
           
            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            schoolDto.Adapt(school);

            schoolUpdateOnlyRepository.Update(school);
            await unitOfWork.Commit();
        }
        private async Task Validate(UpdateSchoolDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
