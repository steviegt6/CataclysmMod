using System;
using Terraria;

namespace CataclysmMod.Common.References
{
    public interface IEntityReference<TEntity> where TEntity : Entity
    {
        int Id { get; }

        void Execute(ref TEntity[] array, Action<TEntity> action);
    }
}