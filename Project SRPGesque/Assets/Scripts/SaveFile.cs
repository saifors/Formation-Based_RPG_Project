using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveFile : MonoBehaviour
{
	public SaveLoad saveLoad;
	public GameObject empty;
	public GameObject existingFile;

	public GameData gameData;

	//public GameData gameData;

	public TextMeshProUGUI saveName;
	public TextMeshProUGUI location;
	public TextMeshProUGUI playtime;

	public Image[] portraits;
	public Sprite[] portSprites;

	public string fileName;

	public bool exists;

	// Start is called before the first frame update
    public void Init(SaveLoad SLoad)
    {
		saveLoad = SLoad;
		
		if (!GameDataManager.ExistsGame(fileName)) return;
		gameData = GameDataManager.Load(fileName);
		UpdateInfo();
    }
	public void UpdateInfo()
	{
		if (GameDataManager.ExistsGame(fileName))
		{
			exists = true;
			empty.SetActive(false);
			existingFile.SetActive(true);
			//gameData = GameDataManager.Load(fileName);

			saveName.text = gameData.Misc.saveName;

			//gameData.Misc.playtimeSeconds = 2354523;
			float pS = gameData.Misc.playtimeSeconds;

			int hours = Mathf.FloorToInt(pS / 3600);
			int minutes = Mathf.FloorToInt((pS % 3600) / 60);
			int seconds = Mathf.FloorToInt((pS % 3600) % 60);
			playtime.text = "Playtime: " + hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

			location.text = "Location: " + LanguageManager.langData.mapNames[gameData.MapEncounterCollection[gameData.Misc.mapID].nameKey];

			if (gameData.Misc.partyMembers.Count < 3)
			{
				for (int i = 0; i < gameData.Misc.partyMembers.Count; i++)
				{
					portraits[i].sprite = portSprites[gameData.Party[i].portraitId];
					portraits[i].color = Color.white;
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					portraits[i].sprite = portSprites[gameData.Party[i].portraitId];
					portraits[i].color = Color.white;
				}
			}
			Debug.Log("Updated Save " + gameData.Misc.saveName);
		}
		else
		{
			exists = false;
			empty.SetActive(true);
			existingFile.SetActive(false);
		}
	}
}
