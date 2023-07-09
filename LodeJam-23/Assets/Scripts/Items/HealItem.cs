using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : BaseItem
{
	protected override void OnPlayerPickup()
	{
		GameManager.Instance.GetPlayerEntity().Heal(3);
	}

	protected override void OnEffectExpire()
	{
		
	}
}
