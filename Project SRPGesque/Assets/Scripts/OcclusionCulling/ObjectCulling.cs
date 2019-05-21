using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCulling : CullingGroupBase
{

	CullingBehaviourBase[] cullee;
	// Use this for initialization
	protected override void Start()
	{
		GameObject[] obj = GameObject.FindGameObjectsWithTag("StaticObj");


		cullingObj = new Transform[obj.Length];
		cullee = new CullingBehaviourBase[obj.Length];
		for (int i = 0; i < obj.Length; i++)
		{
			cullingObj[i] = obj[i].transform;
			cullee[i] = obj[i].GetComponent<CullingBehaviourBase>();
		}

		base.Start();

		group.SetDistanceReferencePoint(reference);
		group.SetBoundingDistances(distances);

	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < cullingObj.Length; i++)
		{
			//Debug.Log(i);
			spheres[i].position = cullingObj[i].position;
		}
	}

	protected override void OnStateChanged(CullingGroupEvent sphere)
	{
		//Debug.Log(sphere.index);
		if (sphere.hasBecomeInvisible)
		{
			cullee[sphere.index].HasBecomeInvisible();
		}
		if (sphere.hasBecomeVisible)
		{
			cullee[sphere.index].HasBecomeVisible();
		}


		if (sphere.currentDistance <= 0) cullee[sphere.index].IsInView();
		/*else if (sphere.currentDistance <= 1) cullee[sphere.index].IsFar();
		else if (sphere.currentDistance <= 2) cullee[sphere.index].IsVeryFar();*/
		else cullee[sphere.index].IsOutOfView();
	}
}

