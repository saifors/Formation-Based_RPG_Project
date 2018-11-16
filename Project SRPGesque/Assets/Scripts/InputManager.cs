using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{

    private OWPlayerController playerController;
    public Vector2 inputAxis;
    public Vector2 previousAxis;


    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
    }

    void Update()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        /*
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
        */
        inputAxis.y = Input.GetAxisRaw("Vertical");
        /*
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
        */
        playerController.SetAxis(inputAxis);
        previousAxis = inputAxis;
    }


}

