using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Transform cam_T;
	public OWPlayerController playerController;
	
	// Use this for initialization
	void Start () 
	{
		cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(playerController.isMoving)
		{
			cam_T.position = playerController.trans.position;
		}
		
	}
}
