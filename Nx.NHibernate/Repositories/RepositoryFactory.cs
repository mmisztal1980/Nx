using NHibernate;
using Ninject;
using Ninject.Parameters;

namespace Nx.Repositories
{
    public class RepositoryFactory : Disposable
    {
        private IKernel _kernel;

        public RepositoryFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IRepository<TEntity> Create<TEntity>(ISession session)
            where TEntity : class, IEntity
        {
            return _kernel.Get<IRepository<TEntity>>(new ConstructorArgument("session", session));
        }

        protected override void Dispose(bool disposing)
        {
            _kernel = null;
        }
    }
}