using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public record UpdateCategoryDto :ICategoryBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}