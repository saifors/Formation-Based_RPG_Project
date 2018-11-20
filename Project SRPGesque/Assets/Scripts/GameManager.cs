using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Transform cam_T;
	public OWPlayerController playerController;
    public enum CameraSetting { OverworldCam, BattleCam, CutsceneCam}; public CameraSetting camSet;
    public enum GameState { Overworld, Battle, GameMenu};
    public GameState gameState;
	
	// Use this for initialization
	void Start () 
	{
		cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
        gameState = GameState.Overworld;
	}
	//Other possibility (better) have battle and overworld in the same scene and just seperated from each other and only one active at the same time, that way you also wo't have to reload scenes and remember things like player position.
	
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
