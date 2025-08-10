using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;

namespace CommonTestUtilities.Request
{
    public static class RegisterSchoolCommandBuilder
    {
        public static RegisterSchoolCommand Build()
        {
            var schoolDto = SchoolDtoBuilder.Build();
            return new RegisterSchoolCommand(schoolDto);
        }
    }
}