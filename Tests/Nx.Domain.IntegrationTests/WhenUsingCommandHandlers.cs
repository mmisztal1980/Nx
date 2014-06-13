using MassTransit;
using NUnit.Framework;
using Nx.Domain.Commands;
using System;

namespace Nx.Domain.IntegrationTests
{
    [TestFixture]
    public class WhenUsingCommandHandlers
    {
        [Test]
        public void ShouldReceiveSingleCommand()
        {
            using (var serviceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(Config.CommandPublisherAddress);
                bus.UseRabbitMq();
            }))
            using (var handler = new TestCommandHandler())
            {
                serviceBus.Publish(new TestCommand());

                handler.CommandReceived.WaitUntilCompleted(5.Seconds());

                Assert.IsTrue(handler.CommandReceived.IsCompleted);
            }
        }
    }

    public class TestCommand : Command
    {
        public TestCommand()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public DateTime CreatedAt { get; private set; }
    }

    public class TestCommandHandler : ReactiveCommandHandler<TestCommand>
    {
        public TestCommandHandler()
            : base(new Uri(Config.CommandHandlerAddress))
        {
            CommandReceived = new Future<bool>();
        }

        public override void HandleCommand(TestCommand command)
        {
            Console.WriteLine("[{0}] Command {1} received. Roundtrip time : {2} [ms]", Key, command.Id, (DateTime.UtcNow - command.CreatedAt).TotalMilliseconds);
            CommandReceived.Complete(true);
        }

        public override string Key
        {
            get { return "TestCommandHandler"; }
        }

        public Future<bool> CommandReceived { get; private set; }
    }
}