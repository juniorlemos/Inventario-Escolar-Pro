using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
