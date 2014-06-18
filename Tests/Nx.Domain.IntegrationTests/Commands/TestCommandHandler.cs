using MassTransit;
using Nx.Commands;
using System;
using System.Threading;

namespace Nx.Domain.IntegrationTests.Commands
{
    public class TestCommandHandler : ReactiveCommandHandler<TestCommand>
    {
        private readonly int _expectedMessages;

        private readonly string _key;

        private int _receivedMessages;

        public TestCommandHandler(Uri uri, Guid receiverId, string name, int expectedMessages)
            : base(uri, receiverId)
        {
            CommandReceived = new Future<bool>();
            _key = name;
            _expectedMessages = expectedMessages;
        }

        public TestCommandHandler(Uri uri, string name, int expectedMessages)
            : this(uri, Guid.NewGuid(), name, expectedMessages)
        {
        }

        public TestCommandHandler(string name, int expectedMessages)
            : this(new Uri(Config.CommandHandlerAddress), name, expectedMessages)
        {
        }

        public Future<bool> CommandReceived { get; private set; }

        public override string Key
        {
            get { return _key; }
        }

        public override void HandleCommand(TestCommand command)
        {
            Console.WriteLine("[{0}] Command {1} received in {2} [ms]", Key, command.Id, (DateTime.UtcNow - command.CreatedAt).TotalMilliseconds);

            Interlocked.Increment(ref _receivedMessages);

            if (_receivedMessages == _expectedMessages)
            {
                CommandReceived.Complete(true);
            }
        }

        protected override IServiceBus ConfigureServiceBus(Uri uri)
        {
            return ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(uri);
                bus.UseRabbitMq();
                bus.SetPurgeOnStartup(true);
            });
        }
    }
}