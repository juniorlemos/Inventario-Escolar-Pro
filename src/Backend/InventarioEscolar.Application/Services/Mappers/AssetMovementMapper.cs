using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.Mappers
{
    public static class AssetMovementMapper
    {
        public static AssetMovementDto ToDto(AssetMovement entity)
        {
            if (entity == null) return null!;

            return new AssetMovementDto
            {
                Id = entity.Id,
                AssetId = entity.AssetId,
                FromRoomId = entity.FromRoomId,
                ToRoomId = entity.ToRoomId,
                MovedAt = entity.MovedAt,
                Responsible = entity.Responsible,
                IsCanceled = entity.IsCanceled,
                CancelReason = entity.CancelReason,
                CanceledAt = entity.CanceledAt,

                Asset = entity.Asset != null
                    ? new AssetDto
                    {
                        Id = entity.Asset.Id,
                        Name = entity.Asset.Name,
                        Description = entity.Asset.Description,
                        PatrimonyCode = entity.Asset.PatrimonyCode
                    }
                    : null,

                FromRoom = entity.FromRoom != null
                    ? RoomLocationMapper.ToDto(entity.FromRoom)
                    : null,

                ToRoom = entity.ToRoom != null
                    ? RoomLocationMapper.ToDto(entity.ToRoom)
                    : null
            };
        }

        public static AssetMovement ToEntity(AssetMovementDto dto)
        {
            if (dto == null) return null!;

            return new AssetMovement
            {
                Id = dto.Id,
                AssetId = dto.AssetId,
                FromRoomId = dto.FromRoomId,
                ToRoomId = dto.ToRoomId,
                MovedAt = dto.MovedAt,
                Responsible = dto.Responsible,
                IsCanceled = dto.IsCanceled,
                CancelReason = dto.CancelReason,
                CanceledAt = dto.CanceledAt,

                Asset = dto.Asset != null
                    ? new Asset
                    {
                        Id = dto.Asset.Id,
                        Name = dto.Asset.Name,
                        Description = dto.Asset.Description,
                        PatrimonyCode = dto.Asset.PatrimonyCode
                    }
                    : null!,

                FromRoom = dto.FromRoom != null
                    ? RoomLocationMapper.ToEntity(dto.FromRoom)
                    : null!,

                ToRoom = dto.ToRoom != null
                    ? RoomLocationMapper.ToEntity(dto.ToRoom)
                    : null!
            };
        }
    }
}