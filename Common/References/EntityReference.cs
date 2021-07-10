using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace CataclysmMod.Common.References
{
    public readonly struct EntityReference<TEntity> : IEntityReference<TEntity> where TEntity : Entity
    {
        public int Id { get; }

        public EntityReference(int id)
        {
            Id = id;
        }

        public EntityReference(TEntity entity) : this(entity.whoAmI)
        {
        }

        public void Execute(ref TEntity[] array, Action<TEntity> action) => action?.Invoke(array[Id]);

        public static IEnumerable<EntityReference<TCollectionEntity>> FormReferenceCollection<TCollectionEntity>(
            params TCollectionEntity[] entities) where TCollectionEntity : Entity =>
            entities.Select(projectile => new EntityReference<TCollectionEntity>(projectile));

        public static implicit operator EntityReference<TEntity>(TEntity reference) =>
            new EntityReference<TEntity>(reference);
    }
}