using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWPlayerController : PhysicsCollision
{
	
	private Animator anim;
	private GameManager gameManager;
	public InteractionOverlapCollider overlap;

	public float timeCounter;

	[Header("Input")]
    public Vector2 axis;
    private Vector2 previousAxis;
	
	public enum FacingDirection { North, NorthWest, West, SouthWest, South, SouthEast, East, NorthEast};
    [Header("Direction")]
	public FacingDirection facing;
    Quaternion targetRotation;
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
    Vector3 movement_N = new Vector3(1,0,1);
    Vector3 movement_NW = new Vector3(1,0,0);
    Vector3 movement_NE = new Vector3(0,0,1);
    Vector3 movement_S = new Vector3(-1,0,-1);
    Vector3 movement_SW = new Vector3(0,0,-1);
    Vector3 movement_SE = new Vector3(-1,0,0);
    Vector3 movement_W = new Vector3(1,0,-1);
    Vector3 movement_E = new Vector3(-1,0,1);
	#endregion

	[Header("Movement State")]
	public Vector3 movementIndicator;
    private float speed;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed = 20;
    public bool isMoving;
    public bool isRunning;
	bool isRotating;// make it so it can't move when rotating and only starts moving when completed rotation.


	// Use this for initialization
	protected override void Start ()
    {
		base.Start();
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		overlap = GetComponent<InteractionOverlapCollider>();
		overlap.Init(this, gameManager);
        anim = GetComponentInChildren<Animator>();
    }

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		/*if (jump)
		{
			rb.velocity = new Vector3(rb.velocity.x, 0, 0);
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			jump = false;
		}*/

		if (gameManager.gameState == GameManager.GameState.Overworld)
		{
			if ((axis.x != 0 || axis.y != 0)) // How to minimize slide (Input Lag?)?
			{
				/*trans.position*/
				rb.velocity = movementIndicator * speed /** Time.deltaTime*/;
				isMoving = true;
			}
			else
			{
				rb.velocity = movementIndicator * 0;
				isMoving = false;
			}

			DetermineDirection();
			previousAxis = axis; //Store axis of last frame
		}
		else
		{
			rb.velocity = movementIndicator * 0;
			isMoving = false;
		}
							 //rb.velocity = new Vector3(speed, rb.velocity.y, 0); //Unity APi says don´t do this, teacher says do this, teacher is smart, be like teacher.
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
				Flip();
            }
            else isRotating = false;

            Rotate();
			

			//Still has some kinks to work out
			/*if ((axis.x != 0 || axis.y != 0) ) // How to minimize slide (Input Lag?)?
            {
                trans.position += movementIndicator * speed * Time.deltaTime;
                isMoving = true;
            }
            else isMoving = false;*/


			/*DetermineDirection();
            previousAxis = axis; //Store axis of last frame*/
		}
		//Animation

		if (isMoving == false)
		{
			anim.Play("Idle");
		}
		else if (isMoving == true && isRunning == false)
		{
			anim.Play("Walk");

		}
		else if (isMoving == true && isRunning == true)
		{
			anim.Play("Run");
		}
		
		//-------------Footstep SFX
		if (timeCounter < 1) timeCounter += Time.deltaTime;

		if(isMoving)
		{
			if (!isRunning)
			{
				if (timeCounter >= 0.5)
				{
					int footStepSFX = Random.Range(8, 12);
					gameManager.soundPlayer.PlaySound(footStepSFX, true);
					timeCounter = 0;
				}
			}
			else
			{
				if (timeCounter >= 0.35f)
				{
					int footStepSFX = Random.Range(8, 12);
					gameManager.soundPlayer.PlaySound(footStepSFX, true);
					timeCounter = 0;
				}
			}
		} 
		//------
    }

	public void OnTriggerEnter(Collider collision)
	{
		
		if (collision.gameObject.tag == "Event")
		{
			//Debug.Log("Help me end it all");
			EventScript eventScript = collision.gameObject.GetComponent<EventScript>();
			if(eventScript.eventType == EventManager.TypeOfEvent.Range)
			{
				gameManager.eventManager.EnterEventRange(eventScript);
			}
			
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
