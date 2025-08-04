using FluentValidation;
using InventarioEscolar.Application.Decorators;
using InventarioEscolar.Application.Services.AuthService;
using InventarioEscolar.Application.Services.AuthService.LoginAuth;
using InventarioEscolar.Application.Services.Email;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Application.Services.Mapster;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Update;
using InventarioEscolar.Application.UsesCases.CategoryCase.Delete;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetById;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
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
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventarioEscolar.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddUseServices(services);
            AddValidators(services);

            AutoMapping.RegisterMappings();

            ConfigureIdentityTokens(services);

            addMediatr(services);
            addScrutor(services);
        }
        private static void AddUseServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, BrevoEmailService>();
            services.AddScoped<ISignInManagerWrapper, SignInManagerWrapper>();

        }
        private static void AddValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtension).Assembly);
        }
        private static void ConfigureIdentityTokens(IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(30);
            });
        }
        private static void addMediatr(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionExtension).Assembly);
                cfg.LicenseKey = "Gere sua licença e aplique aqui";
            });
        }
        private static void addScrutor(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<RegisterAssetCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingHandlerDecorator<,>));
        }
    }

}
