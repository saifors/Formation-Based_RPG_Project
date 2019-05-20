using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingBehaviourBase : MonoBehaviour
{

	//protected Animation anim;
	protected Renderer rend;

	protected int distance; //Near far and very far = 0, 1 and 2
	protected CullingGroupBase objCulling;

	protected virtual void Start()
	{
		rend = GetComponent<Renderer>();
		//anim = GetComponent<Animation>();
		objCulling = GameObject.FindGameObjectWithTag("CullingManager").GetComponent<CullingGroupBase>();

		//distance = objCulling.distances;
	}

	public void HasBecomeInvisible()
	{
		//anim.enabled = false;
		rend.enabled = false;
	}
	public void HasBecomeVisible()
	{
		//anim.enabled = true;
		rend.enabled = true;
	}
	public virtual void IsInView()
	{
		//anim.enabled = true;
		//distance = 0;
		rend.enabled = true;

	}
	public virtual void IsOutOfView()
	{
		//distance = 1;
		rend.enabled = false;
	}

	/*
	public virtual void IsNear()
	{
		//anim.enabled = true;
		distance = 0;
		rend.enabled = true;

	}
	public virtual void IsFar()
	{
		//anim.enabled = true;
		distance = 1;

		rend.enabled = true;
	}
	public virtual void IsVeryFar()
	{
		//anim.enabled = false;
		distance = 2;
		rend.enabled = true;
	}
	public virtual void IsExtremelyFar()
	{
		distance = 3;
		rend.enabled = false;
	}*/
}
