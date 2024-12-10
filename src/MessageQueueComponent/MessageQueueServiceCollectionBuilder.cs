using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MessageQueueComponent
{
    public class MessageQueueServiceCollectionBuilder
    {
        private readonly IServiceCollection cltService;

        public virtual IServiceCollection Services { get => cltService; }

        public MessageQueueServiceCollectionBuilder(IServiceCollection cltService)
        {
            this.cltService = cltService;
        }

        public MessageQueueServiceCollectionBuilder ConfigureMessageStyle(Action<MessageStyle> actnMessageStyle)
        {
            cltService.Replace(new ServiceDescriptor(
                typeof(MessageStyle),
                prvService =>
                {
                    MessageStyle msgStyle = new MessageStyle();

                    actnMessageStyle(msgStyle);

                    return msgStyle;
                },
                ServiceLifetime.Scoped));

            return new MessageQueueServiceCollectionBuilder(cltService);
        }
    }
}