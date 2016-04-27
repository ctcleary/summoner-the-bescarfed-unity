using UnityEngine;

public enum MessageType
{
	NONE,

    // NPC Messages
    OpponentsChange,
	VisionEnter,

    TargetAcquired,
    MovementAdjustment,
    TargetLost,
	Collided,
	
	FightEngaged,
    Attacking,
	Damaged,
    Healed,
	FightResolved,

	Spawned,
    Faced,
    HealthUpdate,
	Died,
	Respawned,

    // Game Control
    // TODO figure out how to split supported message types
    Paused,
    Lost,
    Muted
}

