#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;

namespace CataclysmMod.Core.Weaving
{
    /// <summary>
    ///     Creates detour hooks for MonoMod.
    /// </summary>
    public static class HookCreator
    {
        public static void Edit(MethodInfo editedMethod, ILContext.Manipulator editingMethod)
        {
            HookEndpointManager.Modify(editedMethod, editingMethod);
        }
        
        public static void Detour(MethodInfo detouredMethod, MethodInfo detouringMethod)
        {
            Hook hook = new Hook(detouredMethod, detouringMethod);
            hook.Apply();
        }
    }
}