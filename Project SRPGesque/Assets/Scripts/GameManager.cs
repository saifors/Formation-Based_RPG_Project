using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public bool debug;
	public float timeCounter;
	public Transform cam_T;
	public OWPlayerController playerController;
    public enum CameraSetting { OverworldCam, BattleCam, CutsceneCam}; public CameraSetting camSet;
    public enum GameState { Overworld, Battle, GameMenu};
    public GameState gameState;
	public bool randomEcountersOn;
	public float encounterMinimumPercent = 100;
	public BattleUI battleUI;
	public GameObject BattleMenu;
	public GameObject debugMenu;
	public GameObject BattleCharacterPrefab;

	public int partyMembers;
	private CharacterStats charStats;

    public GameObject[] characters;

	public TileScript tileScript;
    public Transform battlefield;
	
	// Use this for initialization
	void Start () 
	{
		cam_T = GameObject.FindGameObjectWithTag("CamTarget").GetComponent<Transform>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
        gameState = GameState.Overworld;
		battleUI = GameObject.FindGameObjectWithTag("UI").GetComponent<BattleUI>();
		charStats = GetComponent<CharacterStats>();

		tileScript = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileScript>();
        battlefield = GameObject.FindGameObjectWithTag("Battlefield").GetComponent<Transform>();

        randomEcountersOn = true;//Depending on the area.
		partyMembers = 1;
        characters = new GameObject[partyMembers];

		charStats.CreateCharacterStats(0, 10, 10, 5, 3, 2, 4); //PLACHEOLDER;
        MoveFormation(0, 36); //PLACEHOLDER
		
	}
	
	
	// Update is called once per frame
	void Update () 
	{
        


		if(gameState == GameState.Overworld)
		{
			if(camSet == CameraSetting.OverworldCam)
			{
				if (playerController.isMoving)
				{
					cam_T.position = playerController.trans.position;
				}
			}
			if(randomEcountersOn)
			{
				if(playerController.isMoving)
				{
					timeCounter += Time.deltaTime;
					if(timeCounter >= 1)
					{
						encounterMinimumPercent -= 2.5f;
						
						if(Random.Range(0 , 100) >= encounterMinimumPercent)
						{
							InitializeEncounter();
							encounterMinimumPercent = 100;
						}
						timeCounter = 0;
					}
					
					
				}
			}

			//Las mierdas de debug.
			if(debug)
			{

			}
		}
		if(gameState == GameState.Battle)
		{
			

			if(debug)
			{

			}
		}
        
	
	}
	public void InitializeEncounter()
	{
		playerController.isMoving = false;
		gameState = GameState.Battle;
		camSet = CameraSetting.BattleCam;
		cam_T.position = battlefield.position;

		for(int i = 0; i < partyMembers;  i++)
		{
			characters[i] = Instantiate(BattleCharacterPrefab);
            characters[i].name = "Battle_Char_" + i;
            characters[i].GetComponent<CharControl_Battle>().Init(i.ToString());
            PlaceCharacterOnTheirTile(i);


        }

		BattleMenu.SetActive(true);
		battleUI.ChangeNotifText("Encountered an enemy!");
	}
	public void EndBattle()
	{
		gameState = GameState.Overworld;
		camSet = CameraSetting.OverworldCam;
		cam_T.position = playerController.trans.position;
		BattleMenu.SetActive(false);
	}

    public void MoveFormation(int charID, int tileID)
    {
        charStats.setTileOccupied(charID, tileID);
        PlaceCharacterOnTheirTile(charID);
    }
    public void PlaceCharacterOnTheirTile(int charID)
    {
        characters[charID].transform.position = tileScript.tileTransform[PlayerPrefs.GetInt(charID + "_TileID") - 1].position;
    }
	public void RunFromBattle()
	{
		if(Random.Range(0,3) >= 2)
		{
			EndBattle();
		}
		else if(debug)
		{
			EndBattle();
		}
		//else failed to run.
		else FailedToRun();
	}

	public void FailedToRun()
	{
		battleUI.ChangeNotifText("Failed to run!");
	}

	public void ToggleDebug()
	{
		debug = !debug;
		if(debug == true) debugMenu.SetActive(true);
		else debugMenu.SetActive(false);
	}

	public void ToggleEncounterRate()
	{
		randomEcountersOn = !randomEcountersOn;
	}

}
