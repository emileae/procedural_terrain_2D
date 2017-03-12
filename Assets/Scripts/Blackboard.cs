using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blackboard : MonoBehaviour {

	public MapGenerator mapGenerator;

	public GameObject sea;
	public Bounds seaBounds;
	public float seaBuffer = 1.0f;

	// builder polling
	private bool callingBuilders = false;
	public float pollForBuilderTime = 5.0f;

	// worker polling
	private bool callingWorkers = false;
	public float pollForWorkerTime = 5.0f;

	public List<GameObject> npcs = new List<GameObject>();
	public List<NPCController> npcScripts = new List<NPCController>();

	// Fish
	public float fishZPos = -1;
	public List<GameObject> fish = new List<GameObject>();
	public List<FishManager> fishManagerScripts = new List<FishManager>();

	// the hot spots
	public List<GameObject> fishingSpots = new List<GameObject>();

	// Buildings
	public List<GameObject> buildingList = new List<GameObject>();

	// Work / Tasks
	public List<GameObject> workList = new List<GameObject>();

	// Use this for initialization
	void Awake ()
	{
		mapGenerator.GenerateMap();


		// set up the sea Bounds
		if (sea == null) {
			sea = GameObject.Find("Sea");
			seaBounds = sea.transform.GetChild(0).GetComponent<MeshRenderer>().bounds;
		}

		// set up lists
		GameObject npcContainer = GameObject.Find ("NPCs");
		foreach (Transform child in npcContainer.transform) {
			npcs.Add (child.gameObject);
			npcScripts.Add (child.GetComponent<NPCController> ());
		}

		GameObject fishContainer = GameObject.Find ("Fish");
		foreach (Transform child in fishContainer.transform) {
			fish.Add (child.gameObject);
			fishManagerScripts.Add (child.GetComponent<FishManager> ());
		}

		// fishing spots added to list as they're added to the map
//		GameObject[] fishingspots = GameObject.FindGameObjectsWithTag ("FishingSpot");
//		for (int i = 0; i < fishingspots.Length; i++) {
//			fishing.Add(fishingspots[i]);
//		}

		// after all fishing spots have been listed
		/// TODO: make sure that all fish have also been loaded
//		SetFishPath ();

	}

	void Update ()
	{
		if (buildingList.Count > 0 && !callingBuilders) {
			Debug.Log("Polling for builders....");
			callingBuilders = true;
			StartCoroutine(PollForBuilders());
		}

		if (workList.Count > 0 && !callingWorkers) {
			Debug.Log("Polling for workers....");
			callingBuilders = true;
			StartCoroutine(PollForWorkers());
		}
	}

	public void AddGameObjectToList(GameObject go, List<GameObject> list){
		list.Add(go);
	}
	
	public void CallNPCs (Vector3 position)
	{
		for (int i = 0; i < npcScripts.Count; i++) {
			// TODO: check this logic only applies to idling NPCs for now
			if (npcScripts [i].state == 0) {
				npcScripts [i].GoToLocation (position);
			}
		}
	}

	public void AttractFish (Vector3 position, int numFish)
	{
		Vector3 inSeaPosition = new Vector3(position.x, seaBounds.max.y - (seaBuffer*2), fishZPos);
		for (int i = 0; i < fishManagerScripts.Count; i++) {
			fishManagerScripts[i].AttractTo(inSeaPosition);
		}
	}

	public void AttractFishToVictim (Vector3 position)
	{
		Vector3 inSeaPosition = new Vector3(position.x, seaBounds.max.y - (seaBuffer*2), fishZPos);
		for (int i = 0; i < fishManagerScripts.Count; i++) {
			fishManagerScripts[i].victimInWater = true;
			fishManagerScripts[i].AttractTo(inSeaPosition);
		}
	}

	public void RemoveAttractFishToVictim(){
		for (int i = 0; i < fishManagerScripts.Count; i++) {
			fishManagerScripts[i].victimInWater = false;
		}
	}

	public Vector2[] GetFollowPath ()
	{
//		Debug.Log("Fishing.Count..... " + fishingSpots.Count);
		Vector2[] path = new Vector2[fishingSpots.Count];
		for (int i = 0; i < fishingSpots.Count; i++) {
			// add a random depth to fish target, up to 25% of total sea depth
			float randomDepth = Random.Range(0.0f, seaBounds.extents.y/2);
			path[i] = new Vector2(fishingSpots[i].transform.position.x, seaBounds.max.y - (seaBuffer*2 + randomDepth));
		}

		return path;
	}

	public void SetFishPath ()
	{
		Vector2[] path = new Vector2[fishingSpots.Count];
		for (int i = 0; i < fishingSpots.Count; i++) {
			path[i] = new Vector2(fishingSpots[i].transform.position.x, seaBounds.max.y - (seaBuffer*2));
		}

		for (int i = 0; i < fishManagerScripts.Count; i++) {
			fishManagerScripts[i].SetFollowPath(path);
		}
	}

	public void AddToBuildingList (GameObject building)
	{
		buildingList.Add(building);
		Debug.Log("Call NPC to build");

		CallNearestNPC(building);

	}

	public void RemoveFromBuildingList (GameObject building)
	{
		buildingList.Remove(building);
	}

	IEnumerator PollForBuilders ()
	{
		yield return new WaitForSeconds (pollForBuilderTime);
		if (buildingList.Count > 0) {
			Debug.Log ("called a builder. . . . .");
			CallNearestNPC (buildingList [0]);
		}
		callingBuilders = false;
	}

	IEnumerator PollForWorkers ()
	{
		yield return new WaitForSeconds (pollForWorkerTime);
		if (workList.Count > 0) {
			Debug.Log ("called a worker. . . . .");
			CallNearestNPC (workList [0]);
		}
		callingWorkers = false;
	}

	public void CallNearestNPC (GameObject destination)
	{
		bool foundAvailableNPC = false;
		float npcDistance = Mathf.Infinity;
		int nearestNPCIndex = 0;
		for (int i = 0; i < npcScripts.Count; i++) {
			if (npcScripts [i].state == 0) {
				Vector3 offset = destination.transform.position - npcs [i].transform.position;
				float sqrMagDistance = offset.sqrMagnitude;
				if (sqrMagDistance < npcDistance) {
					npcDistance = sqrMagDistance;
					nearestNPCIndex = 0;
					foundAvailableNPC = true;
				}
			}
		}

		if (foundAvailableNPC) {
			npcScripts[nearestNPCIndex].GoToLocation(destination.transform.position);
			npcScripts[nearestNPCIndex].targetGameObject = destination;
		}
	}

}
