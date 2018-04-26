using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TelegramAggregator.Data.Repositories;
using TelegramAggregator.Services;
using TelegramAggregator.Services.CommandsHandler;
using TelegramAggregator.Services.MessageTransferService;
using TelegramAggregator.Services.NotificationsService;

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
            services.AddSingleton<INotificationsService, NotificationsService>();
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IMessageTransferService, MessageTransferService>();
            services.AddScoped<ICommandsHandler, CommandsHandler>();
            services.AddScoped<IBotUserRepository, BotUserRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info
                {
                    Version = "v0",
                    Title = "TelegramAggregator API",
                    Description = "Telegram Aggregator Web API",
                });
                
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "TelegramAggregator.xml"); 
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "TelegramAggregator V0");
            });
        }
    }
}