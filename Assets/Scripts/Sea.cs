using UnityEngine;
using System.Collections;

public class Sea : MonoBehaviour {

	private BoxCollider2D bx2d;
	private Bounds bounds;

	// Use this for initialization
	void Start () {
		bx2d = GetComponent<BoxCollider2D>();
		bounds = GetComponent<MeshRenderer>().bounds;
		Debug.Log("Mesh bounds: " + bounds.size);
		Debug.Log("bx2d.size: " + bx2d.size);
		bx2d.size = new Vector2(bounds.size.x, bounds.size.y);
		Debug.Log("bx2d.size: " + bx2d.size);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
