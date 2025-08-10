using Bogus;
using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;

namespace CommonTestUtilities.Request
{
    public static class UpdateSchoolCommandBuilder
    {
        public static UpdateSchoolCommand Build()
        {
            var faker = new Faker();

            var id = faker.Random.Long(1, 1000);
            var dto = UpdateSchoolDtoBuilder.Build();

            return new UpdateSchoolCommand(id, dto);
        }
    }
}