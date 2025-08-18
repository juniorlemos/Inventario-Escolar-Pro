using FluentValidation;
using InventarioEscolar.Application.Decorators;
using InventarioEscolar.Application.Services.Email;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Application.Services.Mapster;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AuthService.LoginAuth;
using InventarioEscolar.Domain.Interfaces;
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

            AddMediatr(services);
            AddScrutor(services);
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
        private static void AddMediatr(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionExtension).Assembly);
                cfg.LicenseKey = "Gere sua licença e aplique aqui";
            });
        }
        private static void AddScrutor(IServiceCollection services)
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
