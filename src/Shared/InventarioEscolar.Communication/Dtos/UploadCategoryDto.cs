using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public record UploadCategoryDto :ICategoryBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
