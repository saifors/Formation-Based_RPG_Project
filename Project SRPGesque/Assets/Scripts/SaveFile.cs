using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveFile : MonoBehaviour
{
	public GameObject empty;
	public GameObject existingFile;

	public TextMeshProUGUI saveName;
	public TextMeshProUGUI location;
	public TextMeshProUGUI playtime;

	public Image[] portraits;
	public Sprite[] portSprites;

	public string fileName;

	// Start is called before the first frame update
    void Start()
    {
		//Debug.Log("Yo");
		if (GameDataManager.ExistsGame(fileName))
		{
			empty.SetActive(false);
			existingFile.SetActive(true);
			GameData gameData = GameDataManager.Load(fileName);

			saveName.text = gameData.Misc.saveName;
			playtime.text = "Playtime: " + gameData.Misc.playtimeSeconds;
			location.text = "Location: " + gameData.Misc.mapID;

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
			empty.SetActive(true);
			existingFile.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
