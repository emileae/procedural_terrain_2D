using UnityEngine;
using System.Collections;

public class BuildState : INPCState {

	private readonly StatePatternNPC npc;

	public BuildState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState (){
		Debug.Log("Building");
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
