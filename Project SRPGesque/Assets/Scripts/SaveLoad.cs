using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
	public SaveFile[] files;
	private TransitionManager transition;
	string cacheName = "spelQuick.od";
	public GameManager gameManager;
	
	// Start is called before the first frame update
    void Start()
    {
		transition = GameObject.FindGameObjectWithTag("Manager").GetComponent<TransitionManager>();
		gameManager = transition.GetComponent<GameManager>();
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Init(this);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SaveFile(int fileNum)
	{

		switch (fileNum)
		{
			case 0:
				PlayerPrefs.SetString("CurrentFile", "spelEen.od");
				break;
			case 1:
				PlayerPrefs.SetString("CurrentFile", "spelTwee.od");
				break;
			case 2:
				PlayerPrefs.SetString("CurrentFile", "spelDrie.od");
				break;
			default:
				break;
		}
		
		gameManager.AddMiscToGameData();

		
		GameDataManager.Save(gameManager.gameData, PlayerPrefs.GetString("CurrentFile"));
		

		//GameDataManager.SaveXml(gameManager.gameData, "readabledata.txt");
		files[fileNum].gameData = gameManager.gameData;
		files[fileNum].UpdateInfo();
		

		//Debug.Log("Event " + gameManager.gameData.EventCollection[0].hasBeenTriggered);
		PlayerPrefs.SetString("CurrentFile", cacheName);
		GameDataManager.Save(gameManager.gameData, PlayerPrefs.GetString("CurrentFile"));
	}

	public void LoadFile(int fileNum)
	{
		if (!files[fileNum].exists) return;

		switch (fileNum)
		{
			case 0:
				PlayerPrefs.SetString("CurrentFile", "spelEen.od");
				break;
			case 1:
				PlayerPrefs.SetString("CurrentFile", "spelTwee.od");
				break;
			case 2:
				PlayerPrefs.SetString("CurrentFile", "spelDrie.od");
				break;
			default:
				break;
		}


		GameData gameData = GameDataManager.Load(PlayerPrefs.GetString("CurrentFile"));
		transition.gameData = gameData;
		
		int cacheGameSceneID = 3 + gameData.Misc.mapID;
		//PlayerPrefs.SetString("CurrentFile", cacheName);
		GameDataManager.Save(gameData, PlayerPrefs.GetString("CurrentFile", cacheName));
		transition.FadeToSceneChange(false, cacheGameSceneID);
		//
	}
}
