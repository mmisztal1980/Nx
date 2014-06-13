namespace Nx.Domain.IntegrationTests
{
    public static class Config
    {
        public const string RabbitMqBaseAddress = "rabbitmq://54.186.74.118";

        public static string CommandPublisherAddress { get { return string.Format("{0}/testCommandPublisher", RabbitMqBaseAddress); } }

        public static string CommandHandlerAddress { get { return string.Format("{0}/testCommandHandler", RabbitMqBaseAddress); } }
    }
}