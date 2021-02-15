using CataclysmMod.Common.Detours;
using System;
using System.Collections.Generic;
using CataclysmMod.Common.MonoMod;

namespace CataclysmMod.Common
{
    public static class ILManager
    {
        public static Dictionary<string, Detour> Detours;
        public static Dictionary<string, ILEdit> ILEdits;

        public static void Load()
        {
            Detours = new Dictionary<string, Detour>();
            ILEdits = new Dictionary<string, ILEdit>();

            foreach (Type type in CataclysmMod.Instance.Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetConstructor(new Type[] { }) != null)
                {
                    if (type.IsSubclassOf(typeof(Detour)))
                    {
                        Detour detour = Activator.CreateInstance(type) as Detour;

                        if (detour.Autoload())
                            Detours.Add(detour.DictKey, detour);
                    }

                    if (type.IsSubclassOf(typeof(ILEdit)))
                    {
                        ILEdit ilEdit = Activator.CreateInstance(type) as ILEdit;

                        if (ilEdit.Autoload())
                            ILEdits.Add(ilEdit.DictKey, ilEdit);
                    }
                }
            }

            foreach (Detour detour in Detours.Values)
                detour.Load();

            foreach (ILEdit ilEdit in ILEdits.Values)
                ilEdit.Load();
        }

        public static void Unload()
        {
            foreach (Detour detour in Detours.Values)
                detour.Unload();

            foreach (ILEdit ilEdit in ILEdits.Values)
                ilEdit.Unload();

            Detours = null;
            ILEdits = null;
        }
    }
}