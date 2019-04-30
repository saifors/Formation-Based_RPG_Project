using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOverlapCollider : MonoBehaviour
{
	private OWPlayerController playerController;
	private GameManager gameManager;

	public Collider[] hitColliders;

	public Vector3 center;
	public Vector3 halfSize;
	public LayerMask mask;
	
	public void Init(OWPlayerController pC, GameManager gM)
    {
		playerController = pC;
		gameManager = gM;

		mask = LayerMask.NameToLayer("Player");
	}

	private void FixedUpdate()
	{
		if (gameManager.gameState == GameManager.GameState.Overworld)
		{
			hitColliders = Physics.OverlapBox(playerController.trans.position + center, halfSize, Quaternion.identity, mask);
		}
	}
	/*
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(playerController.trans.position + center, halfSize * 2);
	}*/
}
