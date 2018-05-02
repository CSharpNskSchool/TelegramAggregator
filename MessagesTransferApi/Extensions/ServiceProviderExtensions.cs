using MessagesTransferApi.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace MessagesTransferApi.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddTokenGeneratorService(this IServiceCollection services)
        {
            services.AddTransient<ITokenGeneratorService, GuidTokenGeneratorService>();
        }

        public static void AddAggregatorSenderService(this IServiceCollection services)
        {
            services.AddTransient<IAggregatorSenderService, DirectAggregatorSenderService>();
        }
    }
}
