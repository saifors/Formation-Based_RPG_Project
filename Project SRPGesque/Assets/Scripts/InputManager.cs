using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{

    private OWPlayerController playerController;
    private ActionMenu actionMenu;
    public Vector2 inputAxis;
    public Vector2 previousAxis;
    public GameManager gameManager;


    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
        actionMenu = GameObject.FindGameObjectWithTag("UI").GetComponent<ActionMenu>();
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        
        inputAxis.y = Input.GetAxisRaw("Vertical");
        
        if(gameManager.gameState == GameManager.GameState.Overworld)
        {
            playerController.SetAxis(inputAxis);
        }
        else if(gameManager.gameState == GameManager.GameState.Battle)
        {
            actionMenu.SetAxis(inputAxis.y);
        }
        
        previousAxis = inputAxis;
    }


}

