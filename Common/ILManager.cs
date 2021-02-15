using System;
using System.Collections.Generic;
using CataclysmMod.Common.MonoMod;

namespace CataclysmMod.Common
{
    public static class ILManager
    {
        public static Dictionary<string, ILEdit> ILEdits;

        public static void Load()
        {
            ILEdits = new Dictionary<string, ILEdit>();

            foreach (Type type in CataclysmMod.Instance.Code.GetTypes())
            {
                if (type.IsAbstract || type.GetConstructor(new Type[] { }) == null)
                    continue;

                if (!type.IsSubclassOf(typeof(ILEdit)))
                    continue;

                if (Activator.CreateInstance(type) is ILEdit ilEdit && ilEdit.Autoload())
                    ILEdits.Add(ilEdit.DictKey, ilEdit);
            }

            foreach (ILEdit ilEdit in ILEdits.Values)
                ilEdit.Load();
        }

        public static void Unload()
        {
            foreach (ILEdit ilEdit in ILEdits.Values)
                ilEdit.Unload();

            ILEdits = null;
        }
    }
}