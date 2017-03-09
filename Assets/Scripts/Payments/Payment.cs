using UnityEngine;
using System.Collections;

public class Payment : MonoBehaviour {

	// Payments
	public int level = 0;
	public int maxLevel = 1;
	public int buildCost = 3;
	public int useCost = 1;
	private int amountPaid = 0;
	private bool paid = false;

	public bool Pay ()
	{
		amountPaid += 1;
		paid = false;

		// not build
		if (level == 0) {
			if (amountPaid >= buildCost) {
				paid = true;
				amountPaid = 0;
				level += 1;
			}
		} 
		// use cost
		else {
			if (amountPaid >= useCost) {
				paid = true;
				amountPaid = 0;
			}
		}

		return paid;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController>();
			playerScript.payTarget = gameObject;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {
			PlayerController playerScript = go.GetComponent<PlayerController>();
			playerScript.payTarget = null;
			// if player has paid something but exits before completing payment
			if (!paid && amountPaid > 0) {
				playerScript.ReturnPayment(amountPaid);
				amountPaid = 0;
			}
		}
	}
}
