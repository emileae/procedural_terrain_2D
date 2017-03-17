using UnityEngine;
using System.Collections;

public class OffLoadState : INPCState {

	private readonly StatePatternNPC npc;

	public OffLoadState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState (){
		Debug.Log("Offloading");
		OffloadFish();
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

	void OffloadFish(){
		Debug.Log("Offload fish");
	}
}
