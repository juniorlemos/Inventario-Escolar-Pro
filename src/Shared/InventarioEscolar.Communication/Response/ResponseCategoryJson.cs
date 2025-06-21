using InventarioEscolar.Application.Dtos;

namespace InventarioEscolar.Communication.Response
{
    public record ResponseCategoryJson
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<AssetDto> Assets { get; set; } = [];

    }
}
