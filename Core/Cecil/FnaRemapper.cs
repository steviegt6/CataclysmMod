// Copyright (C) 2021 Tomat and Contributors, MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CataclysmMod.Common.Utilities;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils;
using Terraria.ModLoader;

namespace CataclysmMod.Core.Cecil
{
    public static class FnaRemapper
    {
        public static Dictionary<string, TypeReference> Reference;
        public static ModuleDefinition CurrentModule;

        public static void RemapAssembly(Stream asmStream)
        {
            Reference = new Dictionary<string, TypeReference>();

            AssemblyDefinition asmDef = AssemblyDefinition.ReadAssembly(asmStream);

            foreach (ModuleDefinition module in asmDef.Modules.Where(
                module => module.AssemblyReferences.Any(
                    asmRef => asmRef.Name.Contains("Microsoft.Xna"))
                )
            ) RemapModule(module);

            using (MemoryStream symbolStream = new MemoryStream())
            {
                asmDef.Write(asmStream, new WriterParameters
                {
                    WriteSymbols = true,
                    SymbolStream = symbolStream,
                    SymbolWriterProvider = new PortablePdbWriterProvider()
                });

            }
        }

        public static void RemapModule(ModuleDefinition module)
        {
            CurrentModule = module;

            foreach (TypeDefinition typeDef in module.Types)
                RemapType(typeDef);

            AssemblyName fnaName = typeof(Vector2).Assembly.GetName();
            List<AssemblyNameReference> list = module.AssemblyReferences.ToList();
            list.RemoveAll(x => x.Name.Contains("Microsoft.Xna"));
            list.Add(new AssemblyNameReference(fnaName.FullName.Replace("\\", ""), fnaName.Version));
            module.AssemblyReferences.Clear();
            module.AssemblyReferences.AddRange(list);
        }

        private static void RemapType(TypeDefinition type)
        {
            foreach (MethodDefinition method in type.Methods) 
                RemapMethodDefinition(method);

            foreach (FieldDefinition field in type.Fields)
                field.FieldType = GetNewReference(field.FieldType);
        }

        private static void RemapMethodDefinition(MethodDefinition method)
        {
            if (method == null)
                return;

            RemapMethodReference(method);

            if (!method.HasBody)
                return;

            foreach (Instruction instr in method.Body.Instructions)
                switch (instr.Operand)
                {
                    case TypeReference typeRef:
                        instr.Operand = GetNewReference(typeRef);
                        break;

                    case MethodReference methodRef:
                        RemapMethodReference(methodRef);
                        instr.Operand = GetNewReference(methodRef);
                        break;
                }
        }

        private static void RemapMethodReference(MethodReference method)
        {
            if (method == null)
                return;

            if (!(method.DeclaringType is null))
            {
                void DoThis(MethodSpecification refer)
                {
                    while (true)
                    {
                        MethodReference newRef = refer.GetFieldValue<MethodSpecification, MethodReference>("method");

                        if (newRef is MethodSpecification refSpec)
                        {
                            refer = refSpec;
                            continue;
                        }

                        newRef.SetFieldValue<MethodReference, object>("projection", null);
                        newRef.DeclaringType.SetFieldValue<TypeReference, object>("projection", null);
                        newRef.DeclaringType = GetNewReference(method.DeclaringType);
                        break;
                    }
                }

                if (method is MethodSpecification spec)
                {
                    DoThis(spec);
                }
                else
                {
                    method.SetFieldValue<MethodReference, object>("projection", null);
                    method.DeclaringType.SetFieldValue<TypeReference, object>("projection", null);
                    method.DeclaringType = GetNewReference(method.DeclaringType);
                }
            }

            if (!(method.ReturnType is null))
            {
                if (method is MethodSpecification spec)
                {
                    MethodReference newRef = spec.GetFieldValue<MethodSpecification, MethodReference>("method");
                    newRef.SetFieldValue<MethodReference, object>("projection", null);
                    newRef.ReturnType.SetFieldValue<TypeReference, object>("projection", null);
                    newRef.ReturnType = GetNewReference(method.ReturnType);
                }
                else
                {
                    method.SetFieldValue<MethodReference, object>("projection", null);
                    method.ReturnType.SetFieldValue<TypeReference, object>("projection", null);
                    method.ReturnType = GetNewReference(method.ReturnType);
                }
            }

            foreach (ParameterDefinition parameter in method.Parameters)
                parameter.ParameterType = GetNewReference(parameter.ParameterType);
        }

        public static TypeReference GetNewReference(TypeReference member)
        {
            ModContent.GetInstance<Cataclysm>().Logger.Info("Getting reference of: " + member.FullName);

            if (member.DeclaringType is null || !IsRemappable(member.Name))
                return member;

            if (Reference.TryGetValue(member.Name, out TypeReference newRef))
                member = newRef;
            else
                member = Reference[member.Name] = GetSpecialRemap(member);

            member.SetFieldValue<TypeReference, object>("projection", null);

            return Reference[member.Name] = GetSpecialRemap(member);
        }

        public static MethodReference GetNewReference(MethodReference member)
        {
            if (member.DeclaringType is null || !IsRemappable(member.Name))
                return member;

            ModContent.GetInstance<Cataclysm>().Logger.Info("Getting member reference of: " + member.FullName);

            Type requestedType = typeof(Vector2).Assembly.GetType(member.FullName);
            MethodInfo requestedMethod = requestedType.GetMethod(member.Name,
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return CurrentModule.ImportReference(requestedMethod);
        }

        private static bool IsRemappable(string name) => name.StartsWith("Microsoft.Xna");
        
        private static TypeReference GetSpecialRemap(MemberReference member)
        {
            Type requestedType = typeof(Vector2).Assembly.GetType(member.FullName);

            if (requestedType is null)
                throw new NullReferenceException("Null FNA type: " + member.FullName);
            else
                ModContent.GetInstance<Cataclysm>().Logger.Info("Not null FNA type: " + member.FullName);

            return CurrentModule.ImportReference(requestedType);
        }
    }
}