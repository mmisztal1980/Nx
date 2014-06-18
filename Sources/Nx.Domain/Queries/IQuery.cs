using System;
using System.Linq;
using System.Linq.Expressions;

namespace Nx.Queries
{
    public interface IQuery<TEntity>
    {
        Expression<Func<TEntity, bool>> MatchingCriteria { get; }

        TEntity SatisfyingElementFrom(IQueryable<TEntity> candidates);

        IQueryable<TEntity> SatisfyingElementsFrom(IQueryable<TEntity> candidates);
    }
}