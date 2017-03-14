using UnityEngine;
using System.Collections;

public class FishState : INPCState {

	private readonly StatePatternNPC npc;

	public FishState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState (){
		Debug.Log("Fishing");
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
	}

}
