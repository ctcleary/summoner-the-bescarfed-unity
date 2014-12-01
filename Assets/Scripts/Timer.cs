using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	
	public delegate void CallbackFunc ();

	private CallbackFunc callbackFunc;
	private float timeToWait;
	private float timeWaited;

	public Timer (float timeToWait, CallbackFunc callbackFunc = null)
	{
		this.timeToWait = timeToWait;
		this.callbackFunc = callbackFunc;
		timeWaited = 0;
	}

	public IEnumerator doTimer ()
	{
		while (timeWaited < timeToWait) {
			timeWaited += Time.deltaTime;
			yield return null;
		}

		callbackFunc ();
	}
}
