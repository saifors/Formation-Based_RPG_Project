using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiModedCull : CullingBehaviourBase
{

	protected Renderer[] rends;
	protected override void Start()
	{
		base.Start();
		rend = GetComponentInChildren<Renderer>();
		rends = GetComponentsInChildren<Renderer>();
	}
	public override void IsInView()
	{
		base.IsInView();
		for (int i = 0; i < rends.Length; i++) rends[i].enabled = true;
	}
	public override void IsOutOfView()
	{
		base.IsOutOfView();
		for (int i = 0; i < rends.Length; i++) rends[i].enabled = false;
	}
}