
using System;
using System.Collections.Generic;
using UnityEngine;



public static class ServiceProvider
{
    private static Dictionary<Type, object> _services = new();

    // register new service
    // if the service type is already registerd, destroy it
    public static void Register<T>(T service, GameObject go = null) where T : class
    {
        if (HasService<T>())
        {
            if (go != null) UnityEngine.Object.Destroy(go);
            return;
        }

        _services[typeof(T)] = service;

        if (go != null)
        {
            UnityEngine.Object.DontDestroyOnLoad(go);
        }
    }

    public static bool HasService<T>()
    {
        return _services.ContainsKey(typeof(T));
    }

    public static T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return service as T;
        }

        Debug.LogError($"Service Loader has not registerd service of type {typeof(T).Name}");
        return null;
    }
}
