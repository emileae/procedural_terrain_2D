using UnityEngine;
using System.Collections;

public class globalFlock : MonoBehaviour {

	public GameObject fishPrefab;
	public static Vector3 tankDimensions = new Vector3(20, 20, 10);

	public GameObject fishTarget;

	static int numFish = 20;
	public static GameObject[] allFish = new GameObject[numFish];

//	public static Vector3 goalPos = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		fishTarget = GameObject.Find("FishTarget");

		for(int i = 0; i < numFish; i++)
		{
			Vector3 pos = new Vector3(Random.Range(fishTarget.transform.position.x - tankDimensions.x,fishTarget.transform.position.x + tankDimensions.x),
				Random.Range(fishTarget.transform.position.y - tankDimensions.y,fishTarget.transform.position.y + tankDimensions.y),
				Random.Range(-fishTarget.transform.position.z,fishTarget.transform.position.z));
			allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if(Random.Range(0,10000) < 50)
//		{
//			goalPos = new Vector3(Random.Range(-tankSize,tankSize),
//				                  Random.Range(-tankSize,tankSize),
//				                  0);
//		}
	}
}
