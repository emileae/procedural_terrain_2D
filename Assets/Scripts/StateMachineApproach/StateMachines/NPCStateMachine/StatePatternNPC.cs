using UnityEngine;
using System.Collections;
using UnityEditor;

public class StatePatternNPC : MonoBehaviour {

	public Board blackboard;

	public bool toIdle = false;

	public bool busy = false;

	public Transform target;
	public NPCMove moveController;
	public bool arrived = false;
	public int platform = 0;// the platform NPC is standing on

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

	public void DeactivateTarget (){
		target.gameObject.SetActive(false);
		target = null;
	}

	public void PlayProcessAnimation (int resourceType){
		Debug.Log ("Play the necessary animations for type: " + resourceType);
	}

	public void CreateProcessedResource (int resourceType){
		Debug.Log ("Instantiate a package for type: " + resourceType);
		GameObject prefab = blackboard.GetProcessedPrefab(resourceType);
		Instantiate(prefab, target.position, Quaternion.identity);
	}

	public void Build(int targetType, int foundationType){
		Debug.Log ("Play the necessary animations for building: ");
		Debug.Log ("Building for type: " + targetType + " and foundation: " + foundationType);
		GameObject prefab = blackboard.GetBuildingPrefab(targetType, foundationType, platform);
		StartCoroutine(BuildStructure(prefab));
	}

	IEnumerator BuildStructure(GameObject prefab){
		yield return new WaitForSeconds(prefab.GetComponent<Structure>().buildTime);
		GameObject instantiatedPrefab = Instantiate(prefab, target.position, Quaternion.identity) as GameObject;// cast it as a gameobject... otherwise seems to be trnasform.position bugs
		Debug.Log("Structure located at: " + instantiatedPrefab.transform.position);
		toIdle = true;
		// activate the structure's functionality
		Structure structureScript = instantiatedPrefab.GetComponent<Structure>();
		structureScript.Activate();
	}

}
