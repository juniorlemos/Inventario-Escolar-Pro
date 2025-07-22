using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public class UpdateRoomLocationCommandHandler : IRequestHandler<UpdateRoomLocationCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateRoomLocationDto> _validator;
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;
        private readonly IRoomLocationUpdateOnlyRepository _roomLocationUpdateOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateRoomLocationCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<UpdateRoomLocationDto> validator,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
            _roomLocationUpdateOnlyRepository = roomLocationUpdateOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateRoomLocationCommand request, CancellationToken cancellationToken)
        {
            await Validate(request.RoomLocationDto);

            var roomLocation = await _roomLocationReadOnlyRepository.GetById(request.Id);

            if (roomLocation is null)
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            if (roomLocation.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);

            request.RoomLocationDto.Adapt(roomLocation);

            _roomLocationUpdateOnlyRepository.Update(roomLocation);
            await _unitOfWork.Commit();

            return Unit.Value;
        }

        private async Task Validate(UpdateRoomLocationDto dto)
        {
            var result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
