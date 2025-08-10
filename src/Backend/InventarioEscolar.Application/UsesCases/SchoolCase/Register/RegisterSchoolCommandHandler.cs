using FluentValidation;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public class RegisterSchoolCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<SchoolDto> validator,
        ISchoolReadOnlyRepository schoolReadOnlyRepository,
        ISchoolWriteOnlyRepository schoolWriteOnlyRepository) : IRequestHandler<RegisterSchoolCommand, SchoolDto>
    {
        public async Task<SchoolDto> Handle(RegisterSchoolCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.SchoolDto);

            var schoolDuplicate = await schoolReadOnlyRepository.GetDuplicateSchool(
                request.SchoolDto.Name,
                request.SchoolDto.Inep,
                request.SchoolDto.Address);

            if (schoolDuplicate != null)
            {
                if (string.Equals(schoolDuplicate.Name, request.SchoolDto.Name, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);

                if (schoolDuplicate.Inep == request.SchoolDto.Inep)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);

                if (schoolDuplicate.Address == request.SchoolDto.Address)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
            }

            var school = request.SchoolDto.Adapt<School>();

            await schoolWriteOnlyRepository.Insert(school);
            await unitOfWork.Commit();

            return school.Adapt<SchoolDto>();
        }
    }
}