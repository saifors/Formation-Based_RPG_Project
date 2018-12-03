using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour 
{

	public bool isFaded_B;
	public bool isFaded_W;
	public bool isFading_B;
	public bool isFading_W;
	public bool fadingFrom_B;
	public bool fadingFrom_W;
	public bool IsNotFaded;
	public float timeCounter;
	public bool isCounting;

	public bool fadeToSceneChange;
	public GameObject canvas;

	public GameObject blackTransitionScreen;
	public GameObject whiteTransitionScreen;
	Image blackScreen_Img;
	Color blackScreen_Color;
	Image whiteScreen_Img;
	Color whiteScreen_Color;

	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.FindGameObjectWithTag("UI");

		blackTransitionScreen = new GameObject("blackScreen");
		blackTransitionScreen.AddComponent<RectTransform>();
		RectTransform blackScreen_Trans = blackTransitionScreen.GetComponent<RectTransform>();		
		blackScreen_Trans.parent = canvas.transform;
		blackScreen_Trans.localPosition = Vector2.zero;
		blackScreen_Trans.sizeDelta = new Vector2(643,362);
		blackTransitionScreen.AddComponent<Image>();
		blackScreen_Img = blackTransitionScreen.GetComponent<Image>();
		blackScreen_Color = Color.black; 		
		blackScreen_Color.a = 0;
		blackScreen_Img.color = blackScreen_Color;
		blackTransitionScreen.SetActive(false);

		whiteTransitionScreen = new GameObject("whiteScreen");
		whiteTransitionScreen.AddComponent<RectTransform>();
		RectTransform whiteScreen_Trans = whiteTransitionScreen.GetComponent<RectTransform>();		
		whiteScreen_Trans.parent = canvas.transform;
		whiteScreen_Trans.localPosition = Vector2.zero;
		whiteScreen_Trans.sizeDelta = new Vector2(643,362);
		whiteTransitionScreen.AddComponent<Image>();
		whiteScreen_Img = whiteTransitionScreen.GetComponent<Image>();
		whiteScreen_Color = Color.white; 		
		whiteScreen_Color.a = 0;
		whiteScreen_Img.color = blackScreen_Color;
		whiteTransitionScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isCounting)
		{
			timeCounter += Time.deltaTime;
		}
		if(isFading_B)
		{
			blackScreen_Color.a += 0.3f * timeCounter;
			if(blackScreen_Color.a >= 1)
			{
				isFaded_B = true;
				blackScreen_Color.a = 1;
				isFading_B = false;
				isCounting = false;
			}
		}
		if(fadeToSceneChange && isFaded_B)
		{

		}
	}
	public void FadeToBlack()
	{
		isFaded_B = false;
		isFading_B = true;
		blackTransitionScreen.SetActive(true);

		isCounting = true;
	}
	public void FadeToWhite()
	{
		isFaded_W = false;
		isFading_W = true;
		whiteTransitionScreen.SetActive(true);

		isCounting = true;
	}
	public void FadeFromBlack()
	{
		fadingFrom_B = true;

		isCounting = true;
	}
	public void FadeFromWhite()
	{
		fadingFrom_W = true;

		isCounting = true;
	}
	public void FadeToSceneChange(bool fadeColor, int sceneNum)
	{
		if(fadeColor = false)
		{
			FadeToBlack();
			SceneManager.LoadScene(sceneNum);
		}
		else
		{
			FadeToWhite();
			SceneManager.LoadScene(sceneNum);
		}
	}
}
