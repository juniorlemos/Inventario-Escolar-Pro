using Bogus;
using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Request
{
    public static class UpdateRoomLocationCommandBuilder
    {
        public static UpdateRoomLocationCommand Build()
        {
            var faker = new Faker();

            var id = faker.Random.Long(1, 1000);
            var dto = UpdateRoomLocationDtoBuilder.Build();

            return new UpdateRoomLocationCommand(id, dto);
        }
    }
}
