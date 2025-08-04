using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Request
{
    public static class RegisterAssetCommandBuilder
    {
        public static RegisterAssetCommand Build()
        {
            var assetDto = AssetDtoBuilder.Build();
            return new RegisterAssetCommand(assetDto);
        }
    }
}
