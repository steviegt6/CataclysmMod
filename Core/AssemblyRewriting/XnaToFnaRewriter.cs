#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReLogic.OS;

namespace CataclysmMod.Core.AssemblyRewriting
{
    public class XnaToFnaRewriter : IAssemblyRewriter
    {
        public static ModuleDefinition GetFnaModule() =>
            ModuleDefinition.ReadModule(typeof(SpriteBatch).Module.FullyQualifiedName);

        public void Rewrite(ModuleDefinition module, ILProcessor cil, Instruction instruction)
        {
            if (Platform.IsWindows)
                return;

            bool IsFieldReference() => instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldsfld ||
                                       instruction.OpCode == OpCodes.Stfld || instruction.OpCode == OpCodes.Stsfld;

            bool IsMethodReference() => instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt ||
                                        instruction.OpCode == OpCodes.Newobj;

            if (IsFieldReference())
            {
                FieldReference fieldReference = (FieldReference) instruction.Operand;
                FieldDefinition definition = fieldReference.Resolve();

                // Ensure the reference/definition does not exist.
                if (!(definition is null)) 
                    return;

                ModuleDefinition fna = GetFnaModule();
                TypeReference typeReference = new TypeReference(fieldReference.FieldType.Namespace, fieldReference.FieldType.Name, fna, null);
                FieldReference newFieldReference = new FieldReference(fieldReference.Name, typeReference);
                Instruction replacement = Instruction.Create(instruction.OpCode, newFieldReference);
                cil.Replace(instruction, replacement);
            }
            else if (IsMethodReference())
            {
                MethodReference methodReference = (MethodReference) instruction.Operand;
                MethodDefinition definition = methodReference.Resolve();

                // Ensure the reference/definition does not exist.
                if (!(definition is null))
                    return;

                ModuleDefinition fna = GetFnaModule();

                TypeReference returningReference = methodReference.ReturnType;
                bool rewriteReturningReference = returningReference.Resolve() is null;

                TypeReference declaringReference = methodReference.DeclaringType;
                bool rewriteDeclaringReference = !(declaringReference is null) && declaringReference.Resolve() is null;

                if (rewriteReturningReference)
                    returningReference = new TypeReference(returningReference.Namespace, returningReference.Name, fna, null);

                if (rewriteDeclaringReference)
                    declaringReference = new TypeReference(declaringReference.Namespace, declaringReference.Name, fna, null);

                // Should be okay if declaringReference is null. Maybe! :)
                MethodReference newMethodReference = new MethodReference(methodReference.Name, returningReference, declaringReference);

                Instruction replacement = Instruction.Create(instruction.OpCode, newMethodReference);
                cil.Replace(instruction, replacement);
            }
        }
    }
}