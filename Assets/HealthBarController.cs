using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

	private Transform parentTransform;
	
	public Sprite black;
	public Sprite red;
	public Sprite green;

	private GameObject renderObjectBlack;
	private GameObject renderObjectRed;
	private GameObject renderObjectGreen;
	
	private SpriteRenderer renderBlack;
	private SpriteRenderer renderRed;
	private SpriteRenderer renderGreen;
	
	private Transform renderTransformBlack;
	private Transform renderTransforRed;
	private Transform renderTransforGreen;

	// Use this for initialization
	void Start () {
//		parentTransform = GetComponentInParent<Transform>();
//		if (parentTransform == null) {
//			Debug.Log ("Health bar has no parent at Start time.");
//		}
//
//		transform.parent = parentTransform;

		renderObjectBlack = CreateRendererObject ("RenderObjectBlack", transform);
		renderObjectRed = CreateRendererObject ("RenderObjectRed", transform);
		renderObjectGreen = CreateRendererObject ("RenderObjectGreen", transform);
		
		renderBlack = AddRenderer (renderObjectBlack, black);
		renderRed = AddRenderer (renderObjectRed, red);
		renderGreen = AddRenderer (renderObjectGreen, green);
		
//		renderObjectBlack.transform.position = new Vector3(0, 2.2f, 0);
		renderObjectBlack.transform.localScale = new Vector3(24, 4, 0);

//		renderObjectGreen.transform.position = new Vector3(0, 2.2f, -1);
		renderObjectGreen.transform.localScale = new Vector3(22, 2f, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
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
