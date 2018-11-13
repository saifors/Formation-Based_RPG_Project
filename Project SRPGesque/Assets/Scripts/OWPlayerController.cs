using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWPlayerController : MonoBehaviour
{

    public Vector2 axis;
    public enum FacingDirection { North, NorthWest, West, SouthWest, South, SouthEast, East, NorthEast};
    public FacingDirection facing;
    Quaternion targetRotation;
    Transform trans;

    float directionalAngle;

    // Use this for initialization
    void Start ()
    {
        trans = transform;
    }
	
	// Update is called once per frame
	void Update ()
    {

        DetermineDirection();

        Rotate();

        if (axis.x !=0 || axis.y != 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        }



    }

    void DetermineDirection()
    {
        if (axis.x > 0)
        {
            if (axis.y > 0)
            {
                facing = FacingDirection.NorthWest;
                directionalAngle = 90;
            }
            else if (axis.y < 0)
            {
                facing = FacingDirection.SouthWest;
                directionalAngle = 180;
            }
            else
            {
                facing = FacingDirection.West;
                directionalAngle = 135;
            }
        }
        else if (axis.x < 0)
        {
            if (axis.y > 0)
            {
                facing = FacingDirection.NorthEast;
                directionalAngle = 0;
            }
            else if (axis.y < 0)
            {
                facing = FacingDirection.SouthEast;
                directionalAngle = 270;
            }
            else
            {
                facing = FacingDirection.East;
                directionalAngle = 315;
            }
        }
        else
        {
            if (axis.y > 0)
            {
                facing = FacingDirection.North;
                directionalAngle = 45;
            }
            else if (axis.y < 0)
            {
                facing = FacingDirection.South;
                directionalAngle = 225;
            }
        }
    }

    public void SetAxis(Vector2 inputAxis)
    {
        axis = inputAxis;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, directionalAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }


}
