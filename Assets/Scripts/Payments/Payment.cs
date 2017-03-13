using UnityEngine;
using System.Collections;
//using UnityEditor.Animations;
//using System.ComponentModel.Design.Serialization;

public class Payment : MonoBehaviour {

	// Building
	private Building buildingScript;

	// Player
	private PlayerController playerScript;

	// Payments
	public int level = 0;
	public int maxLevel = 1;
	public int buildCost = 3;
	public int useCost = 1;
	private int cost;
	private int amountPaid = 0;
	private bool paid = false;
	public GameObject fishOutline;
	public GameObject currencyIndicatorContainer;
	private GameObject[] currencyIndicators;

	private Bounds bounds;

	void Start(){
		buildingScript = GetComponent<Building>();
		bounds = GetComponent<BoxCollider2D>().bounds;
	}

	public bool Pay ()
	{
		if (buildingScript.payable) {

			amountPaid += 1;

			if (currencyIndicators.Length > 0) {
				Destroy (currencyIndicators [amountPaid - 1]);
			}

			// not built
			if (level == 0) {
				if (amountPaid >= buildCost) {
					paid = true;
					amountPaid = 0;
//				level += 1;
					buildingScript.StartBuilding ();

					if (buildingScript.isPackage) {
						// can on longer move the package once its been paid for
						// change the package model
						buildingScript.placedPackage = true;
					}

				}
			} 
			// use cost
			else {
				if (amountPaid >= useCost) {
					paid = true;
					amountPaid = 0;
					Debug.Log("Paid use cost... not sure if its activating anything?");
				}
			}
		}

		return paid;
	}

	void ShowCost ()
	{
//		Bounds bounds = gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().bounds;
		// TODO: re-work the use cost / build cost thing, now the use cost doesn't show up because its just the package build cost
		if (level == 0) {
			Debug.Log ("Cost = " + buildCost);
			cost = buildCost;
		} else {
			Debug.Log ("Cost = " + useCost);
			cost = useCost;
		}

		currencyIndicators = new GameObject[cost];

		for (int i = 0; i < cost; i++) {
			GameObject currencyFishClone = Instantiate(fishOutline, new Vector3(transform.position.x, transform.position.y + (1.5f * bounds.size.y) + (i * 3), transform.position.z), Quaternion.Euler(-90, 0, 0)) as GameObject;
//			currencyFishClone.transform.parent = currencyIndicatorContainer.transform;
			currencyIndicators[i] = currencyFishClone;
		}

	}

	void HideCost ()
	{
		for (int i = 0; i < currencyIndicators.Length; i++) {
			Destroy(currencyIndicators[i]);
		}

	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {

			if (!buildingScript.building && !buildingScript.active) {
				ShowCost ();
			}

			playerScript = go.GetComponent<PlayerController>();
			playerScript.payTarget = gameObject;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;
		if (go.tag == "Player") {

			if (currencyIndicators.Length > 0) {
				HideCost ();
			}

			playerScript = go.GetComponent<PlayerController>();
			playerScript.payTarget = null;
			// if player has paid something but exits before completing payment
			if (!paid && amountPaid > 0) {
				playerScript.ReturnPayment(amountPaid);
				amountPaid = 0;
			}

			// reset Player Script
			playerScript = null;

		}
	}
}
