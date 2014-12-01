using UnityEngine;
using System.Collections;

public interface IDamageable {

	void Hurt(float dmgTaken);
	bool IsAlive();

}
