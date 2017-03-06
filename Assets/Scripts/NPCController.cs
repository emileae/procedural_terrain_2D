using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {

	public float maxSpeed = 10f;

	private bool facingRight = true;
	private Animator anim;
	private Rigidbody2D rBody;

	// jumping/falling
//	private bool climbing = false;
//	public Transform climbCheck;
//	private float climbRadius = 0.2f;
//	public LayerMask whatIsClimbable;
//	public float releaseForce = 100;

	private bool grounded = false;
	public Transform groundCheck;
	private float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 700;

	// NPC targeting and movement
	public float hitRadius = 1.1f;
	public Vector3 target;
	public bool goToTarget = false;

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D>();
//		anim = GetComponent<Animator>();

		hitRadius += Random.Range(0.0f, 1.0f);
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

		if (goToTarget) {
			if (transform.position.x < target.x) {
				rBody.velocity = new Vector2 (1 * maxSpeed, rBody.velocity.y);
			} else if (transform.position.x > target.x) {
				rBody.velocity = new Vector2 (-1 * maxSpeed, rBody.velocity.y);
			}
		} else {
			rBody.velocity = new Vector2 (0 * maxSpeed, rBody.velocity.y);
		}


//		if (move > 0 && !facingRight)
//			Flip();
//		else if (move < 0 && facingRight)
//			Flip();
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
			Debug.Log("Hit target? " + (transform.position.x < (target.x + hitRadius) && transform.position.x > (target.x - hitRadius)));
			if (transform.position.x < (target.x + hitRadius) && transform.position.x > (target.x - hitRadius)) {
				goToTarget = false;
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
		Debug.Log("Go to this position: " + position);
		target = position;
		goToTarget = true;

	}

}