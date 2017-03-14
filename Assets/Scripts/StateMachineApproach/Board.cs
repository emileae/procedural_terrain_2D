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
