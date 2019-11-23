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
		//Debug.Log("Yo");
		if (GameDataManager.ExistsGame(fileName))
		{
			exists = true;
			empty.SetActive(false);
			existingFile.SetActive(true);
			gameData = GameDataManager.Load(fileName);

			saveName.text = gameData.Misc.saveName;

			gameData.Misc.playtimeSeconds = 2354523;

			int hours = Mathf.FloorToInt(gameData.Misc.playtimeSeconds / 3600);
			int minutes = Mathf.FloorToInt((gameData.Misc.playtimeSeconds % 3600) / 60);
			int seconds = Mathf.FloorToInt((gameData.Misc.playtimeSeconds % 3600) % 60);
			playtime.text = "Playtime: " + hours + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

			location.text = "Location: " + LanguageManager.langData.mapNames[gameData.MapEncounterCollection[gameData.Misc.mapID].nameKey]; 

			if (gameData.Misc.partyMembers.Count < 3)
			{
				for (int i = 0; i < gameData.Misc.partyMembers.Count; i++)
				{
					portraits[i].sprite = portSprites[gameData.Misc.partyMembers[i]];
					portraits[i].color = Color.white;
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					portraits[i].sprite = portSprites[gameData.Misc.partyMembers[i]];
					portraits[i].color = Color.white;
				}
			}
		}
		else
		{
			exists = false;
			empty.SetActive(true);
			existingFile.SetActive(false);
		}
    }
}
