
namespace InventarioEscolar.Application.UsesCases.Asset.GetAll
{
    public interface IGetAllAssetUseCase
    {
        Task<IList<Domain.Entities.Asset>> Execute();
    }
}
