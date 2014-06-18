using System;

namespace Nx
{
    public interface IEntity
    {
    }

    public interface IEntity<TId> : IEntity
        where TId : IEquatable<TId>
    {
    }
}