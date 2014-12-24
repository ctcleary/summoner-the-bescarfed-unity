using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

	private Transform parentTransform;
	
	public Sprite black;
	public Sprite healthColor;

	private float barWidth = 24f;
	private float barHeight = 2f;
	private float barWidthInWorldUnits;
//	private float barHeightInWorldUnits;

	private float yOffset = 1f;
	private Facing facing = Facing.RIGHT;

	private GameObject renderObjectBlack;
	private GameObject renderObjectBar;
	
	private SpriteRenderer renderBlack;
	private SpriteRenderer renderBar;
	
	private Transform renderTransformBlack;
	private Transform renderTransformBar;

	// Use this for initialization
	void Start () {
		barWidthInWorldUnits = barWidth/16f;
//		barHeightInWorldUnits = barHeight/16f;

		renderObjectBlack = CreateRendererObject ("RenderObjectBlack", transform);
		renderObjectBar = CreateRendererObject ("RenderObjectRed", transform);
		
		renderBlack = AddRenderer (renderObjectBlack, black);
		renderBar = AddRenderer (renderObjectBar, healthColor);

		SetBarScale();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void UpdateHealthBar(float percHealth, Facing newFacing)
	{
		if (renderObjectBar == null) {
			return;
		}
		percHealth = Mathf.Clamp (percHealth, 0f, 1f);

		Vector3 newHealthScale = renderObjectBar.transform.localScale;
		newHealthScale.x = barWidth * percHealth;
		renderObjectBar.transform.localScale = newHealthScale;

		if (facing != newFacing) {
			facing = newFacing;
			Vector3 newContainerScale = transform.localScale;
			newContainerScale.x = -newContainerScale.x;
			transform.localScale = newContainerScale;
		}
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
		renderer.sortingLayerName = "UI";
		return renderer;
	}

	public void SetColorSprite(Sprite newSprite)
	{
		healthColor = newSprite;
		if (renderBar != null) {
			renderBar.sprite = healthColor;
		}
	}

	private void SetBarScale()
	{
		if (renderObjectBlack == null || renderObjectBar == null) {
			return;
		}
		renderObjectBlack.transform.localPosition = new Vector3(-barWidthInWorldUnits/2, yOffset, -5);
		renderObjectBlack.transform.localScale = new Vector3(barWidth, barHeight, 0);
		
		renderObjectBar.transform.localPosition = new Vector3(-barWidthInWorldUnits/2, yOffset, -6);
		renderObjectBar.transform.localScale = new Vector3(barWidth, barHeight, 0);
	}

	public void SetBarSize(float barWidth, float barHeight)
	{
		SetBarWidth(barWidth);
		SetBarHeight(barHeight);
		SetBarScale();
	}

	public void SetBarWidth(float barWidth)
	{
		this.barWidth = barWidth;
		barWidthInWorldUnits = this.barWidth/16f;
	}

	public void SetBarHeight(float barHeight)
	{
		this.barHeight = barHeight;
//		barHeightInWorldUnits = this.barHeight/16f;
	}

	public void SetYOffset(float yOffset)
	{
		this.yOffset = yOffset;
		SetBarScale();
	}
}
