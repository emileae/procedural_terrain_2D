using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Blackboard blackboard;

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

	// Paying
	private bool passingCurrency = false;
	public int currency = 100;
	public GameObject payTarget = null;

	// Packages
	public bool nearPackage = false;
	public bool carryingPackage = false;
	public GameObject carriedPackage = null;

	// Boat
	public bool onBoat = false;
	private float boatXVelocity;

	// Use this for initialization
	void Start ()
	{
		rBody = GetComponent<Rigidbody2D> ();
//		anim = GetComponent<Animator>();

		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Blackboard>();
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

		float move = Input.GetAxisRaw ("Horizontal");

//		anim.SetFloat ("Speed", Mathf.Abs (move));

//		if (climbing) {
//			Debug.Log ("CLIMBING................");
//			rBody.velocity = new Vector2 (move * maxSpeed, -rBody.velocity.y);
//		} else {

		if (onBoat) {
			rBody.velocity = new Vector2 (move * maxSpeed + boatXVelocity, rBody.velocity.y);
		} else {
			rBody.velocity = new Vector2 (move * maxSpeed, rBody.velocity.y);
		}

//		rBody.velocity = new Vector2 (move * maxSpeed, rBody.velocity.y);

		// prevent sliding
		if (move == 0) {
			rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
//			rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
		} else {
			rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

//		}

//		Debug.Log ("Player move rBody.velocity: " + rBody.velocity);


		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}
	}

	// put inputs here because if we use Fixedupdate, we may miss an input
	void Update ()
	{
		if (grounded && Input.GetButtonDown ("Jump")) {
//			anim.SetBool ("Ground", false);
			rBody.AddForce (new Vector2 (0, jumpForce));
		}

		float inputV = Input.GetAxisRaw ("Vertical");

//		if (climbing) {
//			Debug.Log("CLIMBING................");
//		}

		if (payTarget != null) {
			if (inputV < 0) {
				Pay ();
			}
		}




		// call NPCs
		bool action = Input.GetButton ("Fire3");
		bool actionButtonDown = Input.GetButtonDown ("Fire3");

		if (actionButtonDown && !nearPackage && !carryingPackage) {
			blackboard.CallNPCs (transform.position);
		} else if (actionButtonDown && nearPackage && !carryingPackage) {

			Building buildingScript = payTarget.GetComponent<Building> ();
			// can only pick up pakcages that have not been placed
			if (!buildingScript.placedPackage) {
				carryingPackage = true;
				// TODO: make sure this GetComponent is not called excessively
				buildingScript.FollowPlayer (gameObject);
				carriedPackage = payTarget;
			} else {
				blackboard.CallNPCs (transform.position);
			}
		}else if (actionButtonDown && carryingPackage){
			carryingPackage = false;
			carriedPackage.GetComponent<Building>().UnFollowPlayer(gameObject);
			carriedPackage = null;
		}

//		else if (action && nearPackage) {
//			if (carriedPackage == null) {
////				Debug.Log ("Pick up package");
//////			payTarget.transform.position = new Vector3 (transform.position.x, transform.position.y, payTarget.transform.position.z);
////				payTarget.transform.parent = gameObject.transform;
////				carryingPackage = true;
//				StartCoroutine(PickUpPackage());
//			}
//		}

//		if (action && carriedPackage != null) {
//			StartCoroutine(DropOffPackage());
//		}

		
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Pay ()
	{
		if (!passingCurrency) {
			passingCurrency = true;
			StartCoroutine(PassCoin());
		}
	}

	IEnumerator PassCoin ()
	{
		yield return new WaitForSeconds (0.3f);
		if (payTarget) {
			currency -= 1;
			Payment paymentScript = payTarget.GetComponent<Payment> ();
			bool paid = paymentScript.Pay ();
		}
		passingCurrency = false;
	}

	public void ReturnPayment(int returnedCurrency){
		currency += returnedCurrency;
	}

	public void KeepPlayerOnBoat(float xVelocity){
		onBoat = true;
		boatXVelocity = xVelocity;
	}

	public void FreezePlayerX(){
		rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
	}

	public void UnFreezePlayerX(){
		rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

//	IEnumerator PickUpPackage(){
//		carryingPackage = true;
//		yield return new WaitForSeconds(0.1f);
//		Debug.Log("Pick up package");
////			payTarget.transform.position = new Vector3 (transform.position.x, transform.position.y, payTarget.transform.position.z);
//		carriedPackage = payTarget;
//		carriedPackage.transform.parent = gameObject.transform;
//	}
//
//	IEnumerator DropOffPackage(){
//		carryingPackage = false;
//		yield return new WaitForSeconds(0.1f);
//		Debug.Log("Drop off package");
//		carriedPackage.transform.parent =null;
//	}

//	public void AddBoatVelocity(Vector2 velocity){
//		onBoat = true;
//		boatVelocity = velocity;
//	}
//
//	public void OffBoat(){
//		onBoat = false;
//		boatVelocity = Vector2.zero;
//	}

}