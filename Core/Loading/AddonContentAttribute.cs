#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;

namespace CataclysmMod.Core.Loading
{
    public class AddonContentAttribute : Attribute
    {
        public readonly Type AddonType;

        public AddonContentAttribute(Type addonType)
        {
            if (!addonType.IsInstanceOfType(typeof(Addon<>)))
                throw new Exception("Non-addon type passed into constructor of AddonContentAttribute");
            
            AddonType = addonType;
        }
    }
}