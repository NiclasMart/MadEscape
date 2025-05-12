
using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;



public static class ServiceProvider
{
    private static Dictionary<Type, object> _services = new();

    // register new service
    // if the service type is already registerd, destroy it
    public static void Register<T>(T service, GameObject go = null) where T : class
    {
        // check if the IService interface is implemented
        if (service is not IService)
        {
            Debug.LogError($"The object {go.name} was tried to be registered as Service, but it didn't implement the IService interface. Ensure that all services implement this interface.");
        }

        // check if the service is already registered
        if (HasService<T>())
        {
            if (go != null) UnityEngine.Object.Destroy(go);
            return;
        }

        _services[typeof(T)] = service;
        (service as IService).Load();

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

    public static IService[] GetAll()
    {
        return _services.Values.OfType<IService>().ToArray();
    }
}
