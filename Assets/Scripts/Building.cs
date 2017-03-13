using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public Blackboard blackboard;

	// Payment
	private Payment paymentScript;

	public bool payable = true;
	public bool building = false;
	public float buildTime = 10.0f;

	// Geometry management
	public GameObject unbuiltModel;

	// following the player
	private GameObject playerObject = null;
	private bool followPlayer = false;

	// Package details
	public GameObject package;
//	public GameObject rigidBodyContainer;
//	private Rigidbody2D rBody;
	public bool isPackage = false;
	public bool placedPackage = false;
	// package types
	// type 0 -> fishing spot
	// type 1 -> wall
	// type 2 -> fishing rack
	// type 3 -> jetty
	// type 4 -> torch light
	// type 5 -> house
	// type 6 -> lighthouse
	// type 7 -> boat
	// type 8 -> fish garden
	public int packageType = 0;

	// Useable Building
	public bool active = false;

	// Fish Rack specific
	public int maxFishStored;
	public int fishStored = 0;

	// Fishing Spot tracking
	public int totalFishingRounds = 0;
	public int numFishingRoundsToClose = 3;

	// Use this for initialization
	void Start ()
	{

		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard> ();
		}

		paymentScript = GetComponent<Payment>();

//		if (isPackage) {
//			rBody = rigidBodyContainer.GetComponent<Rigidbody2D>();
//		}
	
	}

	void Update ()
	{
		if (followPlayer) {
			transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + 5, transform.position.z);
		}
	}

	void FixedUpdate ()
	{
	}

	public void StartBuilding ()
	{
		building = true;
		payable = false;

		if (packageType == 2) {
			FinishedBuilding ();
		}else{
			blackboard.AddToBuildingList (gameObject);
		}

	}

	public void FinishedBuilding ()
	{
		Debug.Log ("Finished building... instantiate package maybe add a particle effect?");
		blackboard.RemoveFromBuildingList (gameObject);
		building = false;
		if (!isPackage) {
			GameObject buildingPackage = Instantiate (package, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			unbuiltModel.SetActive (false);
		} else {
			switch (packageType) {
				case 0:// fishingSpot
					Debug.Log("Activate teh fishingspot model");
//					blackboard.CallNearestNPC(gameObject);
					blackboard.AddGameObjectToList(gameObject, blackboard.workList);
					paymentScript.level += 1;
					isPackage = false;
					active = true;
					break;
				case 2:// fishRack
					isPackage = false;
					active = true;
					paymentScript.level += 1;
					blackboard.ActivateFishRack(gameObject);
					break;
				default:
					Debug.Log("Activate teh fishingspot model");
//					blackboard.CallNearestNPC(gameObject);
					blackboard.AddGameObjectToList(gameObject, blackboard.workList);
					paymentScript.level += 1;
					isPackage = false;
					active = true;
					break;
			}
		}
	}

	void CloseBuilding(){
		payable = true;
		active = false;
		totalFishingRounds = 0;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "NPC") {
			NPCController npcScript = go.GetComponent<NPCController> ();

			// If its something that needs to be built / harvested
			if (npcScript.targetGameObject == gameObject && paymentScript.level == 0) {
				Debug.Log ("Arrived at target building");
				npcScript.state = 2;
				npcScript.StartBuilding (buildTime);
			}

			// if this isn't to be built... so NPC needs to work here
			if (active && npcScript.targetGameObject == gameObject && paymentScript.level != 0) {
				Debug.Log ("Tell NPC to work here");

				// remove it if its on the workList
				blackboard.RemoveGameObjectToList(gameObject, blackboard.workList);

				// if NPC is not currently employed
				if (npcScript.workLocation == null) {
					npcScript.workLocation = gameObject;
				}

				// if fishing spot then set npc state to fishing (0 -> 3)
				switch (packageType) {
				case 0:
					npcScript.state = 3;
					npcScript.GetFish (paymentScript.level);
					totalFishingRounds += 1;
					if (totalFishingRounds > numFishingRoundsToClose) {
						Debug.Log("Close building then idle");
						CloseBuilding ();
						npcScript.Idle();
					}
					break;
				case 2:
					npcScript.state = 3;
					npcScript.DropOffFish (paymentScript.level);
					break;
				default:
					Debug.Log ("Switch statement error - triggerEnter2D in Building.cs");
					break;
				}
			}

			if (!active && npcScript.targetGameObject == gameObject) {
				if (npcScript.workLocation == gameObject) {
					Debug.Log("Idle!!!!!!!!!");
					npcScript.Idle();
				}
			}

		}
		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController> ();
			if (isPackage) {
				playerScript.nearPackage = true;
			}
		}
	}

	// TODO: fix so that it stops NPC from continuing to count the build process if NPC is removed from building collider
	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController> ();
			// Player bool nearPackage is also set in Payment.cs when player pays for a package
			if (isPackage) {
				playerScript.nearPackage = false;
			}
		}
	}

	public void FollowPlayer(GameObject player){
		playerObject = player;
		followPlayer = true;
	}
	public void UnFollowPlayer(GameObject player){
		playerObject = null;
		followPlayer = false;
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
	}

}
