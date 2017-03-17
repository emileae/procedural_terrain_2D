using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Structure : MonoBehaviour {

	private Board blackboard = null;

	public int resourceType;
	public int foundationType;
	public int[] platforms;

	public float buildTime = 5.0f;

	// Activation types / categories
	// 0 -> Call NPC to work here --> requires payment to activate eg. fishing spot / garden / hunting
	// 1 -> Create a barrier --> Creates a collider against weather and enemies but not NPCs or players eg. wall
	//  -> Add to Blackboard --> Used to perform general tasks
	// 2							- Add Fish Rack to blackboard for NPCs to drop off fish
	// 3							- Gather NPCs at night in house
	// 4							- Reduce enemy attack, by preventing them from approaching - torch 
	// 5							- Reduce enemy attack, by preventing them from approaching - lighthouse
	// 6 -> Remove barrier --> build short bridge over a well, remove edge collider
	// 7 -> Remove barrier and add to platform --> long bridge, remove edge collider and extend platform
	// 8 -> Remove barrier and add to platform + boat --> jetty, remove shore collider and extend  platform to the boat, also add payment for number of boat rounds
	public int activationType = 0;

	// Use this for initialization
	void Start ()
	{
		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Board>();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Activate (){
		Debug.Log ("Depending on the type of structure... activate the functionality here");
		switch (activationType) {
			case 0:
				Debug.Log("Call NPC to work here");
				ActivateFishingStructure();
				break;
			case 1:
				Debug.Log("Create wall barrier");
				break;
			case 2:
				Debug.Log("Add Fish Rack to blackboard");
				break;
			case 3:
				Debug.Log("Add night time gather point to blackboard");
				break;
			case 4:
				Debug.Log("Reduce enemy attack with a torch");
				break;
			case 5:
				Debug.Log("Reduce enemy attack with a lighthouse");
				break;
			case 6:
				Debug.Log("Remove barrier - build a bridge over a well");
				break;
			case 7:
				Debug.Log("Remove Barrier and extend platform to long bridge");
				break;
			case 8:
				Debug.Log("Remove Barrier and extend platform to jetty");
				break;
			default:
				Debug.Log("Structure.cs activation fall through error");
				break;
		}
	}

	void ActivateFishingStructure ()
	{
		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Board>();
			blackboard.fishingStructures.Add (gameObject);
			blackboard.fishingStructureScripts.Add (gameObject.GetComponent<FishingStructure> ());
			blackboard.workList.Add (gameObject);
		} else {
			blackboard.fishingStructures.Add (gameObject);
			blackboard.fishingStructureScripts.Add (gameObject.GetComponent<FishingStructure> ());
			blackboard.workList.Add (gameObject);
		}
	}

}
