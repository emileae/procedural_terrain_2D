using UnityEngine;
using System.Collections;

public class BuildState : INPCState {

	private readonly StatePatternNPC npc;

	private bool building = false;
	private NPCInstructions instructions;

	public BuildState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState ()
	{
		npc.busy = true;
		if (!building) {
			Build ();
		}

		if (npc.toIdle) {
			ToIdleState();
		}
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
	}

	public void ToBuildState(){
	}

	public void ToFishState(){
	}

	public void ToOffloadState(){
	}

	void Build(){
		building = true;
		instructions = npc.target.GetComponent<NPCInstructions>();
		npc.Build(instructions.resourceType, instructions.foundationType);
	}
}
