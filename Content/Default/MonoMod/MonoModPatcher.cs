using System;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;

namespace CataclysmMod.Content.Default.MonoMod
{
    public abstract class MonoModPatcher<T>
        where T : class
    {
        public abstract MethodInfo Method { get; }

        public abstract T ModderMethod { get; }

        public virtual void Apply()
        {
            switch (ModderMethod)
            {
                case string method:
                    Delegate @delegate = Delegate.CreateDelegate(typeof(ILContext.Manipulator), GetType(), method);
                    HookEndpointManager.Modify(Method, @delegate);
                    Cataclysm.Modifiers.Add((Method, @delegate));
                    break;

                case MethodInfo method:
                    Hook hook = new Hook(Method, method);
                    hook.Apply();
                    Cataclysm.Hooks.Add(hook);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(ModderMethod),
                        $"Unexpected generic type passed to {nameof(MonoModPatcher<T>)}<{nameof(T)}>");
            }
        }
    }
}