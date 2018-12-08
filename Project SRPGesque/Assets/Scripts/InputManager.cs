using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{

    private OWPlayerController playerController;
    private BattleUI battleUI;
    public Vector2 inputAxis;
    public Vector2 previousAxis;
    public GameManager gameManager;


    

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
        battleUI = GameObject.FindGameObjectWithTag("UI").GetComponent<BattleUI>();
        gameManager = GetComponent<GameManager>();
        
       
    }

    void Update()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        
        inputAxis.y = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(KeyCode.LeftShift)) playerController.isRunning = true;
        else playerController.isRunning = false;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (gameManager.gameState == GameManager.GameState.Battle)
            {
                if (battleUI.selecting == BattleUI.SelectingMenu.selectingAction) battleUI.ConfirmSelectedCommand();
                else if(battleUI.selecting == BattleUI.SelectingMenu.selectingMove)
                {
                    gameManager.MoveFormation(0,battleUI.selectedTile);
                }
            }
        }
        else if (Input.GetKey(KeyCode.X)) battleUI.ReturnToCommandSelection();
        
        if(gameManager.gameState == GameManager.GameState.Overworld)
        {
            playerController.SetAxis(inputAxis);
        }
        else if(gameManager.gameState == GameManager.GameState.Battle)
        {
            battleUI.SetAxis(inputAxis);
        }
        
        previousAxis = inputAxis;

        //Konami Code for debug
        if(Input.GetKeyDown(KeyCode.F10)) gameManager.ToggleDebug();
        if(gameManager.debug)
        {
            if(Input.GetKeyDown(KeyCode.B)) gameManager.InitializeEncounter();
            if(Input.GetKeyDown(KeyCode.N)) gameManager.randomEcountersOn = !gameManager.randomEcountersOn;
        }

    }

}

