using UnityEngine;
using System.Collections;

public class flockk : MonoBehaviour {

	public float speed = 0.001f;
	float rotationSpeed = 4.0f;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighbourDistance = 3.0f;
	public GameObject fishTarget;

	bool turning = false;

	void Start () 
	{
		speed = Random.Range(0.5f,1);
		fishTarget = GameObject.Find("FishTarget");
	}

	void Update () 
	{
		if(Vector3.Distance(transform.position, fishTarget.transform.position) >= globalFlock.tankDimensions.y)
		{
			turning = true;
		}
		else
			turning = false;

		if(turning)
		{
			Vector3 direction = Vector3.zero - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
			speed = Random.Range(0.5f,1);
		}
		else
		{
			if(Random.Range(0,5) < 1)
				ApplyRules();
			
		}
		transform.Translate(Time.deltaTime * speed, 0, 0);
	}

	void ApplyRules ()
	{
		GameObject[] gos;
		gos = globalFlock.allFish;
		
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
