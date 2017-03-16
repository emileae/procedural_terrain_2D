using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	// Processed package prefabs
	public GameObject package0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Processed prefab types
	// type 0 -> wood
	// type 1 -> rock
	// type 2 -> bush
	public GameObject GetProcessedPrefab (int processType)
	{
		switch (processType) {
			case 0:
				return package0;
				break;
			default:
				return null;
				Debug.Log("Blackboard not returning proper package process type");
				break;
		}
	}

}
