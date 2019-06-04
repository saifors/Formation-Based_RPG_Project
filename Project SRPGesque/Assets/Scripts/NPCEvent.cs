using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvent : MonoBehaviour
{
	public Animator anim;

	// Start is called before the first frame update
    public void Init()
    {
		anim = GetComponent<Animator>();

		anim.Play("BattleIdle");
    }
	
}
