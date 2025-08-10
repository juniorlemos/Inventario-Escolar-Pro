using Bogus;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Enum;

namespace CommonTestUtilities.Dtos
{
    public static class AssetDtoBuilder
    {
        public static AssetDto Build()
        {
            var faker = new Faker<AssetDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.PatrimonyCode, f => f.Random.Long(100000, 999999))
                .RuleFor(x => x.AcquisitionValue, f => f.Finance.Amount(100, 10000))
                .RuleFor(x => x.ConservationState, f => f.Random.Enum<ConservationState>())
                .RuleFor(x => x.SerieNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.CategoryId, f => f.Random.Long(1, 10))
                .RuleFor(x => x.RoomLocationId, f => f.Random.Long(1, 5))
                .RuleFor(x => x.SchoolId, f => f.Random.Long(1, 3));

            return faker.Generate();
        }
    }
}