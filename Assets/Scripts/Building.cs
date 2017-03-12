using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public Blackboard blackboard;

	// Building
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
		if (isPackage) {
			Debug.Log ("Started to build package");
//			rBody.isKinematic = true;
//			rigidBodyContainer.SetActive(false);
		}
		blackboard.AddToBuildingList(gameObject);

	}

	public void FinishedBuilding ()
	{
		Debug.Log ("Finished building... instantiate package maybe add a particle effect?");
		blackboard.RemoveFromBuildingList(gameObject);
		if (!isPackage) {
			GameObject buildingPackage = Instantiate (package, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			unbuiltModel.SetActive(false);
		} else {
			switch (packageType) {
				case 0:
					Debug.Log("Activate teh fishingspot model");
//					blackboard.CallNearestNPC(gameObject);
					blackboard.AddGameObjectToList(gameObject, blackboard.workList);
					paymentScript.level += 1;
					isPackage = false;
					active = true;
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
			if (active && paymentScript.level != 0) {
				Debug.Log ("Tell NPC to work here");
				// if fishing spot then set npc state to fishing (0 -> 3)
				switch (packageType) {
					case 0:
						npcScript.state = 3;
						npcScript.GetFish(paymentScript.level);
						break;
					default:
						npcScript.state = 3;
						npcScript.GetFish(paymentScript.level);
						break;
				}
			}
		}
		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController> ();
			if (isPackage) {
				Debug.Log("isPackage??????????????????????");
				playerScript.nearPackage = true;
			}
		}
	}

	// TODO: fix so that it stops NPC from continuing to count the build process if NPC is removed from building collider
	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;

//		if (go.tag == "NPC") {
//			NPCController npcScript = go.GetComponent<NPCController> ();
//			if (npcScript.targetGameObject == gameObject) {
//				if (npcScript.state == 2) {
//					// time to build is totally reset
//					npcScript.StopBuilding();
//				}
//			}
//		}
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
