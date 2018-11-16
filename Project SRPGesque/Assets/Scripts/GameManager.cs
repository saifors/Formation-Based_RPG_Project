using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Transform cam_T;
	public OWPlayerController playerController;
    public enum CameraSetting { OverworldCam, BattleCam, CutsceneCam}; public CameraSetting camSet;
	
	// Use this for initialization
	void Start () 
	{
		cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>(); //Ask about whether there's some way for this to workd or whether it's better to split Game manager into OverworldManager and battleManager
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(camSet == CameraSetting.OverworldCam)
        {
            if (playerController.isMoving)
		    {
			    cam_T.position = playerController.trans.position;
		    }
        }
        
		
	}
}
