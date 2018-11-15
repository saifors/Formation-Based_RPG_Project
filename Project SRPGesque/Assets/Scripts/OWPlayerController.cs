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
    public float speed;
    public float turnSpeed = 20;
    public bool isMoving;

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
    #endregion

    // Use this for initialization
    void Start ()
    {
        trans = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        
        Rotate();

        if (axis.x != 0 || axis.y != 0) // How to minimize slide (Input Lag?)?
        {
            trans.position += trans.forward * speed * Time.deltaTime;
            isMoving = true;
        }
        else isMoving = false;


        previousAxis = axis; //Store axis of last frame
    }

    public void DetermineDirection(InputFlag inputDirection) //problem is now that due to axis being immediately auto set to 0 when decelerating 
    {
        if ((inputDirection & InputFlag.N) != 0)
        {
            if ((inputDirection & InputFlag.W) != 0)
            {
                facing = FacingDirection.NorthWest;
                directionalAngle = angle_NW;
            }
            else if ((inputDirection & InputFlag.E) != 0)
            {
                facing = FacingDirection.NorthEast;
                directionalAngle = angle_NE;
            }
            else
            {
                facing = FacingDirection.North;
                directionalAngle = angle_N;
            }
        }
        else if ((inputDirection & InputFlag.S) != 0)
        {
            if ((inputDirection & InputFlag.W) != 0)
            {
                facing = FacingDirection.SouthWest;
                directionalAngle = angle_SW;
            }
            else if ((inputDirection & InputFlag.E) != 0)
            {
                facing = FacingDirection.SouthEast;
                directionalAngle = angle_SE;
            }
            else
            {
                facing = FacingDirection.South;
                directionalAngle = angle_S;
            }
        }
        else if ((inputDirection & InputFlag.W) != 0)
        {
            facing = FacingDirection.West;
            directionalAngle = angle_W;
        }
        else if ((inputDirection & InputFlag.E) != 0)
        {
            facing = FacingDirection.East;
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
