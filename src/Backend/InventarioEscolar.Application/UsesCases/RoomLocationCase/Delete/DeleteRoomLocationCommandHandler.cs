using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete
{
    public class DeleteRoomLocationCommandHandler : IRequestHandler<DeleteRoomLocationCommand, Unit>
    {
        private readonly IRoomLocationDeleteOnlyRepository _roomLocationDeleteOnlyRepository;
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteRoomLocationCommandHandler(
            IRoomLocationDeleteOnlyRepository roomLocationDeleteOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _roomLocationDeleteOnlyRepository = roomLocationDeleteOnlyRepository;
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteRoomLocationCommand request, CancellationToken cancellationToken)
        {
            var roomLocation = await _roomLocationReadOnlyRepository.GetById(request.RoomLocationId)
                ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            if (roomLocation.Assets.Any())
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_HAS_ASSETS);

            if (roomLocation.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);

            await _roomLocationDeleteOnlyRepository.Delete(roomLocation.Id);
            await _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}
