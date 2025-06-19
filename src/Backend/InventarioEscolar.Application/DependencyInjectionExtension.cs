using FluentValidation;
using InventarioEscolar.Application.Services.Mapster;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventarioEscolar.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddUseCases(services);
            AddValidators(services);
            AddMapsterConfiguration(services);

            AutoMapping.RegisterMappings();
        }
        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IGetAllAssetUseCase, GetAllAssetUseCase>();
            services.AddScoped<IGetByIdAssetUseCase, GetByIdAssetUseCase>();
            services.AddScoped<IRegisterAssetUseCase, RegisterAssetUseCase>();
            services.AddScoped<IUpdateAssetUseCase, UpdateAssetUseCase>();
            services.AddScoped<IDeleteAssetUseCase, DeleteAssetUseCase>();
            
            services.AddScoped<IRegisterRoomLocationUseCase, RegisterRoomLocationUseCase>();

            services.AddScoped<IRegisterSchoolUseCase, RegisterSchoolUseCase>();

            services.AddScoped<IRegisterCategoryUseCase, RegisterCategoryUseCase>();

        }
        private static void AddValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtension).Assembly);
        }
        private static void AddMapsterConfiguration(IServiceCollection services)
        {
            
            AutoMapping.RegisterMappings();
        }
    }

}
