using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

	public float maxSpeed = 2f;

	private bool facingRight = true;
	private Animator anim;
	private Rigidbody2D rBody;
	private BoxCollider2D steerCollider;
	private Bounds steerBounds;

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D> ();
		steerCollider = GetComponent<BoxCollider2D>();
		steerBounds = steerCollider.bounds;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CheckMove (GameObject player)
	{
		PlayerController playerScript = player.GetComponent<PlayerController> ();
		if (Input.GetAxisRaw ("Vertical") != 0) {
			if (player.transform.position.x > steerBounds.center.x) {
				Debug.Log ("Move right");
				rBody.velocity = new Vector2 (1 * maxSpeed, 0);
				playerScript.KeepPlayerOnBoat(rBody.velocity.x);
			} else if (player.transform.position.x < steerBounds.center.x) {
				Debug.Log ("Move left");
				rBody.velocity = new Vector2 (-1 * maxSpeed, 0);
				playerScript.KeepPlayerOnBoat(rBody.velocity.x);
			}
		} else {
			rBody.velocity = Vector2.zero;
		}
	}

	void OnTriggerStay2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			CheckMove(go);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController>();
			playerScript.onBoat = false;
			rBody.velocity = Vector2.zero;
		}
	}

}
