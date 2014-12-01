using UnityEngine;
using System.Collections;

[System.Serializable]
public class CombatProperties
{
	public float health;
	public float dmgResistance;
	public float attackDmg;
	public float attackSpd;
}

public class CombatModule : MonoBehaviour, INPCModule, IDamageable
{
	
	public CombatProperties combatProperties;
	private Animator animator;

	// Health and Defense props
	private float health;
	private float dmgResistance;

	// Attack props
	private float attackDmg;
	private float attackSpd;
	private float attackDelay;
	//private Timer attackTimer;

	private bool isAlive;
	private IDamageable attackTarget;

	// Fancy shit
	public string attackAnim;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponentInParent<Animator> ();
		isAlive = true;
		Reset (); // Set private variables.
	}
	
	public void Reset ()
	{
		isAlive = true;
		
		// Set to private variables so we can reset based on `combatProperties`
		health = combatProperties.health;
		//dmgResistance = combatProperties.dmgResistance;
		
		attackDmg = combatProperties.attackDmg;
		//attackSpd = combatProperties.attackSpd;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (attackTarget != null) {
			Attack ();
		}
	}

	public void Hurt (float dmgTaken)
	{
		health -= dmgTaken;
		if (health <= 0) {
			isAlive = false;
		}
	}

	public void SetAttackTarget (IDamageable attackTarget)
	{
		// Only set if current target is `null`, or if the assignment is to `null`
		if (this.attackTarget == null || attackTarget == null) {
			this.attackTarget = attackTarget;
		}
	}

	public IDamageable GetAttackTarget ()
	{
		return attackTarget;
	}

	public void Attack ()
	{
		if (attackTarget != null) {
			if (attackAnim != null) {
				animator.Play (attackAnim);
			}
			Invoke("DoDamage", 0.3f);

		} else {
			Debug.Log ("attack target nullified!");
			SetAttackTarget (null);
		}
	}

	private void DoDamage()
	{
		if (attackTarget != null) {

			attackTarget.Hurt (attackDmg);
		}
	}

	public bool IsAlive ()
	{
		return isAlive;
	}
}
