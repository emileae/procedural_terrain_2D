using UnityEngine;
using System.Collections;
using UnityEditor;

public class NPCController : MonoBehaviour {

	public float speed;
	public int direction = 1;
	public float maxSpeed = 10f;
	public float idleSpeed = 3.0f;
	public int idleDirection = 1;

	private bool facingRight = true;
	private Animator anim;
	private Rigidbody2D rBody;

	// jumping/falling
//	private bool climbing = false;
//	public Transform climbCheck;
//	private float climbRadius = 0.2f;
//	public LayerMask whatIsClimbable;
//	public float releaseForce = 100;

	public bool stop = false;

	private bool grounded = false;
	public Transform groundCheck;
	private float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 700;

	// NPC targeting and movement
	public float hitRadius = 1.1f;
	public Vector3 target;
	public bool goToTarget = false;

	// check for drops
	private float dropRadius = 1.5f;

	// states
	// state = 0 -> idle, available
	// state = 1 -> moving to a location, unavailable
	// state = 2 -> building, unavailable
	public int state = 0;// give states numbers 0 = idle = available

	// working
	private IEnumerator workingCoroutine;

	// building
	public GameObject targetGameObject;

	// Use this for initialization
	void Start ()
	{
		rBody = GetComponent<Rigidbody2D> ();
//		anim = GetComponent<Animator>();

		// vary stopping distance slightly
		hitRadius += Random.Range (0.0f, 1.0f);
		// vary maxSpeed slightly
		maxSpeed += Random.Range (-1.5f, 1.5f);
		idleSpeed += Random.Range (-1.5f, 1.5f);

		// randomise idle direction
		if (Random.Range (0, 1f) > 0.5) {
			idleDirection = 1;
		} else {
			idleDirection = -1;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		// check if grounded
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
//		anim.SetBool ("Ground", grounded);

		// check if climbing
//		climbing = Physics2D.OverlapCircle (climbCheck.position, climbRadius, whatIsClimbable);

//		anim.SetFloat ("vSpeed", rBody.velocity.y);

//		float move = Input.GetAxis ("Horizontal");

//		anim.SetFloat ("Speed", Mathf.Abs (move));

//		if (climbing) {
//			Debug.Log ("CLIMBING................");
//			rBody.velocity = new Vector2 (move * maxSpeed, -rBody.velocity.y);
//		} else {
//			rBody.velocity  =new Vector2(move * maxSpeed,rBody.velocity.y);
//		}

//		if (state == 0) {
//			Idle ();
//		}
//
//		if (goToTarget && !stop) {
//			if (transform.position.x < target.x) {
//				direction = 1;
//			} else if (transform.position.x > target.x) {
//				direction = -1;
//			}
//			speed = maxSpeed;
//		} else {
//			if (stop) {
//				direction = 0;
//			} else {
//				Idle();
//			}
//		}

		switch (state)
		        {
		        case 0:
					Idle ();
		            break;
		        case 1:
					if (goToTarget && !stop) {
						if (transform.position.x < target.x) {
							direction = 1;
						} else if (transform.position.x > target.x) {
							direction = -1;
						}
						speed = maxSpeed;
					}
		            break;
				case 2:
					direction = 0;
		            break;
		        default:
					Idle ();
		            break;
		        }

		if (direction == 0) {
			Debug.Log("Constrain the x position");
			rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		} else {
			rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		rBody.velocity = new Vector2 (direction * speed, rBody.velocity.y);


		if (rBody.velocity.x > 0 && !facingRight) {
			Flip ();
		} else if (rBody.velocity.x < 0 && facingRight) {
			Flip ();
		}
	}

	void Idle(){
		direction = idleDirection;
		speed = idleSpeed;
	}

	public void ReachedShoreLine ()
	{
		stop = true;
		// if idling then reverse direction
		if (state == 0) {
			idleDirection = -idleDirection;
			stop = false;
		}
	}
	public void UnStopNPC(){
		stop = false;
	}

	// put inputs here because if we use Fixedupdate, we may miss an input
	void Update ()
	{
//		if (grounded && Input.GetButtonDown ("Jump")) {
////			anim.SetBool ("Ground", false);
//			rBody.AddForce (new Vector2 (0, jumpForce));
//		}

//		if (climbing) {
//			Debug.Log("CLIMBING................");
//		}


		// call NPCs
//		bool call = Input.GetButton ("Fire3");
//
//		if (call) {
//			Debug.Log("Come to: " + transform.position);
//		}

		// going to a target
		if (goToTarget) {
			if (transform.position.x < (target.x + hitRadius) && transform.position.x > (target.x - hitRadius)) {
				goToTarget = false;
				// return to idle state
				state = 0;
//				target = null;
			}
		}
		
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void GoToLocation(Vector3 position){
		target = position;
		stop = false;
		goToTarget = true;
		state = 1;
	}

	public void StartBuilding(float buildTime){
		workingCoroutine = WorkingAnimation(buildTime);
		StartCoroutine(workingCoroutine);
	}

	public void StopBuilding ()
	{
		StopCoroutine(workingCoroutine);
		Debug.Log("? stopped coroutine?");
	}

	IEnumerator WorkingAnimation (float seconds)
	{
		if (state == 2) {
			Debug.Log("Show building animation");
		}
		yield return new WaitForSeconds (seconds);

		// notify that work is done
		if (state == 2) {
			Building buildScript = targetGameObject.GetComponent<Building>();
			buildScript.FinishedBuilding();
		}

		// return to idle state after work
		if (targetGameObject != null) {
			targetGameObject = null;
		}
		state = 0;
	}

}