using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class IdleState : INPCState {

	private readonly StatePatternNPC npc;
	private bool enteringIdle = true;

	public IdleState (StatePatternNPC statePatternNPC)
	{
		npc = statePatternNPC;
	}


	public void UpdateState ()
	{
		npc.busy = false;
		npc.toIdle = false;
		Debug.Log("entering Idle()? " + enteringIdle);
		// if npc is being sent to the idle state then reset target
		if (enteringIdle) {
			enteringIdle = false;
			if (npc.target != null) {
				npc.target = null;
			}
		}
		ListenForTarget();
	}

	public void OnTriggerEnter2D (Collider2D col)
	{
		
	}

	// States - START
	public void ToIdleState(){
		Debug.Log("Can't transition to the same state... IdleState");
	}

	public void ToGoState(){
		enteringIdle = true;
		npc.currentState = npc.goState;
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
	// States - END


	private void ListenForTarget ()
	{
		if (npc.target != null) {
			ToGoState ();
		} else {
			Idle ();
		}
	}

	void Idle ()
	{
		if (npc.moveController.direction == 0) {
			if (Random.Range (0, 1) > 0.5) {
				npc.moveController.direction = 1;
			} else {
				npc.moveController.direction = -1;
			}
		}
	}
}
