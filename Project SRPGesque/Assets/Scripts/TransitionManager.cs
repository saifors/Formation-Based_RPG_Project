using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransitionManager : MonoBehaviour 
{

	[HideInInspector] public bool isFaded;
	[HideInInspector] public bool isFading;
	[HideInInspector] public bool fadingFrom;
	[HideInInspector] public bool IsNotFaded;

	private float timeCounter;

	[HideInInspector] public bool fadeToSceneChange;
	private GameObject canvas;

	private GameObject TransitionScreen;
	private Image Screen_Img;
	public Color Screen_Color;

	public float fadeSpeed;

	private int transitionSceneID;
	public GameData gameData;

	// Use this for initialization
	void Start () 
	{
		
		canvas = GameObject.FindGameObjectWithTag("UI");
		fadeSpeed = 0.5f;

		TransitionScreen = new GameObject("transitionScreen");
		TransitionScreen.AddComponent<RectTransform>();
		RectTransform Screen_Trans = TransitionScreen.GetComponent<RectTransform>();		
		Screen_Trans.parent = canvas.transform;
		Screen_Trans.localPosition = Vector2.zero;
		Screen_Trans.sizeDelta = new Vector2(1920,1080);

		TransitionScreen.AddComponent<Image>();
		Screen_Img = TransitionScreen.GetComponent<Image>();
		if(PlayerPrefs.GetInt("fadeisBlack", 1) == 0) Screen_Color = Color.white; 		// ABSOLUTE SHIT, PlayerPrefs are stored in memory so it doesn´t reset each time you restart the game.
		else Screen_Color = Color.black; 		
		if(PlayerPrefs.GetInt("fadedFrom", 0) == 0) PlayerPrefs.SetInt("fadedFrom", 0);
		if(PlayerPrefs.GetInt("fadedFrom") == 1) FadeFrom();
		else 
		{
			Screen_Color.a = 0;
			TransitionScreen.SetActive(false);
		}
		Screen_Img.color = Screen_Color;
		
	


	}
	
	public void FadeTo(Color color)
	{
		if (isFading) return;

		//Debug.Log("fadeTo");
		isFading = true;
		fadeSpeed = 0.5f;
		Screen_Color = color;
		Screen_Color.a = 0;
		Screen_Img.color = Screen_Color;
		Screen_Img.DOFade(1, fadeSpeed).OnComplete(FinishFadeTo);
		TransitionScreen.SetActive(true);
		
	}

	public void FadeTo(Color color, float speed)
	{
		if (isFading) return;

		//Debug.Log("fadeTo");
		isFading = true;
		fadeSpeed = speed;
		Screen_Color = color;
		Screen_Color.a = 0;
		Screen_Img.color = Screen_Color;

		Screen_Img.DOFade(1, fadeSpeed).OnComplete(FinishFadeTo);
		TransitionScreen.SetActive(true);

	}

	public void FinishFadeTo()
	{
		if(fadeToSceneChange)
		{
			PlayerPrefs.SetInt("fadedFrom", 1);
			
			SceneManager.LoadScene(transitionSceneID);
		}
		else
		{
			
		}
	}

	public void FadeFrom()
	{

		Debug.Log("fadeFrom");
		Screen_Color.a = 1;
		Screen_Img.color = Screen_Color;
		Screen_Img.DOFade(0, fadeSpeed).OnComplete(FinishFadeFrom);
		TransitionScreen.SetActive(true);
	}

	public void FinishFadeFrom()
	{
		TransitionScreen.SetActive(false);
		isFading = false;
	}

	public void FadeToSceneChange(bool fadeColor, int sceneNum)
	{
		fadeToSceneChange = true;
		if(sceneNum >= 3) gameData.Misc.mapID = sceneNum - 3;
		transitionSceneID = sceneNum;
		//Debug.Log("FadeToSceneChanged" + fadeColor + sceneNum);
		if (fadeColor == false)
		{
			FadeTo(Color.black);
			PlayerPrefs.SetInt("fadeisBlack", 1);
		}

		else
		{
			FadeTo(Color.white);
			PlayerPrefs.SetInt("fadeisBlack", 0);
		}
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("fadeisBlack", 1); //This is to reset the two transition "booleans" upon exitting the game to avoid weird shit.
		PlayerPrefs.SetInt("fadedFrom",0);
	}
}
