using UnityEngine;
using System;
using System.Collections.Generic;

public static class Singleton_Service
{

    private static Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
    //Adds a singleton to the manager
    public static void RegisterSingletonInstance<T>(T instance) where T : class
    {

        if (!_singletons.ContainsKey(typeof(T)))
        {
            _singletons[typeof(T)] = instance;
        }
        else
        {
            throw new System.InvalidOperationException("A singleton of this type has already been registered.");
        }
    }

    //Removes a singleton type from the manager
    public static void UnregisterSingletonInstance<T>(T instance) where T : class
    {
        if (!_singletons.Remove(typeof(T)))
        {
            throw new System.InvalidOperationException("You are trying to remove a singleton that does not exist!");
        }
    }

    //Attempts to get a singleton from the manager
    public static T GetSingleton<T>() where T : class
    {
        Type requestedType = typeof(T);
        object o = null;
        if (!_singletons.TryGetValue(requestedType, out o))
        {
            //Debug.Log("The type you are attempting to retreive has not been registered as a Singleton are you sure you are looking for the correct type?");
        }
        return (T)o;
    }

}
