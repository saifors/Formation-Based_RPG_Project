using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Recovery
{
	public enum RecoveryType { FixedHeal, PercentHeal, FixedRecover, PercentRecover, };

	public static void Recover(RecoveryType type, int healValue, CharControl_Battle character)
	{
		switch (type)
		{
			case RecoveryType.FixedHeal:
				character.currentHp += healValue;
				break;
			case RecoveryType.PercentHeal:
				character.currentHp += healValue / 100 * character.hp;
				break;
			case RecoveryType.FixedRecover:
				character.currentMp += healValue;
				break;
			case RecoveryType.PercentRecover:
				character.currentMp += healValue / 100 * character.hp;
				break;
			default:
				break;
		}
		if (character.currentHp > character.hp) character.currentHp = character.hp;
		if (character.currentMp > character.mp) character.currentMp = character.mp;
	}
}
