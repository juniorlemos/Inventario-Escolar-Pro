using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.Mappers
{
    public static class AssetMapper
    {
        public static AssetDto ToDto(Asset entity)
        {
            if (entity == null) return null!;

            return new AssetDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                PatrimonyCode = entity.PatrimonyCode,
                AcquisitionValue = entity.AcquisitionValue,
                ConservationState = entity.ConservationState,
                SerieNumber = entity.SerieNumber,
                CategoryId = entity.CategoryId,
                Category = entity.Category != null ? CategoryMapper.ToDto(entity.Category) : null!,
                RoomLocationId = entity.RoomLocationId,
                RoomLocation = entity.RoomLocation != null ? RoomLocationMapper.ToDto(entity.RoomLocation) : null,
                SchoolId = entity.SchoolId
            };
        }

        public static Asset ToEntity(AssetDto dto)
        {
            if (dto == null) return null!;

            return new Asset
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                PatrimonyCode = dto.PatrimonyCode,
                AcquisitionValue = dto.AcquisitionValue,
                ConservationState = dto.ConservationState,
                SerieNumber = dto.SerieNumber,
                CategoryId = dto.CategoryId,
                Category = dto.Category != null ? CategoryMapper.ToEntity(dto.Category) : null!,
                RoomLocationId = dto.RoomLocationId,
                RoomLocation = dto.RoomLocation != null ? RoomLocationMapper.ToEntity(dto.RoomLocation) : null,
                SchoolId = dto.SchoolId
            };
        }
    }
}