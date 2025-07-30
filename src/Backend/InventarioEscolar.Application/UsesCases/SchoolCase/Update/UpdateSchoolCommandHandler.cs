using FluentValidation;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
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

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateSchoolDto> _validator;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;
        private readonly ISchoolUpdateOnlyRepository _schoolUpdateOnlyRepository;

        public UpdateSchoolCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<UpdateSchoolDto> validator,
            ISchoolReadOnlyRepository schoolReadOnlyRepository,
            ISchoolUpdateOnlyRepository schoolUpdateOnlyRepository)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
            _schoolUpdateOnlyRepository = schoolUpdateOnlyRepository;
        }

        public async Task<Unit> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowIfInvalid(request.SchoolDto);

            var school = await _schoolReadOnlyRepository.GetById(request.Id);

            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            request.SchoolDto.Adapt(school);

            _schoolUpdateOnlyRepository.Update(school);
            await _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}

