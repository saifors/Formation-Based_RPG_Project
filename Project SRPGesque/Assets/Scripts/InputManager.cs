using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputFlag : byte
    {
        Null = 0, // 0000 0000
        N = 1, // 0000 0001
        W = 2, // 0000 0010
        S = 4, // 0000 0100
        E = 8 // 0000 1000
    }
public class InputManager : MonoBehaviour
{

    private OWPlayerController playerController;
    public Vector2 inputAxis;
    public Vector2 previousAxis;

    
    private InputFlag inputFlag;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
    }

    void Update()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        if ((inputAxis.x < previousAxis.x && previousAxis.x > 0) || (inputAxis.x > previousAxis.x && previousAxis.x < 0))
        {
            inputAxis.x = 0;
        }
        if(inputAxis.x > 0)
        {
            inputFlag |= InputFlag.N;
        }
        else inputFlag ^= InputFlag.N;
        if (inputAxis.x < 0)
        {
            inputFlag |= InputFlag.S;
        }
        else inputFlag ^= InputFlag.S;

        inputAxis.y = Input.GetAxis("Vertical");
        if ((inputAxis.y < previousAxis.y && previousAxis.y > 0) || (inputAxis.y > previousAxis.y && previousAxis.y < 0))
        {
            inputAxis.y = 0;
        }
        if (inputAxis.y > 0)
        {
            inputFlag |= InputFlag.W;
        }
        else inputFlag ^= InputFlag.W;
        if (inputAxis.y < 0)
        {
            inputFlag |= InputFlag.E;
        }
        else inputFlag ^= InputFlag.E;

        playerController.SetAxis(inputAxis);
        playerController.DetermineDirection(inputFlag);
        previousAxis = inputAxis;
    }


}

