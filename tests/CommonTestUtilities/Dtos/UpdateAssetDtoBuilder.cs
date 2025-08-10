using Bogus;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Enum;

namespace CommonTestUtilities.Dtos
{
   public static class UpdateAssetDtoBuilder
    {
        public static UpdateAssetDto Build()
        {
            var faker = new Faker<UpdateAssetDto>("pt_BR")
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.PatrimonyCode, f => f.Random.Long(100000, 999999))
                .RuleFor(x => x.AcquisitionValue, f => f.Finance.Amount(100, 10000))
                .RuleFor(x => x.ConservationState, f => f.Random.Enum<ConservationState>())
                .RuleFor(x => x.SerieNumber, f => f.Random.AlphaNumeric(10));

            return faker.Generate();
        }
    }
}