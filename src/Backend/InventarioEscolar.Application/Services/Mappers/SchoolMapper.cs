using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using System.Linq;

namespace InventarioEscolar.Application.Services.Mappers
{
    public static class SchoolMapper
    {
        public static SchoolDto ToDto(School entity)
        {
            if (entity == null) return null!;

            return new SchoolDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Inep = entity.Inep,
                Address = entity.Address,
                City = entity.City,
                Email = entity.Users?.FirstOrDefault()?.Email,
                RoomLocations = entity.RoomLocations?
                                    .Select(RoomLocationMapper.ToDto)
                                    .ToList()
                                ?? [],
                Assets = entity.Assets?
                               .Select(AssetMapper.ToDto)
                               .ToList()
                            ?? []
            };
        }

        public static School ToEntity(SchoolDto dto)
        {
            if (dto == null) return null!;

            return new School
            {
                Id = dto.Id,
                Name = dto.Name,
                Inep = dto.Inep,
                Address = dto.Address,
                City = dto.City,
                RoomLocations = dto.RoomLocations?
                                    .Select(RoomLocationMapper.ToEntity)
                                    .ToList()
                                ?? [],
                Assets = dto.Assets?
                               .Select(AssetMapper.ToEntity)
                               .ToList()
                            ?? []
            };
        }
    }
}
