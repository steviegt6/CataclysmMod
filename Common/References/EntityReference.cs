using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace CataclysmMod.Common.References
{
    public readonly struct EntityReference<TEntity> where TEntity : Entity
    {
        public delegate void EntityAction(ref TEntity entity);

        public int Id { get; }

        public EntityReference(int id)
        {
            Id = id;
        }

        public EntityReference(TEntity entity) : this(entity.whoAmI)
        {
        }

        public void Execute(ref TEntity[] array, EntityAction action) => action?.Invoke(ref array[Id]);

        public static IEnumerable<EntityReference<TCollectionEntity>> FromReferenceCollection<TCollectionEntity>(
            params TCollectionEntity[] entities) where TCollectionEntity : Entity =>
            entities.Select(projectile => new EntityReference<TCollectionEntity>(projectile));

        public static implicit operator EntityReference<TEntity>(TEntity reference) =>
            new EntityReference<TEntity>(reference);
    }
}