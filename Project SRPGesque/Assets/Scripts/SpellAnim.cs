using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnim : MonoBehaviour
{
	public Transform trans;
	public ParticleSystem[] particles;

	// Start is called before the first frame update
    public void Init()
    {
		trans = transform;
		particles = GetComponentsInChildren<ParticleSystem>();
		
    }
}
