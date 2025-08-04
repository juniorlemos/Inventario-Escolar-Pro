using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
