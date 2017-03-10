using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class Payment : MonoBehaviour {

	// Payments
	public int level = 0;
	public int maxLevel = 1;
	public int buildCost = 3;
	public int useCost = 1;
	private int amountPaid = 0;
	private bool paid = false;
	public GameObject fishOutline;
	public GameObject currencyIndicatorContainer;

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

	void ShowCost ()
	{
		Bounds bounds = gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().bounds;
		int cost;
		if (level == 0) {
			Debug.Log ("Cost = " + buildCost);
			cost = buildCost;
		} else {
			Debug.Log ("Cost = " + useCost);
			cost = useCost;
		}

		for (int i = 0; i < cost; i++) {
			GameObject currencyFishClone = Instantiate(fishOutline, new Vector3(transform.position.x, transform.position.y + (1.5f * bounds.size.y) + (i * 5), transform.position.z), Quaternion.Euler(-90, 0, 0)) as GameObject;
			currencyFishClone.transform.parent = currencyIndicatorContainer.transform;
		}

	}

	void HideCost ()
	{
		foreach (Transform child in currencyIndicatorContainer.transform) {
			Destroy(child.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {

			ShowCost();

			PlayerController playerScript = go.GetComponent<PlayerController>();
			playerScript.payTarget = gameObject;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {

			HideCost();

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
