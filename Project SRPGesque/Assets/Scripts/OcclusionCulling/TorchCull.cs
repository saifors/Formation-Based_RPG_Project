using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchCull : AnimatedCull
{
	private ParticleSystem[] particles;
	private Light[] lights;

	protected override void Start()
	{
		base.Start();
		particles = GetComponentsInChildren<ParticleSystem>();
		lights = GetComponentsInChildren<Light>();
	}

	public override void IsInView()
	{
		base.IsInView();
		for (int i = 0; i < particles.Length; i++) particles[i].Play();
		for (int i = 0; i < lights.Length; i++) lights[i].enabled = true;
	}
	public override void IsOutOfView()
	{
		base.IsOutOfView();
		for (int i = 0; i < particles.Length; i++) particles[i].Pause();
		for (int i = 0; i < lights.Length; i++) lights[i].enabled = false;
	}
}
