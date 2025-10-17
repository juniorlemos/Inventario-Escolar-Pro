using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToDto(Category entity)
        {
            if (entity == null) return null!;

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SchoolId = entity.SchoolId
            };
        }

        public static Category ToEntity(CategoryDto dto)
        {
            if (dto == null) return null!;

            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                SchoolId = dto.SchoolId
            };
        }
    }
}
