using Bogus;
using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;

namespace CommonTestUtilities.Request
{
    public static class UpdateAssetCommandBuilder
    {
        public static UpdateAssetCommand Build()
        {
            var faker = new Faker();

            var id = faker.Random.Long(1, 1000);
            var dto = UpdateAssetDtoBuilder.Build();

            return new UpdateAssetCommand(id, dto);
        }
    }
}