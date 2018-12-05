using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen_Script : MonoBehaviour
{
    public GameObject selectionImageGroup;
    public Image[] selectionImage;
    public int titleSelection;
    public Color unselectedColor;
    public float inputAxis;
    public float scrollCooldown = 0.25f;
    public float scrollCooldownCounter;
    public int NewGameSceneID;
    private TransitionManager transition;

    // Use this for initialization
    void Start () {
        selectionImageGroup = GameObject.FindGameObjectWithTag("TitleScreenOptions");
        selectionImage = selectionImageGroup.GetComponentsInChildren<Image>();
        transition = GameObject.FindGameObjectWithTag("Manager").GetComponent<TransitionManager>();

        unselectedColor = new Color(0.66f, 0.66f, 0.66f, 0.75f);

        
        SelectOption(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        inputAxis = Input.GetAxisRaw("Vertical");

        if (scrollCooldownCounter >= scrollCooldown)
        {
            if (inputAxis <= -1)
            {
                if (titleSelection >= selectionImage.Length - 1) SelectOption(0);
                else SelectOption(titleSelection + 1);
                scrollCooldownCounter = 0;
            }
            else if (inputAxis >= 1)
            {
                if (titleSelection <= 0) SelectOption(3);
                else SelectOption(titleSelection - 1);
                scrollCooldownCounter = 0;
            }
        }
        else scrollCooldownCounter += Time.deltaTime;
        if (Input.GetKey(KeyCode.Z)) ConfirmSelection(titleSelection);
    }
        
    
    public void UnselectAll()
    {
        for (int i = 0; i < selectionImage.Length; i++)
        {
            selectionImage[i].color = unselectedColor;
            selectionImage[i].transform.localScale = Vector3.one;
        }
    }

    public void SelectOption(int optionNum)
    {
        titleSelection = optionNum;
        UnselectAll();
        selectionImage[optionNum].color = Color.white;
        selectionImage[optionNum].transform.localScale = Vector3.one * 1.1f;
    }
    public void ConfirmSelection(int optionNum)
    {
        if (optionNum == 0) transition.FadeToSceneChange(false, NewGameSceneID);
    }

}
