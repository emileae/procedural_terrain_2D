using UnityEngine;
using System.Collections;

public class Sea : MonoBehaviour {

	public Blackboard blackboard;

	private BoxCollider2D bx2d;
	private Bounds bounds;

	void Start(){
		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Blackboard>();
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "Player") {
			blackboard.AttractFishToVictim(go.transform.position);
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "Player") {
			blackboard.RemoveAttractFishToVictim();
		}
	}
}
