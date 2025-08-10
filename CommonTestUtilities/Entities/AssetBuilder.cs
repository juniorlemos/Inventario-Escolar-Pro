using Bogus;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public static class AssetBuilder
    {
            public static Faker<Asset> CreateFake()
            {
                return new Faker<Asset>("pt_BR")
                    .RuleFor(a => a.Id, f => f.Random.Long(1, 10000))
                    .RuleFor(a => a.CreatedOn, f => f.Date.Past())
                    .RuleFor(a => a.Active, f => true)
                    .RuleFor(a => a.Name, f => f.Commerce.ProductName())
                    .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                    .RuleFor(a => a.PatrimonyCode, f => f.Random.Long(100000, 999999))
                    .RuleFor(a => a.AcquisitionValue, f => f.Finance.Amount(1000, 10000))
                    .RuleFor(a => a.ConservationState, f => f.PickRandom<ConservationState>())
                    .RuleFor(a => a.SerieNumber, f => f.Random.AlphaNumeric(10))
                    .RuleFor(a => a.School, f => SchoolBuilder.Build())
                    .RuleFor(a => a.SchoolId, (f, a) => a.School.Id)
                    .RuleFor(a => a.Category, f => CategoryBuilder.Build())
                    .RuleFor(a => a.CategoryId, (f, a) => a.Category.Id)
                    .RuleFor(a => a.RoomLocation, f => f.Random.Bool() ? RoomLocationBuilder.Build() : null)
                    .RuleFor(a => a.RoomLocationId, (f, a) => a.RoomLocation?.Id);
            }
            public static Asset Build() => CreateFake().Generate();
            public static List<Asset> BuildList(int quantity) => CreateFake().Generate(quantity);
    }
}