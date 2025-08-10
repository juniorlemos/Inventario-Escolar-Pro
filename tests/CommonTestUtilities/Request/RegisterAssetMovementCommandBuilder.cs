using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;

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