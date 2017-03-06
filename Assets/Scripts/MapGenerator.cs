using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

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

	void Start ()
	{
//		col = GetComponent<EdgeCollider2D>();
		GenerateMap();
	}

	public void GenerateMap(){
		newVerticies = new List<Vector2>();
		col = GetComponent<EdgeCollider2D>();
		float[] noiseMap = Noise.GenerateNoise(mapWidth, seed, noiseScale, octaves, persistance, lacunarity, offset);

		float leftX = (noiseMap.Length - 1) / -2f;

		for (int i=0; i<noiseMap.Length; i++){
			newVerticies.Add( new Vector2(i + leftX, noiseMap[i] * meshHeightMultiplier + meshHeightOffset) );
		}

		col.points = newVerticies.ToArray();

		DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightOffset));
	}

	public void DrawMesh(MeshData meshData){

		Mesh mesh = meshFilter.sharedMesh;
		mesh.Clear();
		meshFilter.sharedMesh = meshData.CreateMesh();
		
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
