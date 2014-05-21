using Nx.Domain.Infrastructure.Queries;
using System.Linq;

namespace Nx.Domain.Infrastructure.Repositories
{
    public interface ISession
    {
        IQueryable<T> Linq<T>();

        T Get<T>(int id);

        void SaveOrUpdate<T>(T entity);
    }

    public class Repository<T> //: IRepository<T>
    {
        private ISession Session { get; set; }

        public Repository(ISession session)
        {
            Session = session;
        }

        public IQueryable<T> GetList()
        {
            return (from entity in Session.Linq<T>() select entity);
        }

        public T GetById(int id)
        {
            return Session.Get<T>(id);
        }

        public void Save(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public T GetOne(QueryBase<T> query)
        {
            return query.SatisfyingElementFrom(Session.Linq<T>());
        }

        public IQueryable<T> GetList(QueryBase<T> query)
        {
            return query.SatisfyingElementsFrom(Session.Linq<T>());
        }
    }
}