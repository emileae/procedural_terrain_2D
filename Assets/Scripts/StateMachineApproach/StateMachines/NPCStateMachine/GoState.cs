using UnityEngine;
using System.Collections;

public class GoState : INPCState {

	private readonly StatePatternNPC npc;

	private GeneralBuilding buildingScript;

	public GoState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState ()
	{
		if (!npc.arrived) {
			FindDirection ();
		}
		CheckArrival ();
	}

	public void OnTriggerEnter2D (Collider2D col)
	{
		Debug.Log ("Trigger enter");
		if (col.gameObject == npc.target.gameObject) {
			npc.arrived = true;
		}
	}

	public void ToIdleState(){
		npc.currentState = npc.idleState;
	}

	public void ToGoState(){
	}

	public void ToGetInstructionsState (){
		npc.currentState = npc.getInstructionsState;
	}

	public void ToProcessState (){
	}

	public void ToBuildState(){
	}

	public void ToFishState(){
	}

	public void ToOffloadState(){
	}

	void FindDirection ()
	{
		if (npc.target != null) {
			Debug.Log ("Find target direction");
			if (npc.gameObject.transform.position.x > npc.target.position.x) {
				npc.moveController.direction = -1;
			} else if (npc.gameObject.transform.position.x < npc.target.position.x) {
				npc.moveController.direction = 1;
			}
		} else {
			ToIdleState();
		}
	}

	void CheckArrival ()
	{
		if (npc.target != null) {
			if (npc.arrived) {
				npc.moveController.direction = 0;
				Arrived ();
			}
			float distanceFromTarget = (npc.gameObject.transform.position - npc.target.position).magnitude;
//			Debug.Log("distanceFromTarget: " + distanceFromTarget + "  - " + (distanceFromTarget < npc.moveController.stopDistance));
			if (distanceFromTarget < npc.moveController.stopDistance) {
				npc.arrived = true;
				npc.moveController.direction = 0;
				Arrived ();
			}
		}
	}

	void Arrived ()
	{
		// reset arrival boolean
		npc.arrived = false;
		if (npc.target != null) {
			Debug.Log("Arrived at target.... do something here");
			ToGetInstructionsState ();
			//
		} else {
			ToIdleState();
		}
	}
}
