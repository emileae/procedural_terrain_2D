using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public Blackboard blackboard;

	public int mapWidth;
	public float noiseScale;

	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	// create edge colliders
	public List<Vector2> newVerticies = new List<Vector2>();
    private EdgeCollider2D col;

    // auto update
    public bool autoUpdate;

    // for the mesh
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public float meshHeightMultiplier;
    public float meshHeightOffset;

    public float wellDepthCutoff = 0.1f;
	public float wellDepth = 50f;

	// PRefabs
	public GameObject shoreLine;
	public GameObject tree;
	public GameObject well;
	public GameObject fishing;

	public GameObject instantiatedPoints;

	void Start ()
	{
		if (blackboard == null) {
			blackboard = GameObject.Find("Blackboard").GetComponent<Blackboard>();
		}
		// moved this to the blackboard
//		GenerateMap();

	}

	public void GenerateMap ()
	{
		// remove all previously generated prefabs
		foreach (Transform child in instantiatedPoints.transform) {
			GameObject.DestroyImmediate (child.gameObject);
		}


		newVerticies = new List<Vector2> ();
		col = GetComponent<EdgeCollider2D> ();
		float[] noiseMap = Noise.GenerateNoise (mapWidth, seed, noiseScale, octaves, persistance, lacunarity, offset);

		//			Set an array for randomised points of interest
		Debug.Log ("noiseMap: " + noiseMap.Length);
		// how many edge points make up 1 unit
		int edgePointsPerUnit = 4;
		string[] pointsOfInterest = SetPointsArray (noiseMap, edgePointsPerUnit);

		float leftX = (noiseMap.Length - 1) / -2f;

		for (int i = 0; i < noiseMap.Length; i++) {
			float currentWellDepth = wellDepth;
//			if (noiseMap [i] > wellDepthCutoff) {
//				currentWellDepth = 0;
//			}

//			if (i % 4 == 0) {
//				Debug.Log ("Potentially add a point here");
//
////				if (noiseMap [i] > wellDepthCutoff) {
////					currentWellDepth = 0;
////				}
//
////				float randFloat = Random.Range (0f, 1f);
////
////				Debug.Log("randFloat " + randFloat);
//
////				GameObject wellPoint = Instantiate(well, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.identity) as GameObject;
////				wellPoint.transform.parent = instantiatedPoints.transform;
//
//				// change to a switch statement
////				if (randFloat > 0.5f) {
////					GameObject treePoint = Instantiate (tree, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
////					treePoint.transform.parent = instantiatedPoints.transform;
////					Debug.Log("Place a tree: " + i);
////				}else if (randFloat < 0.2f) {
////					GameObject wellPoint = Instantiate(well, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.identity) as GameObject;
////					wellPoint.transform.parent = instantiatedPoints.transform;
////					Debug.Log("Place a well: " + i);
////				}
//			}

			// this is the shoreline, NPCs must stop here
			if (i == 0 || i == (noiseMap.Length - 1)) {
				GameObject shoreLinePoint = Instantiate (shoreLine, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
				shoreLinePoint.transform.parent = instantiatedPoints.transform;
			}

			if (i % edgePointsPerUnit == 0 && i != 0) {
				string pointType = pointsOfInterest [i / edgePointsPerUnit];
				switch (pointType)
		        {
		        case "tree":
					GameObject treePoint = Instantiate (tree, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.Euler (0, 180, 0)) as GameObject;
					treePoint.transform.parent = instantiatedPoints.transform;
		            break;
		        case "well":
					GameObject wellPoint = Instantiate(well, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.identity) as GameObject;
					wellPoint.transform.parent = instantiatedPoints.transform;
		            break;
				case "fishing":
					GameObject fishingPoint = Instantiate(fishing, new Vector3 (i + leftX, noiseMap [i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth, 0), Quaternion.identity) as GameObject;
					fishingPoint.transform.parent = instantiatedPoints.transform;
					blackboard.AddGameObjectToList(fishingPoint, blackboard.fishingSpots);
		            break;
		        default:
		            print ("Nothing.");
		            break;
		        }
			}


			newVerticies.Add( new Vector2(i + leftX, noiseMap[i] * meshHeightMultiplier + meshHeightOffset - currentWellDepth) );
		}

		col.points = newVerticies.ToArray();

		DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightOffset, wellDepthCutoff, wellDepth));
	}

	public void DrawMesh(MeshData meshData){

		Mesh mesh = meshFilter.sharedMesh;
		mesh.Clear();
		meshFilter.sharedMesh = meshData.CreateMesh();

	}

	string[] SetPointsArray (float[] noiseMap, int edgePointsPerUnit)
	{
//		Debug.Log("Place " + noiseMap.Length + " points");
		// center x positions
		float leftX = (noiseMap.Length - 1) / -2f;

		string[] points = new string[(noiseMap.Length - (noiseMap.Length % edgePointsPerUnit)) / edgePointsPerUnit];

		// number of units to place
		int NumWells = 5;
		int NumFishingSpots = 2;
		int numTrees = 12;

		for (int i = 0; i < points.Length; i++) {
//			Debug.Log("Settings point " + i);
			if (NumWells > 0) {
				points [i] = "well";
				NumWells -= 1;
			} else if (numTrees > 0) {
				points [i] = "tree";
				numTrees -= 1;
			}else if (NumFishingSpots > 0) {
				points [i] = "fishing";
				NumFishingSpots -= 1;
			} else {
				points [i] = "null";
			}
		}

		// Randomize a list
		for (int i = 0; i < points.Length; i++) {
			string temp = points [i];
			int randomIndex = Random.Range (i, points.Length);
			points [i] = points [randomIndex];
			points [randomIndex] = temp;
		}

		return points;

	}

	void OnValidate ()
	{
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}

}
