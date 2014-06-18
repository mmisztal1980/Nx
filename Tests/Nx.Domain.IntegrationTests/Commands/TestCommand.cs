using Nx.Commands;
using System;

namespace Nx.Domain.IntegrationTests.Commands
{
    public class TestCommand : Command
    {
        public TestCommand()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public DateTime CreatedAt { get; private set; }
    }
}