using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MapsterMapper;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public class RegisterSchoolUseCase(
       IUnitOfWork unitOfWork,
       IValidator<SchoolDto> validator,
       ISchoolReadOnlyRepository schoolReadOnlyRepository,
       ISchoolWriteOnlyRepository schoolWriteOnlyRepository) : IRegisterSchoolUseCase
    {
        public async Task<SchoolDto> Execute(SchoolDto schoolDto)
        {
            await Validate(schoolDto);

            var schoolDuplicate = await schoolReadOnlyRepository.GetDuplicateSchool(schoolDto.Name,schoolDto.Inep,schoolDto.Address);

            if (schoolDuplicate != null)
            {
                if (string.Equals(schoolDuplicate.Name, schoolDto.Name, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);

                if (schoolDuplicate.Inep == schoolDto.Inep)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);

                if (schoolDuplicate.Address == schoolDto.Address)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
            }

            var school = schoolDto.Adapt<School>();

            await schoolWriteOnlyRepository.Insert(school);
            await unitOfWork.Commit();

            return school.Adapt<SchoolDto>();
        }
           private async Task Validate(SchoolDto dto)
          {
           var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
           }
    }
 }

