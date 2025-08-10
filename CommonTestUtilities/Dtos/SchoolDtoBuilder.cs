using Bogus;
using InventarioEscolar.Communication.Dtos;

namespace CommonTestUtilities.Dtos
{
    public static class SchoolDtoBuilder
    {
        public static SchoolDto Build(int roomLocationCount = 2, int assetCount = 3)
        {
            var faker = new Faker<SchoolDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.Inep, f => f.Random.Bool(0.7f) ? f.Random.Replace("##########") : null)
                .RuleFor(x => x.Address, f => f.Address.FullAddress())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.RoomLocations, f =>
                    new List<RoomLocationDto>(
                        f.Make(roomLocationCount, () => RoomLocationDtoBuilder.Build())
                    )
                )
                .RuleFor(x => x.Assets, f =>
                    new List<AssetDto>(
                        f.Make(assetCount, () => AssetDtoBuilder.Build())
                    )
                );

            return faker.Generate();
        }
    }
}