using FluentValidation;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public class RegisterSchoolCommandHandler : IRequestHandler<RegisterSchoolCommand, SchoolDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SchoolDto> _validator;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;
        private readonly ISchoolWriteOnlyRepository _schoolWriteOnlyRepository;

        public RegisterSchoolCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<SchoolDto> validator,
            ISchoolReadOnlyRepository schoolReadOnlyRepository,
            ISchoolWriteOnlyRepository schoolWriteOnlyRepository)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
            _schoolWriteOnlyRepository = schoolWriteOnlyRepository;
        }

        public async Task<SchoolDto> Handle(RegisterSchoolCommand request, CancellationToken cancellationToken)
        {
            await Validate(request.SchoolDto);

            var schoolDuplicate = await _schoolReadOnlyRepository.GetDuplicateSchool(
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

            await _schoolWriteOnlyRepository.Insert(school);
            await _unitOfWork.Commit();

            return school.Adapt<SchoolDto>();
        }

        private async Task Validate(SchoolDto dto)
        {
            var result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
