using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MessagesTransferApi.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using MessagesTransferApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace MessagesTransferApi
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
            services.AddTokenGeneratorService();
            services.AddAggregatorSenderService();
            
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MessagesTransferData"));
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info
                {
                    Version = "v0",
                    Title = "Message Transfer API",
                    Description = "API для основного узла MTA"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v0/swagger.json", "Message Transfer API v0"); });
        }
    }
}
