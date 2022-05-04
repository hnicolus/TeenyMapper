﻿using System.Reflection;
using System.Reflection.Emit;

namespace MiniMapper;

public static class Mapper
{
    private static readonly Dictionary<(Type, Type), MethodInfo> Cache = new();
    public static T Map<T>(object v)
    {
        var fromType = v.GetType();

        var toType = typeof(T);
        var key = (fromType, toType);

        if (!Cache.ContainsKey(key))
        {
            Cache[key] = CreateMapMethod(fromType, toType);
        }

        return (T)Cache[key].Invoke(null, new[] { v });
    }
    static MethodInfo CreateMapMethod(Type fromType, Type toType)
    {

        AssemblyName aName = new AssemblyName("InternalMapperAssembly");
        var ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
        var mb = ab.DefineDynamicModule(aName.Name);

        var typeBuilder = mb.DefineType("Mapper", TypeAttributes.NotPublic);
        var methodBuilder = typeBuilder.DefineMethod(
            "Map",
            MethodAttributes.Public | MethodAttributes.Static,
            toType,
            new[] { fromType });

        var gen = methodBuilder.GetILGenerator();
        gen.Emit(OpCodes.Newobj, toType.GetConstructor(Type.EmptyTypes));

        var properties = fromType.GetProperties();

        foreach (var property in properties)
        {
            var toProp = toType.GetProperty(property.Name);

            //Skip Method if MappingTo Type does not have property
            if (toProp == null) continue;

            gen.Emit(OpCodes.Dup);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Callvirt, property.GetMethod);
            gen.Emit(OpCodes.Callvirt, toProp.SetMethod);
        }

        gen.Emit(OpCodes.Ret);

        var type = typeBuilder.CreateType();
        return type.GetMethod("Map", BindingFlags.Public | BindingFlags.Static, new[] { fromType });
    }
}