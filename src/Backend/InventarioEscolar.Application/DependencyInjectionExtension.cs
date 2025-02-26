using InventarioEscolar.Application.Services.AutoMapper;
using InventarioEscolar.Application.UsesCases.Asset.Delete;
using InventarioEscolar.Application.UsesCases.Asset.GetAll;
using InventarioEscolar.Application.UsesCases.Asset.GetById;
using InventarioEscolar.Application.UsesCases.Asset.Register;
using InventarioEscolar.Application.UsesCases.Asset.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventarioEscolar.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }
        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IGetAllAssetUseCase, GetAllAssetUseCase>();
            services.AddScoped<IGetByIdAssetUseCase, GetByIdAssetUseCase>();
            services.AddScoped<IRegisterAssetUseCase, RegisterAssetUseCase>();
            services.AddScoped<IUpdateAssetUseCase, UpdateAssetUseCase>();
            services.AddScoped<IDeleteAssetUseCase, DeleteAssetUseCase>();
        }
        private static void AddAutoMapper(IServiceCollection services)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Console.WriteLine($"Assembly: {assembly.FullName} - Location: {assembly.Location}");
            }
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapping>();
            }, AppDomain.CurrentDomain.GetAssemblies()); 
        }
    }

}
