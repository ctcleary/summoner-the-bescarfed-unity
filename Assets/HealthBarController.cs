using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

	public Sprite black;
	public Sprite red;
	public Sprite green;
	
	private SpriteRenderer renderBlack;
	private SpriteRenderer renderRed;
	private SpriteRenderer renderGreen;
	
	private GameObject renderObjectBlack;
	private GameObject renderObjectRed;
	private GameObject renderObjectGreen;

	// Use this for initialization
	void Start () {
		
		renderObjectBlack = CreateRendererObject ("RenderObjectBlack");
		renderObjectRed = CreateRendererObject ("RenderObjectRed");
		renderObjectGreen = CreateRendererObject ("RenderObjectGreen");
		
		renderBlack = AddRenderer (renderObjectBlack, black);
		renderRed = AddRenderer (renderObjectBlack, red);
		renderGreen = AddRenderer (renderObjectBlack, green);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private GameObject CreateRendererObject(string name)
	{
		GameObject obj = new GameObject ();
		obj.name = name;
		obj.transform.parent = transform;
		return obj;
	}

	private SpriteRenderer AddRenderer(GameObject addToThis, Sprite sprite)
	{
		SpriteRenderer renderer = addToThis.AddComponent("SpriteRenderer") as SpriteRenderer;
		renderer.sprite = sprite;
		return renderer;
	}
}
