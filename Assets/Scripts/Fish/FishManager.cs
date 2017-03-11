using UnityEngine;
using System.Collections;
using Steer2D;

public class FishManager : MonoBehaviour {

	public Blackboard blackboard;

	private SteeringAgent steeringScript;
	private Arrive arriveScript;
	private Flee fleeScript;
	private FollowPath followScript;

	public bool attractedToFishingSpot = false;

	public bool victimInWater = false;
	public Vector3 victimPosition;

	// Use this for initialization
	void Start ()
	{

		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard> ();
		}

		steeringScript = GetComponent<SteeringAgent> ();
		fleeScript = GetComponent<Flee> ();
		arriveScript = GetComponent<Arrive> ();
		followScript = GetComponent<FollowPath> ();

		Vector2[] path = blackboard.GetFollowPath ();
		SetFollowPath(path);

		InvokeRepeating("ToggleFlocking", 0.0f, 5.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (steeringScript.CurrentVelocity.sqrMagnitude <= 10) {
			GoToRandomLocation ();
		}

//		if (arriveScript.enabled) {
//			if (arriveScript.arrived || arriveScript.nearlyArrived) {
//				arriveScript.enabled = false;
//				ToggleFlocking ();
//			}
//		} else {
//			GoToRandomLocation();
//		}

	}

	public void AttractTo (Vector3 position)
	{
		if (!arriveScript.enabled) {
			arriveScript.enabled = true;
		}
		arriveScript.TargetPoint = position;
	}

	public void SetFollowPath (Vector2[] path)
	{
		if (!followScript.enabled) {
			followScript.enabled = true;
		}
		followScript.Path = path;
	}

	void ToggleFlocking(){
		if (Random.Range (0, 100) < 50 && !victimInWater) {
			followScript.enabled = !followScript.enabled;
		}
	}

	void ToggleArrive(){
		arriveScript.enabled = !arriveScript.enabled;
	}

	void GoToRandomLocation ()
	{
		float randomX = Random.Range(blackboard.seaBounds.min.x + blackboard.seaBuffer, blackboard.seaBounds.max.x - blackboard.seaBuffer);
		float randomY = Random.Range(blackboard.seaBounds.min.y + blackboard.seaBuffer, blackboard.seaBounds.max.y - blackboard.seaBuffer);

		AttractTo(new Vector2(randomX, randomY));
	}

}
