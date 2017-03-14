using UnityEngine;
using System.Collections;

public interface INPCState {

	void UpdateState();

	void OnTriggerEnter2D(Collider2D col);

	void ToIdleState();

	void ToGoState();

	void ToGetInstructionsState();

	void ToProcessState();

	void ToBuildState();

	void ToFishState();

	void ToOffloadState();
}
