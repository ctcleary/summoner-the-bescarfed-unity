using UnityEngine;
using System.Collections;

public interface IStatusEffectable {
	
	void AddStatusEffect(StatusEffectKind status);
	void RemoveStatusEffect(StatusEffectKind status);
}
