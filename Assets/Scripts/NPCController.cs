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
	// state = 3 -> fishing, unavailable (TODO: should probably respond to Player calling though.....)
	public int state = 0;// give states numbers 0 = idle = available

	// working
	private IEnumerator workingCoroutine;

	// building
	public GameObject targetGameObject;

	// work specialisations
	// fishing
	public int fishCarryingCapacity = 3;
	public int fishInHand = 0;
	public float baseFishingTime = 10.0f;

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

		switch (state) {
			case 0:
				Idle ();
				break;
			case 1:
				if (goToTarget && !stop) {
					if (transform.position.x < target.x) {
						Debug.Log ("GO RIGHT");
						direction = 1;
					} else if (transform.position.x > target.x) {
						Debug.Log ("GO LEFT");
						direction = -1;
					}
					speed = maxSpeed;
				}
				if (!targetGameObject) {
					if (transform.position.x < (target.x + hitRadius) && transform.position.x > (target.x - hitRadius)) {
						Debug.Log("Arrived at player's call");
						direction = 0;
						state = 0;
						goToTarget = false;
					}
				}
	            break;
			case 2:
				direction = 0;
	            break;
			case 3:
				direction = 0;
				Debug.Log("NPC has stopped to work... catching fish");
	            break;
	        default:
				Idle ();
	            break;
        }

		if (direction == 0) {
//			Debug.Log("Constrain the x position");
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

		// stop after responding to player's call... does not apply to moving towards a building / work target
//		if (goToTarget && !targetGameObject == null) {
//			if (transform.position.x < (target.x + hitRadius) && transform.position.x > (target.x - hitRadius)) {
//				Debug.Log("ARRIVED AT PLAYERS CALL");
//				goToTarget = false;
//				// return to idle state
//				state = 0;
////				target = null;
//			}
//		}
		
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
			Debug.Log("...Show building animation...");
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

	public void GetFish (int FishingSpotLevel)
	{
		switch (FishingSpotLevel) {
			case 1:
				Debug.Log("Fetch " + fishCarryingCapacity + " fish");
				StartCoroutine(GetAFish(FishingSpotLevel));
				break;
			default:
				Debug.Log("Fetch " + fishCarryingCapacity + " fish");
				StartCoroutine(GetAFish(FishingSpotLevel));
				break;
		}
	}

	IEnumerator GetAFish (int FishingSpotLevel)
	{
		// reduce wait time depending on the fishing spot level
		// TODO: take into account the fishing spot's height.. lower spots give more fish / fish are caught faster
		if (FishingSpotLevel <= 0) {
			Debug.Log("ERROR - NPC is trying to fish at a level 0");
			FishingSpotLevel = 1;
		}
		float waitTime = baseFishingTime / FishingSpotLevel;
		yield return new WaitForSeconds (waitTime);
		fishInHand += 1;
		if (fishInHand < fishCarryingCapacity) {
			GetFish (FishingSpotLevel);
		}else{
			Debug.Log("Drop fish off at fish rack");
		}
	}

}