using UnityEngine;
using System.Collections;

public class ProcessState : INPCState {

	private readonly StatePatternNPC npc;
	private bool startedProcessing = false;
	private float processTime = 5.0f;
	private float spentProcessingTime = 0.0f;

	private NPCInstructions instructions;

	public ProcessState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}

	public void UpdateState ()
	{
		Debug.Log ("Processing");
		if (!startedProcessing) {
			startedProcessing = true;
			ProcessTarget ();
		}

		spentProcessingTime += Time.deltaTime;
		if (spentProcessingTime >= processTime) {
			StopProcessing();
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

	void ProcessTarget (){
		instructions = npc.target.GetComponent<NPCInstructions> ();
		if (instructions != null) {
			npc.PlayProcessAnimation(instructions.resourceType);
		} else {
			Debug.Log ("No instructions so just idle about....");
			ToIdleState ();
		}
	}

	void StopProcessing(){
		npc.CreateProcessedResource(instructions.resourceType);
		npc.DeactivateTarget();
		spentProcessingTime = 0f;
		ToIdleState();
	}

}
