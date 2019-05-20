using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CullingGroupBase : MonoBehaviour
{

	public float cullingSphereRadius;
	protected CullingGroup group;
	protected BoundingSphere[] spheres;

	public Transform[] cullingObj;
	public float[] distances;
	public Transform reference;
	private int len;

	protected virtual void Start()
	{
		group = new CullingGroup();
		//-----------------------------------For culling depending on Visibility.--------------------------------------
		group.targetCamera = Camera.main;
		spheres = new BoundingSphere[100];

		len = cullingObj.Length;
		if (len > 100) len = 100;
		for (int i = 0; i < len; i++)
		{
			spheres[i] = new BoundingSphere(cullingObj[i].position, cullingSphereRadius);
		}
		group.SetBoundingSpheres(spheres);
		group.SetBoundingSphereCount(len);

		//Callback
		group.onStateChanged = OnStateChanged; //Name is customizable. Calls OnStateChanged everytime visibility is changed from true to false and vice versa

	}
	protected virtual void OnStateChanged(CullingGroupEvent sphere)
	{
		//cullingObj[sphere.index].gameObject.SetActive(sphere.isVisible); //Not recommended o deactivate and reactivate the entire gameobject just a component like Mesh renderer Collider behaviour....
	}
	private void OnDestroy() //Normally we won't have to use this as Unity does that automatically usually.
	{
		group.Dispose();
		group = null;
	}
	void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		Gizmos.color = Color.blue;
		len = cullingObj.Length;

		for (int i = 0; i < len; i++)
		{
			Gizmos.DrawWireSphere(spheres[i].position, spheres[i].radius);
		}
	}
}
