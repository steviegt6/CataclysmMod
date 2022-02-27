#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;

namespace CataclysmMod.Core.Loading
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AddonContentAttribute : Attribute
    {
        public readonly Type AddonType;

        public AddonContentAttribute(Type addonType)
        {
            if (!addonType.IsSubclassOf(typeof(Addon)))
                throw new Exception($"Non-addon type passed into constructor of {nameof(AddonContentAttribute)}");
            
            AddonType = addonType;
        }
    }
}