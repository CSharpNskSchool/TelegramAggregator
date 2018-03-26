using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.VkMessenger.Bot.Models;
using Telegram.VkMessenger.Bot.Services;

namespace Telegram.VkMessenger.Bot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("UsersList"));
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddSingleton<IBotService, BotService>();

            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}