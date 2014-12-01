using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour {

	public StatusEffectKind kind;

	private bool canTick = true;
	private float tickDelay;
	private float timeSpentInTickCooldown = 0;

	public StatusEffect(StatusEffectKind kind /* entity */) {
		this.kind = kind;

	}
	
	// Update is called once per frame
	void Update () {
		switch (kind) {
		case StatusEffectKind.SLOW:
			TickSlow();
			break;
		case StatusEffectKind.POISON:
			SetTickDelay(1f);
			TickPoison();
			break;
		default:
			break;
		}
	}

	void SetTickDelay(float delay) {
		if (tickDelay.Equals(null)) {
			tickDelay = delay;
		}
	}

	void TickSlow() {
		Debug.Log ("Sloww");
	}

	void TickPoison() {
		if (!canTick) {
			timeSpentInTickCooldown += Time.deltaTime;

			if (timeSpentInTickCooldown > tickDelay) {
				canTick = true;
				timeSpentInTickCooldown = 0;
			}
		}

		if (canTick) {
			Debug.Log ("Poisoned!");
			canTick = false;
		}
	}
}
