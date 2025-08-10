using FluentValidation;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public class UpdateSchoolCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<UpdateSchoolDto> validator,
        ISchoolReadOnlyRepository schoolReadOnlyRepository,
        ISchoolUpdateOnlyRepository schoolUpdateOnlyRepository) : IRequestHandler<UpdateSchoolCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.SchoolDto);

            var school = await schoolReadOnlyRepository.GetById(request.Id);

            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            request.SchoolDto.Adapt(school);

            schoolUpdateOnlyRepository.Update(school);
            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}