namespace Nx.Domain.IntegrationTests
{
    public static class Config
    {
        public const string RabbitMqBaseAddress = "rabbitmq://54.186.74.118";

        public static string CommandPublisherAddress { get { return string.Format("{0}/testCommandPublisher", RabbitMqBaseAddress); } }

        public static string CommandHandlerAddress { get { return string.Format("{0}/testCommandHandler", RabbitMqBaseAddress); } }

        public static string CommandHandlerAddress1 { get { return string.Format("{0}/testCommandHandler1", RabbitMqBaseAddress); } }

        public static string CommandHandlerAddress2 { get { return string.Format("{0}/testCommandHandler2", RabbitMqBaseAddress); } }

        public static string CommandHandlerAddress3 { get { return string.Format("{0}/testCommandHandler3", RabbitMqBaseAddress); } }
    }
}