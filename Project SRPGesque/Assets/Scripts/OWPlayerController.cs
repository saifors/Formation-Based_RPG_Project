using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWPlayerController : MonoBehaviour
{

    public Vector2 axis;
    private Vector2 previousAxis;
    public enum FacingDirection { North, NorthWest, West, SouthWest, South, SouthEast, East, NorthEast};
    public FacingDirection facing;
    Quaternion targetRotation;
    public Transform trans;
    public Vector3 movementIndicator;
    private float speed;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed = 20;
    public bool isMoving;
    public bool isRunning;

    private GameManager gameManager;

    bool isRotating;// make it so it can't move when rotating and only starts moving when completed rotation.

    float directionalAngle;
    //Could this be done as an enum?
    #region 
    float angle_N = 45;
    float angle_NW = 90;
    float angle_NE = 0;
    float angle_S = 225;
    float angle_SW = 180;
    float angle_SE = 270;
    float angle_W = 135;
    float angle_E = 315;
    Vector3 movement_N = new Vector3(1,0,1);
    Vector3 movement_NW = new Vector3(1,0,0);
    Vector3 movement_NE = new Vector3(0,0,1);
    Vector3 movement_S = new Vector3(-1,0,-1);
    Vector3 movement_SW = new Vector3(0,0,-1);
    Vector3 movement_SE = new Vector3(-1,0,0);
    Vector3 movement_W = new Vector3(1,0,-1);
    Vector3 movement_E = new Vector3(-1,0,1);
    #endregion

    // Use this for initialization
    void Start ()
    {
        trans = transform;
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManager.isPaused) return;
        if (isRunning) speed = runSpeed;
        else speed = walkSpeed;

        if(gameManager.gameState == GameManager.GameState.Overworld)
        {
            if(trans.rotation != targetRotation)
            {
                isRotating = true;
            }
            else isRotating = false;

            Rotate();
            //Still has some kinks to work out
            if ((axis.x != 0 || axis.y != 0) ) // How to minimize slide (Input Lag?)?
            {
                trans.position += movementIndicator * speed * Time.deltaTime;
                isMoving = true;
            }
            else isMoving = false;


            DetermineDirection();
            previousAxis = axis; //Store axis of last frame
        }
        
    }

    public void DetermineDirection()  
    {
        if (axis.y > 0)
        {
            if (axis.x > 0)
            {
                facing = FacingDirection.NorthWest;
                movementIndicator = movement_NW;
                directionalAngle = angle_NW;
            }
            else if (axis.x < 0)
            {
                facing = FacingDirection.NorthEast;
                movementIndicator = movement_NE;
                directionalAngle = angle_NE;
            }
            else
            {
                facing = FacingDirection.North;
                movementIndicator = movement_N;
                directionalAngle = angle_N;
            }
        }
        else if (axis.y < 0)
        {
            if (axis.x > 0)
            {
                facing = FacingDirection.SouthWest;
                movementIndicator = movement_SW;
                directionalAngle = angle_SW;
            }
            else if (axis.x < 0)
            {
                facing = FacingDirection.SouthEast;
                movementIndicator = movement_SE;
                directionalAngle = angle_SE;
            }
            else
            {
                facing = FacingDirection.South;
                movementIndicator = movement_S;
                directionalAngle = angle_S;
            }
        }
        else if (axis.x > 0)
        {
            facing = FacingDirection.West;
            movementIndicator = movement_W;
            directionalAngle = angle_W;
        }
        else if (axis.x < 0)
        {
            facing = FacingDirection.East;
            movementIndicator = movement_E;
            directionalAngle = angle_E;
        }
    }

    public void SetAxis(Vector2 inputAxis)
    {
        axis = inputAxis;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, directionalAngle, 0);
        trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }


}
