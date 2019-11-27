using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharMenuInfo : MonoBehaviour
{
	public Image portrait;

	public RectTransform[] barSize;
	public TextMeshProUGUI[] barText;

	public TextMeshProUGUI name;
	public TextMeshProUGUI level;

	public Image[] equipmentIcons;
	public TextMeshProUGUI[] equipmentNames;

	public TextMeshProUGUI remainingExp;

	public GameManager gameManager;

	public float maxWidth;

	// Start is called before the first frame update
	public void SetUp(CharacterData data, Sprite[] portSprites, GameManager gM)
    {
		gameManager = gM;
		portrait.sprite = portSprites[data.portraitId];
		barText[0].text = data.currentHp + " / " + data.hp;
		barText[1].text = data.currentMp + " / " + data.mp;
		float hpPercent = (data.currentHp * 100) / data.hp;
		float mpPercent = (data.currentMp * 100) / data.mp;
		Vector2 hpSize;
		Vector2 mpSize;
		hpSize.y = barSize[0].sizeDelta.y;
		mpSize.y = barSize[1].sizeDelta.y;
		hpSize.x = maxWidth * hpPercent / 100;
		mpSize.x = maxWidth * mpPercent / 100;

		barSize[0].sizeDelta = hpSize;
		barSize[1].sizeDelta = mpSize;

		name.text = data.name;
		level.text = "Lv. " + data.level;
		int expToNextLevel = 0;
		for (int i = 0; i < data.level; i++)
		{
			expToNextLevel += gameManager.gameData.LevelRequirement[i].exp;
		}
		expToNextLevel -= data.exp;
		remainingExp.text = "Next Lv. " + expToNextLevel;
	}
	
}
