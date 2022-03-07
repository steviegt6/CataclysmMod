#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CataclysmMod.Core
{
    public static class ReflectionHelper
    {
        #region Cache

        public enum ReflectionType
        {
            Field,
            Property,
            Method,
            Constructor,
            Type
        }

        private static Dictionary<ReflectionType, Dictionary<string, object>> ReflectionCache =>
            new Dictionary<ReflectionType, Dictionary<string, object>>
            {
                {ReflectionType.Field, new Dictionary<string, object>()},
                {ReflectionType.Property, new Dictionary<string, object>()},
                {ReflectionType.Type, new Dictionary<string, object>()},
                {ReflectionType.Constructor, new Dictionary<string, object>()},
                {ReflectionType.Method, new Dictionary<string, object>()}
            };

        public static BindingFlags UniversalFlags => BindingFlags.Public
                                                     | BindingFlags.NonPublic
                                                     | BindingFlags.Instance
                                                     | BindingFlags.Static;

        public static Type GetCachedType(this Assembly assembly, string typeName) =>
            RetrieveFromCache(ReflectionType.Type, typeName, () => assembly.GetType(typeName));

        public static FieldInfo GetCachedField(this Type type, string fieldName) =>
            RetrieveFromCache(
                ReflectionType.Field,
                GetFieldNameForCache(type, fieldName),
                () => type.GetField(fieldName, UniversalFlags)
            );

        public static PropertyInfo GetCachedProperty(this Type type, string propertyName) =>
            RetrieveFromCache
            (ReflectionType.Property,
                GetPropertyNameForCache(type, propertyName),
                () => type.GetProperty(propertyName, UniversalFlags)
            );

        public static ConstructorInfo GetCachedConstructor(this Type type, params Type[] types) =>
            RetrieveFromCache(
                ReflectionType.Constructor,
                GetConstructorNameForCache(type, types),
                () => type.GetConstructor(UniversalFlags, null, types, null)
            );

        public static MethodInfo GetCachedMethod(this Type type, string methodName) =>
            RetrieveFromCache(
                ReflectionType.Method,
                GetMethodNameForCache(type, methodName),
                () => type.GetMethod(methodName, UniversalFlags)
            );

        public static object InvokeUnderlyingMethod(
            this FieldInfo field,
            string methodName,
            object fieldInstance,
            params object[] parameters
        ) => field.FieldType.GetCachedMethod(methodName).Invoke(field.GetValue(fieldInstance), parameters);

        public static string GetFieldNameForCache(Type type, string fieldName)
        {
            string assemblyName = type.Assembly.GetName().Name;
            string typeName = type.Name;
            return $"{assemblyName}.{typeName}.{fieldName}";
        }

        public static string GetPropertyNameForCache(Type type, string property)
        {
            string assemblyName = type.Assembly.GetName().Name;
            string typeName = type.Name;
            return $"{assemblyName}.{typeName}.{property}";
        }

        public static string GetConstructorNameForCache(Type type, params Type[] types)
        {
            string assemblyName = type.Assembly.GetName().Name;
            string typeName = type.Name;
            List<string> typeNames = types.Select(cType => cType.Name).ToList();
            return $"{assemblyName}.{typeName}::{{{string.Join(",", typeNames)}}}";
        }

        public static string GetMethodNameForCache(Type type, string method)
        {
            string assemblyName = type.Assembly.GetName().Name;
            string typeName = type.Name;
            return $"{assemblyName}.{typeName}::{method}";
        }

        private static TReturn RetrieveFromCache<TReturn>(ReflectionType refType, string key, Func<TReturn> fallback)
        {
            if (ReflectionCache[refType].ContainsKey(key))
                return (TReturn) ReflectionCache[refType][key];

            TReturn value = fallback();
            ReflectionCache[refType].Add(key, value);
            return value;
        }

        #endregion

        public static TFieldType GetFieldValue<TType, TFieldType>(this TType obj, string field) =>
            (TFieldType) typeof(TType).GetCachedField(field)?.GetValue(obj);

        public static void SetFieldValue<TType, TFieldType>(this TType obj, string field, TFieldType value) =>
            typeof(TType).GetCachedField(field)?.SetValue(obj, value);

        public static TFieldType GetPropertyValue<TType, TFieldType>(this TType obj, string property) =>
            (TFieldType) typeof(TType).GetCachedProperty(property)?.GetValue(obj);

        public static void SetPropertyValue<TType, TFieldType>(this TType obj, string property, TFieldType value) =>
            typeof(TType).GetCachedProperty(property)?.SetValue(obj, value);

        public static FieldInfo GetCachedField<TType>(string fieldName) => typeof(TType).GetCachedField(fieldName);

        public static PropertyInfo GetCachedProperty<TType>(string propertyName) =>
            typeof(TType).GetCachedProperty(propertyName);

        public static void SetToNewInstance(
            this FieldInfo field,
            object fieldInstance = null,
            object constructedInstance = null
        ) => field.SetValue(fieldInstance, constructedInstance ?? Activator.CreateInstance(field.FieldType));

        public static void SetToNewInstance(
            this PropertyInfo property,
            object propertyInstance = null,
            object constructedInstance = null
        ) => property.SetValue(
            propertyInstance,
            constructedInstance ?? Activator.CreateInstance(property.PropertyType)
        );

        public static T GetValue<T>(this FieldInfo field, object fieldInstance) => (T) field.GetValue(fieldInstance);

        public static T GetValue<T>(this PropertyInfo property, object propertyInstance) => 
            (T) property.GetValue(propertyInstance);
    }
}