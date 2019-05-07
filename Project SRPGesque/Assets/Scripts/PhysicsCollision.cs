using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsCollision : MonoBehaviour
{
	protected Rigidbody rb;
	public Transform trans;
	[Header("Physics properties")]
	public float gravityMagnitude = 1; //instead of changing gravity for everything in project settings this allows us to so for everything individually.
	[Header("Collision state")]
	public bool isGrounded;
	protected bool wasGrounded;
	protected bool justGrounded;
	protected bool justNotGrounded;

	public bool isTouchingWall;
	protected bool wasTouchingWall;
	protected bool justTouchingWall;
	protected bool justNotTouchingWall;

	public float facingRotation;
	[Header("Ground Collision")]
	public Vector3 gDirection;
	public LayerMask gMask;
	public float gDistance;
	public int gNumRays;
	public float gDistBtwnRays;
	[Header("Wall Collision")]
	public Vector3 wDirection;
	public LayerMask wMask;
	public float wDistance;
	public int wNumRays;
	public float wDistBtwnRays;

	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody>();
		trans = transform;

		facingRotation = trans.rotation.y;
	}

	protected virtual void FixedUpdate()
	{
		rb.AddForce(Physics.gravity * gravityMagnitude, ForceMode.Acceleration); //Gravity is now being done by code Gravity needs to be turned off in the rigidbody now.

		CheckGround();
		CheckWall();


	}

	private void OnDrawGizmos()
	{
		DrawGroundRays();
		DrawWallRays();
	}

	private void CheckGround()
	{
		wasGrounded = isGrounded; //Gets is Groundeds state from the last frame, needs to be above the is Grounded line otherwise it would get the new one.
		isGrounded = false; //It´s false on default unless the Raycast a few lines later tells it that it´s true. 
		justGrounded = false;
		justNotGrounded = false;
		int altNegPosNr = 1;
		Vector3 pos = transform.position;
		for (int i = 0; i < gNumRays; i++) //Makes Rays centered around origin. Making multiple arrays is useful for uneven terrain where one might not be enough.
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = new Ray(pos, gDirection); //Create the Ray
			if (Physics.Raycast(ray, out hit, gDistance, gMask)) // Fires the ray can be put into an if because it returns a true or false.
			{
				if (hit.normal.y > 0.85f)
				{
					isGrounded = true;
					if (!wasGrounded) justGrounded = true; //If he wasn´t grounded before, but now he is, he just now grounded at this moment.
														   //Debug.Log(hit.transform.name);
					break; // Exits the "for" loop because it doesn't have to check any of the others because it already found the ground.
				}

			}
			pos.x = pos.x + (i + 1) * gDistBtwnRays * altNegPosNr;
			altNegPosNr *= -1;
		}
		if (!isGrounded && wasGrounded) justNotGrounded = true; //if he isn´t grounded but he was grounded then he just stopped being grounded.
	}
	private void CheckWall()
	{
		wasTouchingWall = isTouchingWall;
		isTouchingWall = false;
		justTouchingWall = false;
		justTouchingWall = false;
		int altNegPosNr = 1;
		Vector3 pos = trans.position;
		//Debug.Log("CheckWall");
		for (int i = 0; i < wNumRays; i++)
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = new Ray(pos, wDirection);
			if (Physics.Raycast(ray, out hit, wDistance, wMask))
			{
				//Debug.Log("Wall touchy" + Mathf.Abs(hit.normal.z));
				if (Mathf.Abs(hit.normal.z) > 0.85f || Mathf.Abs(hit.normal.x) > 0.85f)
				{
					
					isTouchingWall = true;
					if (!wasTouchingWall) justTouchingWall = true;

					break;
				}


			}
			pos.y = pos.y + (i + 1) * wDistBtwnRays * altNegPosNr;
			altNegPosNr *= -1;
		}
		if (!isTouchingWall && wasTouchingWall) justNotTouchingWall = true;
	}

	protected virtual void Flip()

	{
		//isFacingRight = !isFacingRight;
		facingRotation = trans.rotation.y;

		wDirection = trans.forward; //Quaternion.Euler(new Vector3(0, facingRotation, 0)) * Vector3.forward;

		/*if (isFacingRight) wDirection = Vector3.right;
		else wDirection = Vector3.left;*/
	}



	private void DrawWallRays()
	{
		if (isTouchingWall) Gizmos.color = Color.blue;
		else Gizmos.color = Color.red;
		int altNegPosNr = 1;
		Vector3 pos = transform.position;
		for (int i = 0; i < wNumRays; i++)
		{

			Gizmos.DrawRay(pos, wDirection * wDistance);
			pos.y = pos.y + (i + 1) * wDistBtwnRays * altNegPosNr;
			altNegPosNr *= -1;
		}
	}
	private void DrawGroundRays()
	{
		if (isGrounded) Gizmos.color = Color.blue;
		else Gizmos.color = Color.red;
		int altNegPosNr = 1;
		Vector3 pos = transform.position;
		for (int i = 0; i < gNumRays; i++)
		{

			Gizmos.DrawRay(pos, gDirection * gDistance);
			pos.x = pos.x + (i + 1) * gDistBtwnRays * altNegPosNr;
			altNegPosNr *= -1;
		}
	}
}
