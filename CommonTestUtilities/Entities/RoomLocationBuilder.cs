using Bogus;
using InventarioEscolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Entities
{
    public static class RoomLocationBuilder
    {
        public static Faker<RoomLocation> CreateFake()
        {
            return new Faker<RoomLocation>("pt_BR")
                .RuleFor(r => r.Id, f => f.Random.Long(1, 10000))
                .RuleFor(r => r.CreatedOn, f => f.Date.Past())
                .RuleFor(r => r.Active, f => true)
                .RuleFor(r => r.Name, f => f.Commerce.Department())
                .RuleFor(r => r.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.Building, f => f.Address.BuildingNumber())
                .RuleFor(r => r.School, f => SchoolBuilder.Build())
                .RuleFor(r => r.SchoolId, (f, rl) => rl.School.Id)
                .RuleFor(r => r.Assets, _ => new List<Asset>());
        }
        public static RoomLocation Build() => CreateFake().Generate();
        public static List<RoomLocation> BuildList(int quantity) => CreateFake().Generate(quantity);

    }
}