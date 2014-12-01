using UnityEngine;
using System.Collections;

public class Timer
{
	
	public delegate void CallbackFunc ();

	private CallbackFunc callbackFunc;
	private float timeToWait;
	private float timeWaited;

	public Timer ()
	{
		this.timeToWait = 1;
		timeWaited = 0;
	}

	public Timer (float timeToWait, CallbackFunc callbackFunc = null)
	{
		this.timeToWait = timeToWait;
		this.callbackFunc = callbackFunc;
		timeWaited = 0;
	}

	public IEnumerator DoTimer ()
	{
		timeWaited += Time.deltaTime;
		while (timeWaited < timeToWait) {
			yield return null;
		}

		if (!callbackFunc.Equals (null)) {
			callbackFunc ();
		}
		timeWaited = 0;
	}

	public void Reset()
	{
		timeWaited = 0;
	}
}
