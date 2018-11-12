using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private OWPlayerController playerController;
    public Vector2 inputAxis;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OWPlayerController>();
    }

    void Update()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        playerController.SetAxis(inputAxis);
    }


}

