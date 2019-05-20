using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCull : MultiModedCull
{
	protected Animator[] anims;
	protected override void Start()
	{
		base.Start();
		anims = GetComponentsInChildren<Animator>();
	}

	public override void IsInView()
	{
		base.IsInView();
		for (int i = 0; i < anims.Length; i++) anims[i].enabled = true;
	}
	public override void IsOutOfView()
	{
		base.IsOutOfView();
		for (int i = 0; i < anims.Length; i++) anims[i].enabled = false;
	}
}
