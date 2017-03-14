using UnityEngine;
using System.Collections;
using UnityEditor;

public class StatePatternNPC : MonoBehaviour {

	public Board blackboard;

	public Transform target;
	public NPCMove moveController;
	public bool arrived = false;

	[HideInInspector] public INPCState currentState;
	[HideInInspector] public IdleState idleState;
	[HideInInspector] public GetInstructionsState getInstructionsState;
	[HideInInspector] public ProcessState processState;
	[HideInInspector] public BuildState buildState;
	[HideInInspector] public FishState fishState;
	[HideInInspector] public GoState goState;
	[HideInInspector] public OffLoadState offLoadState;

	private void Awake(){
		idleState = new IdleState(this);
		getInstructionsState = new GetInstructionsState(this);
		processState = new ProcessState(this);
		buildState = new BuildState(this);
		fishState = new FishState(this);
		goState = new GoState(this);
		offLoadState = new OffLoadState(this);
	}

	// Use this for initialization
	void Start ()
	{
		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Board>();
		}
		moveController = GetComponent<NPCMove>();
		currentState = idleState;
	}
	
	// Update is called once per frame
	void Update () {
		currentState.UpdateState();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		currentState.OnTriggerEnter2D(col);
	}

	public void DestroyTarget (){
		Destroy(target.gameObject);
	}

	public void PlayProcessAnimation (int processType){
		Debug.Log ("Play the necessary animations for type: " + processType);
	}

	public void CreateProcessedResource (int processType){
		Debug.Log ("Instantiate a package for type: " + processType);
		GameObject prefab = blackboard.GetProcessedPrefab(processType);
		Instantiate(prefab, transform.position, Quaternion.identity);
	}

}
