
using System;
using System.Collections.Generic;

public static class ServiceProvider
{
    private static Dictionary<Type, object> _services = new();

    public static void Register<T>(T service) where T : class
    {
        if (_services.ContainsKey(typeof(T))) return;

        _services[typeof(T)] = service;
    }

    public static T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return service as T;
        }

        return null;
    }
}
