using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramAggregator.Data.Repositories;
using TelegramAggregator.Services;
using TelegramAggregator.Services.CommandsHandler;
using TelegramAggregator.Services.MessagesNotify;
using TelegramAggregator.Services.MessagesTrasfer;

namespace TelegramAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<IMessageNotify, MessageNotify>();
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IMessageTransfer, VkNativeMessageTransfer>();
            services.AddScoped<ICommandsHandler, CommandsHandler>();
            services.AddScoped<IBotUserRepository, BotUserRepository>();

            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}