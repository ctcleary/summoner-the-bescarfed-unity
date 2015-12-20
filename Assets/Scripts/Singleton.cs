using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T _instance;

	// Return the instance of this Singleton
	public static T Instance
    {
        get
		{
            if (_instance == null)
            {
                Debug.LogError("An instance of " + typeof(T) + " was needed but not found.");
            }

            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }
}
