using UnityEngine;
using System.Collections;

public class FishState : INPCState {

	private readonly StatePatternNPC npc;

	private float timeSpentFishing = 0f;

	public FishState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState (){
		npc.busy = true;
		Debug.Log("Fishing");
		Fish ();
	}

	public void OnTriggerEnter2D(Collider2D col){
	}

	public void ToIdleState(){
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
		npc.currentState = npc.offLoadState;
	}

	void Fish ()
	{
		Debug.Log ("Carry out fishing task");
		timeSpentFishing += Time.deltaTime;
		if (timeSpentFishing >= npc.blackboard.fishingTime) {
			ToOffloadState();
		}
	}

}
