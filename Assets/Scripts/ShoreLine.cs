using UnityEngine;
using System.Collections;

public class ShoreLine : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.layer == 9) {
			Debug.Log("TRIGGERERERERERERER");
			NPCController npcScript = go.GetComponent<NPCController>();
			npcScript.ReachedShoreLine();
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.layer == 9) {
			NPCController npcScript = go.GetComponent<NPCController>();
			npcScript.UnStopNPC();
		}
	}

}
