﻿using System;
using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;

namespace CataclysmMod.Content.Default.MonoMod
{
    public abstract class MonoModPatcher<T>
        where T : class
    {
        protected MonoModPatcher()
        {
            Type type = typeof(T);

            if (type != typeof(string) && type != typeof(MethodInfo))
                throw new Exception("Invalid generic type in MonoModPatcher<T>");
        }

        public abstract MethodInfo Method { get; }

        public abstract T ModderMethod { get; }

        public virtual void Apply()
        {
            if (typeof(T) == typeof(string))
            {
                HookEndpointManager.Modify(Method,
                    Delegate.CreateDelegate(typeof(ILContext.Manipulator), GetType(),
                        ModderMethod as string ??
                        throw new InvalidOperationException("ModderMethod was not cast to String!")));
            }

            if (typeof(T) == typeof(MethodInfo))
            {
                new Hook(Method,
                    ModderMethod as MethodInfo ??
                    throw new InvalidOperationException("ModderMethod was not cast to MethodInfo!")).Apply();
            }
        }
    }
}