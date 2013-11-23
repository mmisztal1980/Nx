using System;

namespace Nx.EF
{
    public interface IEntity<TId>
        where TId : IEquatable<TId>
    {
        TId Id { get; }
    }
}