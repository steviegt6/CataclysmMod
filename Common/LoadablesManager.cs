using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace CataclysmMod.Common
{
    public static class ApplicableManager
    {
        public interface IApplicable
        {
            void Apply();
        }

        public static List<IApplicable> ApplicableContainer { get; set; }

        public static void Load()
        {
            ApplicableContainer = new List<IApplicable>();

            foreach (Type type in CataclysmMod.Instance.Code.GetTypes())
            {
                if (type.IsAbstract || type.GetConstructor(new Type[] { }) == null || !type.IsInstanceOfType(typeof(IApplicable)))
                    continue;

                if (Activator.CreateInstance(type) is IApplicable loadable)
                    ApplicableContainer.Add(loadable);
            }

            MonoModHooks.RequestNativeAccess();

            foreach (IApplicable loadable in ApplicableContainer)
                loadable.Apply();
        }

        public static void Unload()
        {
            ApplicableContainer = null;
        }
    }
}