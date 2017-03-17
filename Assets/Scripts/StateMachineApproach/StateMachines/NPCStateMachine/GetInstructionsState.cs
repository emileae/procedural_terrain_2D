using UnityEngine;
using System.Collections;

public class GetInstructionsState : INPCState {

	private readonly StatePatternNPC npc;

	public GetInstructionsState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState (){
		npc.busy = true;
		CheckTarget();
	}

	public void OnTriggerEnter2D(Collider2D col){
	}

	public void ToIdleState(){
		npc.currentState = npc.idleState;
	}

	public void ToGoState(){
	}

	public void ToGetInstructionsState (){
	}

	public void ToProcessState (){
		npc.currentState = npc.processState;
	}

	public void ToBuildState(){
		npc.currentState = npc.buildState;
	}

	public void ToFishState(){
		npc.currentState = npc.fishState;
	}

	public void ToOffloadState(){
		npc.currentState = npc.offLoadState;
	}

	void CheckTarget ()
	{
		Debug.Log ("Check the NPC's target location and determine what to do");
		NPCInstructions instructions = npc.target.GetComponent<NPCInstructions> ();
		if (instructions != null) {
			Debug.Log ("Do the necessary actions for type: " + instructions.targetType);
			switch (instructions.targetType) {
				case 0:
					ToProcessState();
					break;
				case 1:
					ToBuildState();
					break;
				case 2:
					ToFishState();
					break;
				case 3:
					ToOffloadState();
					break;
				default:
					Debug.Log("Error - Fall Through State");
					break;
			}
		} else {
			Debug.Log("No instructions so just idle about....");
			ToIdleState();
		}
	}

}