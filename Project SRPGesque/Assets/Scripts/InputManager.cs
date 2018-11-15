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
        inputAxis.x = Input.GetAxis("Horizontal");
        if ((inputAxis.x < previousAxis.x && previousAxis.x > 0) || (inputAxis.x > previousAxis.x && previousAxis.x < 0))
        {
            inputAxis.x = 0;
        }
            
        inputAxis.y = Input.GetAxis("Vertical");
        if ((inputAxis.y < previousAxis.y && previousAxis.y > 0) || (inputAxis.y > previousAxis.y && previousAxis.y < 0))
        {
            inputAxis.y = 0;
        }
        playerController.SetAxis(inputAxis);
        previousAxis = inputAxis;
    }


}

