using ChatApp.Application.Commands.SendMessage;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddHandlers();

            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<SendMessageCommand>());

            return services;
        }
}
