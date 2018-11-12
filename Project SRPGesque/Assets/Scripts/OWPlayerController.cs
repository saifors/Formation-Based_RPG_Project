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

        if (axis.x !=0 && axis.y != 0)
        {
            switch (facing)
            {
                case FacingDirection.North:
                    trans.position += new Vector3(10,0,10) * Time.deltaTime;
                    break;
                case FacingDirection.NorthWest:
                    trans.position += Vector3.right * Time.deltaTime;
                    break;
                case FacingDirection.West:
                    break;
                case FacingDirection.SouthWest:
                    trans.position += Vector3.back * Time.deltaTime;
                    break;
                case FacingDirection.South:                    
                    break;
                case FacingDirection.SouthEast:
                    trans.position += Vector3.left * Time.deltaTime;
                    break;
                case FacingDirection.East:
                    break;
                case FacingDirection.NorthEast:
                    trans.position += Vector3.forward * Time.deltaTime;
                    break;
                default:
                    break;
            }
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
