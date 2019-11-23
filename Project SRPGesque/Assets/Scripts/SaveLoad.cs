using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
	public SaveFile[] files;
	private TransitionManager transition;
	string cacheName = "spelQuick.od";
	
	// Start is called before the first frame update
    void Start()
    {
		transition = GameObject.FindGameObjectWithTag("Manager").GetComponent<TransitionManager>();
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Init(this);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void LoadFile(int fileNum)
	{
		if (files[fileNum].exists) return;

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

		GameData gameData = files[fileNum].gameData;
		GameDataManager.Save(gameData, cacheName);
		int cacheGameSceneID = 3 + gameData.Misc.mapID;
		transition.FadeToSceneChange(false, cacheGameSceneID);
		//
	}
}
