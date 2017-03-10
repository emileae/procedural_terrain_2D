using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	public Blackboard blackboard;

	public bool payable = true;
	public bool building = false;
	public float buildTime = 10.0f;
	public GameObject package;
	public bool isPackage = false;

	// Use this for initialization
	void Start () {

		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Blackboard>();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartBuilding ()
	{
		building = true;
		payable = false;

		blackboard.AddToBuildingList(gameObject);

	}

	public void FinishedBuilding(){
		Debug.Log("Finished building... instantiate package maybe add a particle effect?");
		GameObject buildingPackage = Instantiate(package, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity) as GameObject;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "NPC") {
			NPCController npcScript = go.GetComponent<NPCController> ();
			if (npcScript.targetGameObject == gameObject) {
				Debug.Log("Arrived at target building");
				npcScript.state = 2;
				npcScript.StartBuilding(buildTime);
			}
		}
	}

	// TODO: fix so that it stops NPC from continuing to count the build process if NPC is removed from building collider
	void OnTriggerExit2D (Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.tag == "NPC") {
			NPCController npcScript = go.GetComponent<NPCController> ();
			if (npcScript.targetGameObject == gameObject) {
				if (npcScript.state == 2) {
					// time to build is totally reset
					npcScript.StopBuilding();
				}
			}
		}
	}

}
