using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	public float speed = 5.0f;
	public Vector2 direction = Vector2.right;
	private bool facingRight = true;

	private Transform childModel;

	public FishController controller;

	public GameObject fishTarget;

	// flocking logic
	float rotationSpeed = 4.0f;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighbourDistance = 3.0f;

	bool turning = false;

	// Use this for initialization
	void Start ()
	{
		fishTarget = GameObject.Find("FishTarget");
		controller = GameObject.Find("FishController").GetComponent<FishController>();
		childModel = this.gameObject.transform.GetChild(0);
		speed += Random.Range (-0.3f, 0.3f);
		direction = (new Vector2(transform.position.x, transform.position.y) - Vector2.zero);
		if (Random.Range (0f, 1f) > 0.5) {
			direction *= -1;
			facingRight = !facingRight;
			Vector3 theScale = childModel.localScale;
			theScale.x *= -1;
			childModel.localScale = theScale;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
//		transform.Translate(direction * speed * Time.deltaTime);
//
//		if (direction.x > 0 && !facingRight) {
//			Flip ();
//		} else if (direction.x < 0 && facingRight) {
//			Flip ();
//		}
//
//		if (transform.position.x <= (controller.seaBounds.min.x + controller.seaBuffer) || transform.position.x >= (controller.seaBounds.max.x - controller.seaBuffer)){
//			ChangeDirection ();
//		}


// Flocking
		if (Vector3.Distance (transform.position, fishTarget.transform.position) >= 5) {
			turning = true;
		} else {
			turning = false;
		}

		if (turning) {
			Vector3 direction =fishTarget.transform.position - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
			speed = Random.Range (0.5f, 1);
		} else {
			if (Random.Range (0, 5) < 1) {
				FlockRules ();
			}
			
		}
		transform.Translate(Time.deltaTime * speed, 0, 0);

	}

	public void ChangeDirection ()
	{
		direction *= -1;
		Flip();
	}

	public void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = childModel.localScale;
		theScale.x *= -1;
		childModel.localScale = theScale;
	}


	void FlockRules(){
		GameObject[] gos;
		gos = controller.allFish;
		
		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.1f;
		
//		Vector3 goalPos = globalFlock.goalPos.transform.position;
		Vector3 goalPos = fishTarget.transform.position;

		float dist;

		int groupSize = 0;
		foreach (GameObject go in gos) {
			if (go != this.gameObject) {
				dist = Vector3.Distance (go.transform.position, this.transform.position);

				// if this fish is close enough to flock with
				if (dist <= neighbourDistance) {
					// get the direction to the centre
					vcentre += go.transform.position;
					groupSize++;	

					// avoid if too close
					if (dist < 1.0f) {
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}

					// match speed
					Fish anotherFlock = go.GetComponent<Fish> ();
					gSpeed = gSpeed + anotherFlock.speed;
				}
			}
		} 

		// if there's a flock
		if (groupSize > 0) {
			// get the average center taking into account the goal position
			vcentre = vcentre / groupSize + (goalPos - this.transform.position);
			// get average speed
			speed = gSpeed / groupSize;
			
			Vector3 direction = (vcentre + vavoid) - transform.position;
			if (direction != Vector3.zero) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), rotationSpeed * Time.deltaTime);
			}
		}
	}

}
