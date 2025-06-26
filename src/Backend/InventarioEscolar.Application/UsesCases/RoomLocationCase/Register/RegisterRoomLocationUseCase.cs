using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public class RegisterRoomLocationUseCase(
       IUnitOfWork unitOfWork,
       IValidator<RoomLocationDto> validator,
       IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
       IRoomLocationWriteOnlyRepository roomLocationWriteOnlyRepository) : IRegisterRoomLocationUseCase
    {
        public async Task<RoomLocationDto> Execute(RoomLocationDto roomLocationDto)
        {
            var roomLocationAlreadyExists = await roomLocationReadOnlyRepository.ExistRoomLocationName(roomLocationDto.Name);
          
            if (roomLocationAlreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.ROOMLOCATION_NAME_ALREADY_EXISTS);
           
            await Validate(roomLocationDto);

            var roomLocation = roomLocationDto.Adapt<RoomLocation>();

            await roomLocationWriteOnlyRepository.Insert(roomLocation);
            await unitOfWork.Commit();

            return roomLocation.Adapt<RoomLocationDto>();
        }
        private async Task Validate(RoomLocationDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
