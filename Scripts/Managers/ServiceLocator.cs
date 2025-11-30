using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Register<T>(T _service) where T : class
    {
        services[typeof(T)] = _service;
    }

    public static T GetService<T>() where T : class
    {
        if (services.TryGetValue(typeof(T), out var _service))
        {
            return (T)_service;
        }
        return null;
    }

    public static object Get(Type _type)
    {
        if (services.TryGetValue(_type, out var _service))
        {
            return _service;
        }
        return null;
    }

    public static bool Contains<T>() where T : class
    {
        return services.ContainsKey(typeof(T));
    }

    public static void Remove<T>() where T : class
    {
        services.Remove(typeof(T));
    }
}
