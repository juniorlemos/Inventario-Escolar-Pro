using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;
using Mapster;

namespace InventarioEscolar.Application.Services.Mapster
{
    public static class AutoMapping
    {
        public static void RegisterMappings()
        {
            ConfigureRoomLocationMappings();
            ConfigureSchoolMappings();
            ConfigureCategoryMappings();
            ConfigureAssetMappings();
            ConfigureAssetMovementMappings();

            TypeAdapterConfig.GlobalSettings.Compile();
        }

        // 🔹 RoomLocation
        private static void ConfigureRoomLocationMappings()
        {
            TypeAdapterConfig<RoomLocation, RoomLocationDto>
                .NewConfig()
                .Map(dest => dest.Description, src => src.Description ?? string.Empty)
                .Map(dest => dest.Building, src => src.Building ?? string.Empty);
                
            TypeAdapterConfig<RoomLocationDto, RoomLocation>.NewConfig();
            TypeAdapterConfig<RequestRegisterRoomLocationJson, RoomLocationDto>.NewConfig();
        }

        // 🔹 School
        private static void ConfigureSchoolMappings()
        {
            TypeAdapterConfig<School, SchoolDto>.NewConfig()
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.Address, src => src.Address ?? string.Empty);

            TypeAdapterConfig<SchoolDto, School>.NewConfig();
            TypeAdapterConfig<UpdateSchoolDto, School>.NewConfig();
            TypeAdapterConfig<School, UpdateSchoolDto>.NewConfig();
            TypeAdapterConfig<RequestRegisterSchoolJson, SchoolDto>.NewConfig();
        }

        // 🔹 Category
        private static void ConfigureCategoryMappings()
        {
            TypeAdapterConfig<Category, CategoryDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.Description, src => src.Description ?? string.Empty)
                .Map(dest => dest.SchoolId, src => src.SchoolId);

            TypeAdapterConfig<CategoryDto, Category>.NewConfig()
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.Description, src => src.Description ?? string.Empty);

            TypeAdapterConfig<RequestRegisterCategoryJson, CategoryDto>.NewConfig();
        }

        // 🔹 Asset
        private static void ConfigureAssetMappings()
        {
            // DTO → Entidade
            TypeAdapterConfig<AssetDto, Asset>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.Description, src => src.Description ?? string.Empty)
                .Ignore(dest => dest.RoomLocation) // evita recursão
                .Map(dest => dest.RoomLocationId, src => src.RoomLocationId)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.SchoolId, src => src.SchoolId);

            // Entidade → DTO
            TypeAdapterConfig<Asset, AssetDto>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name ?? string.Empty)
                .Map(dest => dest.Description, src => src.Description ?? string.Empty)
                .Map(dest => dest.RoomLocationId, src => src.RoomLocationId)
                .Ignore(dest => dest.RoomLocation) // evita recursão
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.SchoolId, src => src.SchoolId)
                .Map(dest => dest.Category, src => src.Category != null
                    ? new CategoryDto
                    {
                        Id = src.Category.Id,
                        Name = src.Category.Name ?? string.Empty
                    }
                    : null);

            TypeAdapterConfig<RequestRegisterAssetJson, AssetDto>.NewConfig();
        }

        // 🔹 AssetMovement
        private static void ConfigureAssetMovementMappings()
        {
            TypeAdapterConfig<AssetMovementDto, AssetMovement>
                .NewConfig()
                .Ignore(dest => dest.Asset)
                .Ignore(dest => dest.FromRoom)
                .Ignore(dest => dest.ToRoom);

            TypeAdapterConfig<AssetMovement, AssetMovementDto>
                .NewConfig()
                .Map(dest => dest.Asset, src => src.Asset != null
                    ? new AssetDto
                    {
                        Id = src.Asset.Id,
                        Name = src.Asset.Name ?? string.Empty,
                        PatrimonyCode = src.Asset.PatrimonyCode
                    }
                    : null)
                .Map(dest => dest.FromRoom, src => src.FromRoom != null
                    ? new RoomLocationDto
                    {
                        Id = src.FromRoom.Id,
                        Name = src.FromRoom.Name ?? string.Empty
                    }
                    : null)
                .Map(dest => dest.ToRoom, src => src.ToRoom != null
                    ? new RoomLocationDto
                    {
                        Id = src.ToRoom.Id,
                        Name = src.ToRoom.Name ?? string.Empty
                    }
                    : null);

            TypeAdapterConfig<RequestRegisterAssetMovementJson, AssetMovementDto>.NewConfig();
        }
    }
}
