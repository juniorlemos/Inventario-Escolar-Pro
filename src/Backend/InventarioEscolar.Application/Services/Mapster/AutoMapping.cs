using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Entities;
using Mapster;

namespace InventarioEscolar.Application.Services.Mapster
{
    public static class AutoMapping
    {
        public static void RegisterMappings()
        {
            RoomLocationMappings();
            SchoolMappings();
            CategoryMappings();
            AssetMappings();

            TypeAdapterConfig.GlobalSettings.Compile();
        }

        public static void RoomLocationMappings() 
        {
            TypeAdapterConfig<RoomLocationDto,RoomLocation>.NewConfig();
            TypeAdapterConfig<RoomLocation, RoomLocationDto>.NewConfig();
            TypeAdapterConfig<RequestRegisterRoomLocationJson, RoomLocationDto>.NewConfig();
            TypeAdapterConfig<RoomLocationDto, ResponseRegisterRoomLocationJson>.NewConfig();
        }

        public static void SchoolMappings()
        {
            TypeAdapterConfig<SchoolDto, School>.NewConfig();
            TypeAdapterConfig<School, SchoolDto>.NewConfig();
            TypeAdapterConfig<RequestRegisterSchoolJson, SchoolDto>.NewConfig();
            TypeAdapterConfig<SchoolDto, ResponseRegisterSchoolJson>.NewConfig();
        }
        public static void CategoryMappings()
        {
            TypeAdapterConfig<CategoryDto, Category>.NewConfig();
            TypeAdapterConfig<Category, CategoryDto>.NewConfig();
            TypeAdapterConfig<RequestRegisterCategoryJson, CategoryDto>.NewConfig();
            TypeAdapterConfig<CategoryDto, ResponseRegisterCategoryJson>.NewConfig();
        }

        public static void AssetMappings()
        {
            TypeAdapterConfig<AssetDto, Asset>.NewConfig();
            TypeAdapterConfig<Asset, AssetDto>.NewConfig();
            TypeAdapterConfig<RequestRegisterAssetJson, AssetDto>.NewConfig();
            TypeAdapterConfig<AssetDto, ResponseRegisterAssetJson>.NewConfig();
        }
    }
}
