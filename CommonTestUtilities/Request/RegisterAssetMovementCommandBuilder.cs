using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Request
{
    public static class RegisterAssetMovementCommandBuilder
    {
        public static RegisterAssetMovementCommand Build()
        {
            var assetMovementDto = AssetMovementDtoBuilder.Build();
            return new RegisterAssetMovementCommand(assetMovementDto);
        }
    }
}
