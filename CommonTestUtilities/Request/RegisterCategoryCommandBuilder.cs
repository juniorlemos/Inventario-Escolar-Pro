using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;

namespace CommonTestUtilities.Request
{
    public static class RegisterCategoryCommandBuilder
    {
        public static RegisterCategoryCommand Build()
        {
            var categoryDto = CategoryDtoBuilder.Build();
            return new RegisterCategoryCommand(categoryDto);
        }
    }
}