using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quasar.Models
{
    public abstract class SingletonModel<T> : MonoBehaviour where T : SingletonModel<T>
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Try to find an existing instance in the scene
                    _instance = FindObjectOfType<T>();

                    // If no instance is found, throw Exception
                    if (_instance == null)
                    {
                        throw new System.Exception($"An Instance of {typeof(T).ToString()}");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                //DontDestroyOnLoad(gameObject); // Optional: Makes sure the instance persists across scene loads
            }
            else
            {
                if (!ReferenceEquals(this, _instance))
                {
                    Destroy(gameObject); // Ensures only one instance exists
                }
            }
        }

    }

}

