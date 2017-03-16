using UnityEngine;
using System.Collections;

public class PayTarget : MonoBehaviour {

	public int cost = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.CompareTag ("Player")) {
			PlayerController playerScript = col.gameObject.GetComponent<PlayerController>();
			playerScript.payTarget = gameObject;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.CompareTag ("Player")) {
			PlayerController playerScript = col.gameObject.GetComponent<PlayerController>();
			playerScript.payTarget = null;
		}
	}

}
