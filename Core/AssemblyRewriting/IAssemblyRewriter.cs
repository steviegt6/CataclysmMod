#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CataclysmMod.Core.AssemblyRewriting
{
    public interface IAssemblyRewriter
    {
        void Rewrite(ModuleDefinition module, ILProcessor cil, Instruction instruction);
    }
}