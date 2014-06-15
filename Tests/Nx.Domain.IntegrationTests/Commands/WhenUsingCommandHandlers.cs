using MassTransit;
using NCrunch.Framework;
using NUnit.Framework;
using Nx.Domain.ServiceBus;
using System;

namespace Nx.Domain.IntegrationTests.Commands
{
    [TestFixture]
    public class WhenUsingCommandHandlers
    {
        [Test]
        [ExclusivelyUses("RabbitMQ")]
        public void ShouldReceiveSingleCommand()
        {
            using (var serviceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(Config.CommandPublisherAddress);
                bus.UseRabbitMq();
            }))
            using (var handler = new TestCommandHandler("TestCommandHandler", 1))
            {
                serviceBus.Publish(new TestCommand());

                handler.CommandReceived.WaitUntilCompleted(5.Seconds());
                Assert.IsTrue(handler.CommandReceived.IsCompleted);
            }
        }

        [Test]
        [ExclusivelyUses("RabbitMQ")]
        public void ShouldReceiveMultipleCommands()
        {
            using (var serviceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(Config.CommandPublisherAddress);
                bus.UseRabbitMq();
            }))
            using (var handler = new TestCommandHandler("TestCommandHandler", 2))
            {
                serviceBus.Publish(new TestCommand());
                serviceBus.Publish(new TestCommand());

                handler.CommandReceived.WaitUntilCompleted(5.Seconds());

                Assert.IsTrue(handler.CommandReceived.IsCompleted);
            }
        }

        [Test]
        [ExclusivelyUses("RabbitMQ")]
        public void AllNonCompetingHandlersShouldReceiveComands()
        {
            using (var serviceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(Config.CommandPublisherAddress);

                bus.UseRabbitMq();
            }))
            using (var handler1 = new TestCommandHandler(new Uri(Config.CommandHandlerAddress1), "TestCommandHandler1", 1))
            using (var handler2 = new TestCommandHandler(new Uri(Config.CommandHandlerAddress2), "TestCommandHandler2", 1))
            {
                serviceBus.Publish(new TestCommand());

                handler1.CommandReceived.WaitUntilCompleted(10.Seconds());
                handler2.CommandReceived.WaitUntilCompleted(10.Seconds());
                Assert.IsTrue(handler1.CommandReceived.IsCompleted && handler2.CommandReceived.IsCompleted);
            }
        }

        [Test]
        [ExclusivelyUses("RabbitMQ")]
        public void OnlyOneCompetingHandlerShouldReceiveTheCommand()
        {
            using (var serviceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(Config.CommandPublisherAddress);

                bus.UseRabbitMq();
            }))
            using (var handler1 = new TestCommandHandler(new Uri(Config.CommandHandlerAddress), "TestCommandHandler1", 1))
            using (var handler2 = new TestCommandHandler(new Uri(Config.CommandHandlerAddress), "TestCommandHandler2", 1))
            {
                serviceBus.Publish(new TestCommand());

                handler1.CommandReceived.WaitUntilCompleted(5.Seconds());

                Assert.IsTrue(
                    (handler1.CommandReceived.IsCompleted || handler2.CommandReceived.IsCompleted) &&
                    !(handler1.CommandReceived.IsCompleted && handler2.CommandReceived.IsCompleted));
            }
        }

        [Test]
        [ExclusivelyUses("RabbitMQ")]
        public void OnlyReceiverShouldReceiveCommands()
        {
            const string receiverGuid = "af486b11-0fad-4f5a-98d3-7308ec925708";
            var receiverId = new Guid(receiverGuid);

            using (var serviceBus = ServiceBusFactory.New(bus =>
            {
                bus.ReceiveFrom(Config.CommandPublisherAddress);

                bus.UseRabbitMq();
            }))
            using (var handler1 = new TestCommandHandler(new Uri(Config.CommandHandlerAddress1), receiverId, "TestCommandHandler1", 5))
            using (var handler2 = new TestCommandHandler(new Uri(Config.CommandHandlerAddress2), "TestCommandHandler2", 1))
            {
                serviceBus.SendTo(new TestCommand(), receiverId);
                serviceBus.SendTo(new TestCommand(), receiverId);
                serviceBus.SendTo(new TestCommand(), receiverId);
                serviceBus.SendTo(new TestCommand(), receiverId);
                serviceBus.SendTo(new TestCommand(), receiverId);

                handler1.CommandReceived.WaitUntilCompleted(7.Seconds());

                Assert.IsTrue(handler1.CommandReceived.IsCompleted && !handler2.CommandReceived.IsCompleted);
            }
        }
    }
}