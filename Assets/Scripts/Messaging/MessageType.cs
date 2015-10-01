using UnityEngine;

public enum MessageType
{
	NONE,

    // NPC Messages
    OpponentsChange,
	VisionEnter,

	Collided,
    Faced,
	
	FightEngaged,
    Attacking,
	Damaged,
    Healed,
	FightResolved,

	Spawned,
    HealthUpdate,
	Died,
	Respawned
}

