using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour 
{

	public bool isFaded;
	public bool isFading;
	public bool fadingFrom;
	public bool IsNotFaded;

	public float timeCounter;

	public bool fadeToSceneChange;
	public GameObject canvas;

	public GameObject TransitionScreen;
	public Image Screen_Img;
	public Color Screen_Color;

	public int transitionSceneID;

	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.FindGameObjectWithTag("UI");

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
		if(PlayerPrefs.GetInt("fadedFrom",0) == 0) PlayerPrefs.SetInt("fadedFrom", 0);
		if(PlayerPrefs.GetInt("fadedFrom") == 1) FadeFrom();
		else 
		{
			Screen_Color.a = 0;
			TransitionScreen.SetActive(false);
		}
		Screen_Img.color = Screen_Color;
		
	


	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(isFading)
		{
			timeCounter += Time.deltaTime;
			Screen_Color.a = timeCounter * 0.5f;
			Screen_Img.color = Screen_Color;
			//Debug.Log("Is fading");
			if(Screen_Color.a >= 1)
			{
				isFaded = true;
				Screen_Color.a = 1;
				isFading = false;	
			}
		}
		if(fadeToSceneChange && isFaded)
		{
			//Debug.Log("scene change");
			PlayerPrefs.SetInt("fadedFrom", 1);
			if(Screen_Color == Color.black) PlayerPrefs.SetInt("fadeisBlack", 1);
			else PlayerPrefs.SetInt("fadeisBlack", 0);
			SceneManager.LoadScene(transitionSceneID);
		}
		if(fadingFrom)
		{
			
			Screen_Color.a -= Time.deltaTime * 0.5f;
			Screen_Img.color = Screen_Color;
			//Debug.Log("Is fading");
			if(Screen_Color.a <= 0)
			{
				
				Screen_Color.a = 0;
				fadingFrom = false;	
				TransitionScreen.SetActive(false);
			}
		}
	}
	public void FadeTo(Color color)
	{
		isFaded = false;
		isFading = true;
		Screen_Color = color;
		Screen_Color.a = 0;
		Screen_Img.color = Screen_Color;
		TransitionScreen.SetActive(true);

		
	}

	public void FadeFrom()
	{
		fadingFrom = true;
		isFaded = false;
		isFading = false;
		Screen_Color.a = 1;
		Screen_Img.color = Screen_Color;
		TransitionScreen.SetActive(true);

		
	}

	public void FadeToSceneChange(bool fadeColor, int sceneNum)
	{
		//Debug.Log("FadeToSceneChanged" + fadeColor + sceneNum);
		if(fadeColor == false)
		{			
			FadeTo(Color.black);
			//Debug.Log("Fade To Black");
			fadeToSceneChange = true;
			transitionSceneID = sceneNum;
		}
		else
		{
			FadeTo(Color.white);
			//Debug.Log("Fade To white");
			fadeToSceneChange = true;
			transitionSceneID = sceneNum;
		}
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("fadeisBlack", 1); //This is to reset the two transition "booleans" upon exitting the game to avoid weird shit.
		PlayerPrefs.SetInt("fadedFrom",0);
	}
}
