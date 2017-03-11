using UnityEngine;
using System.Collections;
using System.IO;

public class Building : MonoBehaviour {

	public Blackboard blackboard;

	public bool payable = true;
	public bool building = false;
	public float buildTime = 10.0f;

	// Package details
	public GameObject package;
//	public GameObject rigidBodyContainer;
//	private Rigidbody2D rBody;
	public bool isPackage = false;
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

	// Use this for initialization
	void Start ()
	{

		if (blackboard == null) {
			blackboard = GameObject.Find ("Blackboard").GetComponent<Blackboard> ();
		}

//		if (isPackage) {
//			rBody = rigidBodyContainer.GetComponent<Rigidbody2D>();
//		}
	
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
		if (!isPackage) {
			GameObject buildingPackage = Instantiate (package, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		} else {
			switch (packageType) {
				case 0:
					Debug.Log("Activate teh fishingspot model");
					break;
				default:
					Debug.Log("Activate teh fishingspot model");
					break;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "NPC") {
			NPCController npcScript = go.GetComponent<NPCController> ();
			if (npcScript.targetGameObject == gameObject) {
				Debug.Log("Arrived at target building");
				npcScript.state = 2;
				npcScript.StartBuilding(buildTime);
			}
		}
//		if (go.tag == "Player") {
//			PlayerController playerScript = go.GetComponent<PlayerController> ();
//		}
	}

	// TODO: fix so that it stops NPC from continuing to count the build process if NPC is removed from building collider
	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "NPC") {
			NPCController npcScript = go.GetComponent<NPCController> ();
			if (npcScript.targetGameObject == gameObject) {
				if (npcScript.state == 2) {
					// time to build is totally reset
					npcScript.StopBuilding();
				}
			}
		}
//		if (go.tag == "Player") {
//			PlayerController playerScript = go.GetComponent<PlayerController> ();
//		}
	}

}
