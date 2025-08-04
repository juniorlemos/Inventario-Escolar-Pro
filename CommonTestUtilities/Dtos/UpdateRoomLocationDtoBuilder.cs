using Bogus;
using InventarioEscolar.Communication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Dtos
{
    public static class UpdateRoomLocationDtoBuilder
    {
        public static UpdateRoomLocationDto Build()
        {
            var faker = new Faker<UpdateRoomLocationDto>("pt_BR")
                .RuleFor(x => x.Name, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence(5))
                .RuleFor(x => x.Building, f => f.Address.BuildingNumber());

            return faker.Generate();
        }
    }
}
