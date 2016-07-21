using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SingletonClass : MonoBehaviour { }

public abstract class SingletonClass<T> : SingletonClass where T: SingletonClass{

    private static T _instance = default(T);

    public static T instance
    {
        get
        {
            
            if (GenericsHelper.Compare<T>(_instance, default(T)) == true)
            {
                CreateInstance();
                return _instance;
            }
            else
            {           
                return _instance;
            }
        }
    }

    private void OnLevelWasLoaded()
    {
        _instance = default(T);
    }

    private static void CreateInstance()
    {
        
        T[] singeltonObjects = FindObjectsOfType(typeof(T)) as T[];

        if (_instance == null)
        {
            if(singeltonObjects.Length > 1)
            {
                Debug.LogError("There are multiple instances of this singleton in the scene!");
            }
            else if(singeltonObjects.Length == 1)
            {
                Debug.Log("An already existing singelton instance was found on initialization!");
                _instance = singeltonObjects[0];
            }
            else
            {                
                GameObject singletonGO = new GameObject();
                singletonGO.name = typeof(T).Name + "_Singleton";
                _instance = singletonGO.AddComponent<T>();
            }
        }

    }
}
