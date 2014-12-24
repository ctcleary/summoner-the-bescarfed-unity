using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

	private Transform parentTransform;
	
	public Sprite black;
	public Sprite red;
	public Sprite green;

	private float barWidth = 24f/16f;

	private GameObject renderObjectBlack;
	private GameObject renderObjectBar;
	
	private SpriteRenderer renderBlack;
	private SpriteRenderer renderBar;
	
	private Transform renderTransformBlack;
	private Transform renderTransformBar;

	// Use this for initialization
	void Start () {
//		parentTransform = GetComponentInParent<Transform>();
//		if (parentTransform == null) {
//			Debug.Log ("Health bar has no parent at Start time.");
//		}
//
//		transform.parent = parentTransform;

		renderObjectBlack = CreateRendererObject ("RenderObjectBlack", transform);
		renderObjectBar = CreateRendererObject ("RenderObjectRed", transform);
		
		renderBlack = AddRenderer (renderObjectBlack, black);
		renderBar = AddRenderer (renderObjectBar, green);
		
		renderObjectBlack.transform.localPosition = new Vector3(-barWidth/2, 1.2f, -1);
		renderObjectBlack.transform.localScale = new Vector3(24, 2, 0);

		renderObjectBar.transform.localPosition = new Vector3(-barWidth/2, 1.2f, -2);
		renderObjectBar.transform.localScale = new Vector3(24, 2, 0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newScale = renderObjectBar.transform.localScale;
		newScale.x = newScale.x * 0.95f; // TODO plug health percentage in here
		renderObjectBar.transform.localScale = newScale;
	}

	private GameObject CreateRendererObject(string name, Transform parentTransform)
	{
		GameObject obj = new GameObject ();
		obj.name = name;
		obj.transform.position = new Vector3(0,0,0);
		obj.transform.parent = parentTransform;
		return obj;
	}

	private SpriteRenderer AddRenderer(GameObject addToThis, Sprite sprite)
	{
		SpriteRenderer renderer = addToThis.AddComponent("SpriteRenderer") as SpriteRenderer;
		renderer.sprite = sprite;
		return renderer;
	}
}
