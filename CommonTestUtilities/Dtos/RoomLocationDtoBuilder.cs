using Bogus;
using InventarioEscolar.Communication.Dtos;

namespace CommonTestUtilities.Dtos
{
    public static class RoomLocationDtoBuilder
    {
        public static RoomLocationDto Build(int assetCount = 2)
        {
            var faker = new Faker<RoomLocationDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Name, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence(5))
                .RuleFor(x => x.Building, f => f.Address.BuildingNumber())
                .RuleFor(x => x.SchoolId, f => f.Random.Long(1, 3))
                .RuleFor(x => x.Assets, f =>
                    new List<AssetDto>(
                        f.Make(assetCount, () => AssetDtoBuilder.Build())
                    )
                );

            return faker.Generate();
        }
    }
}