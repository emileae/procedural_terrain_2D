using UnityEngine;
using System.Collections;

public static class MeshGenerator {

	public static MeshData GenerateTerrainMesh (float[] heightMap, float heightMultiplier, float meshHeightOffset, float wellDepthCutoff, float wellDepth)
	{
		int width = heightMap.GetLength (0);
		//  height = 2 -> 2 rows of vertices
		int height = 2;// set this manually... only want a surface for now
		// want to center the mesh on the screen...
		float leftX = (width - 1) / -2f;

		MeshData meshData = new MeshData (width, height);
		int vertexIndex = 0;

		Color[] colors = new Color[meshData.vertices.Length];


		// https://youtu.be/4RpVBYW1r5M?t=10m13s ... just before this section of the video
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

//				Debug.Log("vertexIndex: " + vertexIndex);

				float platformDepth = 1;
//				if (y == 0) {
//					platformDepth = 1;
//				} else if (y == 1) {
//					platformDepth = -3;
//				}

				// set the wells
				float currentWellDepth = wellDepth;
				if (heightMap [x] > wellDepthCutoff) {
					currentWellDepth = 0;
				}
				
				meshData.vertices [vertexIndex] = new Vector3 (leftX + x, heightMap [x] * heightMultiplier * platformDepth + (y * meshHeightOffset) - currentWellDepth, 0);
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, 1f);

				if (x < width - 1 && y < height - 1 ) {
//					// add first triangle in quad
//					meshData.AddTriangle (vertexIndex, vertexIndex + width + 1, vertexIndex + width);
//					// add second triangle in quad
//					meshData.AddTriangle (vertexIndex + width + 1, vertexIndex, vertexIndex + 1);

					// add first triangle in quad
					meshData.AddTriangle (vertexIndex, vertexIndex + width, vertexIndex + width + 1);
					// add second triangle in quad
					meshData.AddTriangle (vertexIndex + width + 1, vertexIndex + 1, vertexIndex);
				}

				vertexIndex++;// increment this so that we know where we are in the 1D vertex array
			}
		}

//		Debug.Log("final vertexIndex: " + vertexIndex);

		return meshData;

	}
}

public class MeshData{
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData (int meshWidth, int meshHeight)
	{
		vertices = new Vector3[meshWidth * meshHeight];

//		Debug.Log("mesh data num vertices: " + vertices.Length);
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	public void AddTriangle(int a, int b, int c){

//		Debug.Log("triangle array length: " + triangles.Length);
//		Debug.Log("triangleIndex a: " + a);
//		Debug.Log("triangleIndex b: " + b);
//		Debug.Log("triangleIndex c: " + c);

		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;

		triangleIndex += 3;
	}

	public Mesh CreateMesh(){
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		return mesh;
	}
}
