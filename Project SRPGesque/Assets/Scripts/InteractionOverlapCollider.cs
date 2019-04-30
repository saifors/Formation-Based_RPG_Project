using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOverlapCollider : MonoBehaviour
{
	private OWPlayerController playerController;
	private GameManager gameManager;

	public Collider[] hitColliders;

	public Vector3 boxSize;
	public Vector3 offset;
	private bool overlap;
	public LayerMask mask;
	
	public void Init(OWPlayerController pC, GameManager gM)
    {
		playerController = pC;
		gameManager = gM;
		
	}

	private void FixedUpdate()
	{
		/*overlap = false;
		GetBox();*/
	}
	public Collider[] GetBox()
	{
		hitColliders = Physics.OverlapBox(transform.position + transform.right * offset.x + transform.up * offset.y + transform.forward * offset.z, boxSize / 2, transform.rotation, mask);

		if (hitColliders.Length > 0)
		{
			overlap = true;
			/*
			for (int i = 0; i < cols.Length; i++)
			{
				Debug.Log(cols[i].name);
			}*/
			return hitColliders;
		}
		else
		{
			overlap = false;
			return null;
		}

	}
	private void OnDrawGizmos()
	{
		Matrix4x4 def = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.right * offset.x + transform.up * offset.y + transform.forward * offset.z, transform.rotation, boxSize);
		if (overlap) Gizmos.color = Color.cyan;
		else Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		Gizmos.matrix = def;
	}
}
