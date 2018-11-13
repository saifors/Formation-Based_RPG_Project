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
    Transform trans;
    public float speed;

    float directionalAngle;
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

        DetermineDirection();

        Rotate();

        if (axis.x != 0 || axis.y != 0 && (((axis.x >= previousAxis.x && axis.x > 0) || (axis.y >= previousAxis.y && axis.y > 0)) || ((axis.x <= previousAxis.x && axis.x < 0) || (axis.y <= previousAxis.y && axis.y < 0))) ) // How to minimize slide (Input Lag?)?
        {
            trans.position += trans.forward * speed * Time.deltaTime;
        }


        previousAxis = axis; //Store axis of last frame
    }

    void DetermineDirection()
    {
        if (axis.x > 0)
        {
            if (axis.y > 0)
            {
                facing = FacingDirection.NorthWest;
                directionalAngle = angle_NW;
            }
            else if (axis.y < 0)
            {
                facing = FacingDirection.SouthWest;
                directionalAngle = angle_SW;
            }
            else
            {
                facing = FacingDirection.West;
                directionalAngle = angle_W;
            }
        }
        else if (axis.x < 0)
        {
            if (axis.y > 0)
            {
                facing = FacingDirection.NorthEast;
                directionalAngle = angle_NE;
            }
            else if (axis.y < 0)
            {
                facing = FacingDirection.SouthEast;
                directionalAngle = angle_SE;
            }
            else
            {
                facing = FacingDirection.East;
                directionalAngle = angle_E;
            }
        }
        else
        {
            if (axis.y > 0)
            {
                facing = FacingDirection.North;
                directionalAngle = angle_N;
            }
            else if (axis.y < 0)
            {
                facing = FacingDirection.South;
                directionalAngle = angle_S;
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
        trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, 10 * Time.deltaTime);
    }


}
