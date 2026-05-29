using System;
using System.Reflection;
using UnityEngine;

public struct ScriptCore
{
    public static bool IsOverridedMethod(object script, string methodName)
    {
        Type type = script.GetType();
        MethodInfo method = type.GetMethod(methodName);
        return method.DeclaringType != type.BaseType;
    }
}
