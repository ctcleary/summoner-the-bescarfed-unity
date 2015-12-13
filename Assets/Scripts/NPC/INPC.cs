using UnityEngine;
using System.Collections;

public interface INPC
{

	void Reset (); // Reset health and attackDmg, etc.

	//void SetAttackTarget (IDamageable attackTarget);
	//void Attack (); // Attack the "AttackTarget"
	//void StopFighting (); // Resume walking.

	//void BounceAgainstAlly(Collider2D other); // On collision with ally.
	//void AvoidAlly(); // Pre-emptively avoid within trigger radius.

	//void SetPursuitTarget(O pursuitTarget);
	//void Pursue(); // Pursue the "PursuitTarget"

	//void SetFleeTarget(O fleeTarget);
	//void Flee(); // Move away from the "FleeTarget"
}
