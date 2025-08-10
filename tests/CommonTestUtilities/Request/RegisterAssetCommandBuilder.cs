using CommonTestUtilities.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;

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