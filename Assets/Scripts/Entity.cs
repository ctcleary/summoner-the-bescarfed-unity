using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public GameObject healthBarPrefab;
	public Sprite healthBarColorSprite;
	protected GameObject healthBarInstance;
	protected HealthBarController healthBarController;

	protected void AttachHealthBar(float barWidth = 24, float barHeight = 2, float yOffset = 1f)
	{
		Object healthBarObject = Instantiate (healthBarPrefab, transform.position, Quaternion.identity);
		GameObject healthBarInstance = healthBarObject as GameObject;

        healthBarInstance.transform.parent = transform;
		healthBarController = healthBarInstance.GetComponent<HealthBarController>();
        healthBarController.SetFacing(Facing.RIGHT);
        healthBarController.SetColorSprite(healthBarColorSprite);
		healthBarController.SetBarSize(barWidth, barHeight);
		healthBarController.SetYOffset(yOffset);
	}
}

