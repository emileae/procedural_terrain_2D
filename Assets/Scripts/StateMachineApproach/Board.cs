using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	// Processed package prefabs
	public GameObject[] packages;

	// Building prefabs
	public GameObject[] structures;
	[HideInInspector]
	public Structure[] structureScripts;

	// LISTS

	// - NPC
	public List<GameObject> npcs = new List<GameObject>();
	public List<StatePatternNPC> npcScripts = new List<StatePatternNPC>();
	// - FishingSpots
	public float fishingTime = 5.0f;// time spent per fishing session
	public float fishOffloadTime = 5.0f;// time spent offloading Fish
	public List<GameObject> fishingStructures = new List<GameObject>();
	public List<FishingStructure> fishingStructureScripts = new List<FishingStructure>();

	// WORK LIST
	// - a list ofunoccupied work structures that need manning
	public float workerCallTime = 1.0f;
	private bool callingWorkers = false;
	public List<GameObject> workList = new List<GameObject>();

	void Awake ()
	{
		// populate structure scripts
		structureScripts = new Structure[structures.Length];
		for (int i = 0; i < structures.Length; i++) {
			structureScripts [i] = structures [i].GetComponent<Structure> ();
		}

		if (npcs.Count <= 0) {
			GameObject npcContainer = GameObject.Find("NPCs");
			foreach(Transform child in npcContainer.transform){
				npcs.Add(child.gameObject);
				npcScripts.Add(child.gameObject.GetComponent<StatePatternNPC>());
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update ()
	{
		if (workList.Count > 0 && !callingWorkers) {
			callingWorkers = true;
			StartCoroutine(CallWorkers());
		}
	}

	IEnumerator CallWorkers(){
		yield return new WaitForSeconds(workerCallTime);
		// TODO might want to think about how to order the worklist maybe not just call first item on work list?
		CallNearestNPC (workList[0]);
		Debug.Log("Calling NPC to work at this destination: " + workList[0].transform.position);
	}

	// in Statemachine version --> called from PayTarget.cs
	public void CallNearestNPC (GameObject destination)
	{

		bool foundAvailableNPC = false;
		float npcDistance = Mathf.Infinity;
		int nearestNPCIndex = 0;

		for (int i = 0; i < npcScripts.Count; i++) {
			if (!npcScripts [i].busy) {
				Vector3 offset = destination.transform.position - npcs [i].transform.position;
				float sqrMagDistance = offset.sqrMagnitude;
				if (sqrMagDistance < npcDistance) {
					npcDistance = sqrMagDistance;
					nearestNPCIndex = i;
					foundAvailableNPC = true;
				}
			}
		}

		if (foundAvailableNPC) {
			npcScripts[nearestNPCIndex].target = destination.transform;
		}

	}

	// Processed prefab types
	// type 0 -> wood
	// type 1 -> rock
	// type 2 -> bush
	public GameObject GetProcessedPrefab (int resourceType)
	{
		return packages[resourceType];
//		switch (processType) {
//			case 0:
//				return package0;
//				break;
//			default:
//				return null;
//				Debug.Log("Blackboard not returning proper package process type");
//				break;
//		}
	}

	public GameObject GetBuildingPrefab (int resourceType, int foundationType, int platform)
	{
		GameObject prefab = null;
		for (int i = 0; i < structureScripts.Length; i++) {
			if (structureScripts [i].resourceType == resourceType && structureScripts [i].foundationType == foundationType) {
				for (int j = 0; j < structureScripts [i].platforms.Length; j++) {
					if (structureScripts [i].platforms [j] == platform) {
						prefab = structures [i];
						break;
					}
				}
			}
		}

		if (prefab != null) {
			Debug.Log("Can build... " + prefab.name);
		} else {
			Debug.Log("Can not build here... ");	
		}

		return prefab;
	}

}
