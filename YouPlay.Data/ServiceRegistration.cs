using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;
using YouPlay.Data.Repositories;

namespace YouPlay.Data
{
    public static class ServiceRegistration
    {
        public static void AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameImageRepository, GameImageRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();


            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
        }
    }
}
