using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blackboard : MonoBehaviour {

	public List<GameObject> npcs = new List<GameObject>();
	public List<NPCController> npcScripts = new List<NPCController>();

	// Use this for initialization
	void Awake () {
		GameObject npcContainer = GameObject.Find("NPCs");
		foreach (Transform child in npcContainer.transform)
		{
			npcs.Add(child.gameObject);
			npcScripts.Add(child.GetComponent<NPCController>());
		}
	}
	
	public void CallNPCs (Vector3 position)
	{
		for (int i = 0; i < npcScripts.Count; i++) {
			npcScripts[i].GoToLocation(position);
		}
	}
}
