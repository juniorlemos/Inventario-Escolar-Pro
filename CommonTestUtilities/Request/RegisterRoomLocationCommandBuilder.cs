using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
