using Microsoft.Extensions.DependencyInjection;
using YouPlay.Business.Services.Implementations;
using YouPlay.Business.Services.Interfaces;

namespace YouPlay.Business
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            //services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IGameService, GameService>();

        }
    }
}
