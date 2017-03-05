﻿using UnityEngine;
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

	void Start ()
	{
//		col = GetComponent<EdgeCollider2D>();
		GenerateMap();
	}

	public void GenerateMap(){
		newVerticies = new List<Vector2>();
		col = GetComponent<EdgeCollider2D>();
		float[] noiseMap = Noise.GenerateNoise(mapWidth, seed, noiseScale, octaves, persistance, lacunarity, offset);
		for (int i=0; i<noiseMap.Length; i++){
			newVerticies.Add( new Vector2(i, noiseMap[i]) );
		}

		col.points = newVerticies.ToArray();

		DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap));
	}

	public void DrawMesh(MeshData meshData){

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
