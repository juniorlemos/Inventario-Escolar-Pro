using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
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

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public class RegisterRoomLocationCommandHandler : IRequestHandler<RegisterRoomLocationCommand, RoomLocationDto>
    {
        private readonly IRoomLocationWriteOnlyRepository _roomLocationWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RoomLocationDto> _validator;
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public RegisterRoomLocationCommandHandler(
            IRoomLocationWriteOnlyRepository roomLocationWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IValidator<RoomLocationDto> validator,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            ICurrentUserService currentUser)
        {
            _roomLocationWriteOnlyRepository = roomLocationWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<RoomLocationDto> Handle(RegisterRoomLocationCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowIfInvalid(request.RoomLocationDto);

            if (!_currentUser.IsAuthenticated)
            throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            var alreadyExists = await _roomLocationReadOnlyRepository.ExistRoomLocationName(request.RoomLocationDto.Name, _currentUser.SchoolId);

            if (alreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.ROOMLOCATION_NAME_ALREADY_EXISTS);

            var roomLocation = request.RoomLocationDto.Adapt<RoomLocation>();

            await _roomLocationWriteOnlyRepository.Insert(roomLocation);
            await _unitOfWork.Commit();

            return roomLocation.Adapt<RoomLocationDto>();
        }
    }
}