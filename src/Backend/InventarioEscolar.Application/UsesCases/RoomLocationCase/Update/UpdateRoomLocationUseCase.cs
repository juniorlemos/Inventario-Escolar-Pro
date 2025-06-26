using FluentValidation;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Exceptions.ExceptionsBase;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Api.Extension;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public class UpdateRoomLocationUseCase(
       IUnitOfWork unitOfWork,
       IValidator<UpdateRoomLocationDto> validator,
       IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
       IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository) : IUpdateRoomLocationUseCase
    {
        public async Task Execute(long id, UpdateRoomLocationDto roomLocationDto)
        {
            await Validate(roomLocationDto);

            var roomLocation = await roomLocationReadOnlyRepository.GetById(id);

            if (roomLocation is null)
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            roomLocationDto.Adapt(roomLocation);

            roomLocationUpdateOnlyRepository.Update(roomLocation);
            await unitOfWork.Commit();

        }
        private async Task Validate(UpdateRoomLocationDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
