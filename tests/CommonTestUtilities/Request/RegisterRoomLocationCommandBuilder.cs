using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;

namespace CommonTestUtilities.Request
{
    public static class RegisterRoomLocationCommandBuilder
    {
        public static RegisterRoomLocationCommand Build()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            return new RegisterRoomLocationCommand(roomLocationDto);
        }
    }
}