using FluentValidation;
using InventarioEscolar.Application.Services.Mapster;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.CategoryCase.Delete;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetById;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Application.UsesCases.SchoolCase.Delete;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetAll;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
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
            services.AddScoped<IGetByIdRoomLocationUseCase, GetByIdRoomLocationUseCase>();
            services.AddScoped<IGetAllRoomLocationUseCase, GetAllRoomLocationUseCase>();
            services.AddScoped<IUpdateRoomLocationUseCase, UpdateRoomLocationUseCase>();
            services.AddScoped<IDeleteRoomLocationUseCase, DeleteRoomLocationUseCase>();

            services.AddScoped<IRegisterSchoolUseCase, RegisterSchoolUseCase>();
            services.AddScoped<IGetByIdSchoolUseCase, GetByIdSchoolUseCase>();
            services.AddScoped<IGetAllSchoolUseCase, GetAllSchoolUseCase>();
            services.AddScoped<IUpdateSchoolUseCase, UpdateSchoolUseCase>();
            services.AddScoped<IDeleteSchoolUseCase, DeleteSchoolUseCase>();

            services.AddScoped<IRegisterCategoryUseCase, RegisterCategoryUseCase>();
            services.AddScoped<IGetByIdCategoryUseCase, GetByIdCategoryUseCase>();
            services.AddScoped<IGetAllCategoryUseCase, GetAllCategoryUseCase>();
            services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
            services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();

        }
        private static void AddValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtension).Assembly);
        }
        
    }

}
