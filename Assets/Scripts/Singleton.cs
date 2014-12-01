using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T _instance;

	// Return the instance of this Singleton
	public static T instance
	{
		get
		{
			if (_instance == null)
			{
				// Make sure we don't already have one.
				_instance = (T) FindObjectOfType(typeof(T));

				// If not, make it.
				if (_instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) + " was needed but not found.");
				}
			}

			return _instance;
		}
	}
}
