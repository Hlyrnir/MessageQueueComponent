using MessageQueueComponent.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueueComponent
{
    public static class ServiceCollectionExtension
    {
        public static MessageQueueServiceCollectionBuilder AddMessageQueue(this IServiceCollection cltService)
        {
            cltService.AddScoped<IMessageQueueProvider, MessageQueueProvider>();
            cltService.AddScoped<MessageStyle>();

            return new MessageQueueServiceCollectionBuilder(cltService);
        }
    }
}