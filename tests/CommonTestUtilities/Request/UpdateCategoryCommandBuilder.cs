using Bogus;
using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;

namespace CommonTestUtilities.Request
{
    public static class UpdateCategoryCommandBuilder
    {
        public static UpdateCategoryCommand Build()
        {
            var faker = new Faker();

            var id = faker.Random.Long(1, 1000);
            var dto = UpdateCategoryDtoBuilder.Build();

            return new UpdateCategoryCommand(id, dto);
        }
    }
}