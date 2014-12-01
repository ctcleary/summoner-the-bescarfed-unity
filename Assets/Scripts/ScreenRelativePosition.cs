using UnityEngine;
using System.Collections;

public class ScreenRelativePosition : MonoBehaviour {

	public enum ScreenEdge { TOP, RIGHT, BOTTOM, LEFT };
	public ScreenEdge screenEdge;
	public float xOffset;
	public float yOffset;

	// Use this for initialization
	void Start () {
		Vector3 newPosition = transform.position;

		Camera mainCam = Camera.main;
		float aspect = mainCam.aspect;
		float orthoSize = mainCam.orthographicSize;

		switch (screenEdge) {
		case ScreenEdge.TOP:
			newPosition.x = xOffset;
			newPosition.y = orthoSize + yOffset;
			break;
		case ScreenEdge.BOTTOM:
			newPosition.x = xOffset;
			newPosition.y = -1 * orthoSize + yOffset;
			break;
		case ScreenEdge.RIGHT:
			newPosition.x = aspect * orthoSize + xOffset;
			newPosition.y = yOffset;
			break;
		case ScreenEdge.LEFT:
			newPosition.x = -1 * aspect * orthoSize + xOffset;
			newPosition.y = yOffset;
			break;
		}

		transform.position = newPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
