using UnityEngine;
using System.Collections;

public class FishController : MonoBehaviour {

	public GameObject fishPrefab;
	public GameObject sea;
	public Bounds seaBounds;
	public float seaBuffer = 1.0f;

	static int numFish = 20;
	public GameObject[] allFish = new GameObject[numFish];

	// Use this for initialization
	void Start () {

		seaBounds = sea.GetComponent<MeshRenderer>().bounds;

		for(int i = 0; i < numFish; i++)
		{
			Vector3 pos = new Vector3(Random.Range(seaBounds.min.x + seaBuffer,seaBounds.max.x - seaBuffer), Random.Range(seaBounds.min.y + seaBuffer,seaBounds.max.y - seaBuffer), -1);
			allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
