using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.Mappers
{
        public static class RoomLocationMapper
        {
            public static RoomLocationDto ToDto(RoomLocation entity)
            {
                if (entity == null) return null!;

                return new RoomLocationDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Building = entity.Building,
                    Assets = entity.Assets?
                        .Select(a => new AssetDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            PatrimonyCode = a.PatrimonyCode
                        }).ToList() ?? []
                };
            }

            public static RoomLocation ToEntity(RoomLocationDto dto)
            {
                if (dto == null) return null!;

                return new RoomLocation
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Building = dto.Building,
                    Assets = dto.Assets?
                        .Select(d => new Asset
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Description = d.Description,
                            PatrimonyCode = d.PatrimonyCode
                        }).ToList() ?? []
                };
            }
        }
 }

