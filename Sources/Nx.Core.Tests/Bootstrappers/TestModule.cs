using Nx.Modules;

namespace Nx.Core.Tests.Bootstrappers
{
    public interface ILoadable
    {
        bool IsLoaded { get; }
    }

    public class TestModule : Module, ILoadable
    {
        public TestModule()
        {
            IsLoaded = false;
        }

        public bool IsLoaded { get; private set; }

        public override string Name
        {
            get { return "TestModule"; }
        }

        public override void OnDisposing()
        {
        }

        public override void OnLoading()
        {
            IsLoaded = true;
        }
    }
}