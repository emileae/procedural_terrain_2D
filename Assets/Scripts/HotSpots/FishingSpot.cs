using UnityEngine;
using System.Collections;

public class FishingSpot : MonoBehaviour {

	public Blackboard blackboard;
	public int numFishToAttract = 5;

	void Start ()
	{
		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Blackboard>();
		}

		// might come in handy when player throws bait into the water
//		AttractFish();
	}

	void Update () {
	
	}

	void AttractFish(){
		blackboard.AttractFish(transform.position, numFishToAttract);
	}

}
