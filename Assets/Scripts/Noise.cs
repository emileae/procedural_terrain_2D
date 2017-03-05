using UnityEngine;
using System.Collections;
using System.Xml.Linq;

public static class Noise {

	public static float[] GenerateNoise (int mapWidth, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
	{
		// add a seed to offset the noise generated so that the same result isn't returned each time
		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		// make sure scale isnt 0
		if (scale <= 0) {
			scale = 0.0001f;
		}
		float[] noiseMap = new float[mapWidth];

		// random y value for perlin noise:
//		int randomY = Random.Range (0, 100);
		int randomY = 0;

		// keep track of min max noise heights so that we can normalize the noise map
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;

		for (int x = 0; x < mapWidth; x++) {

			float amplitude = 1;
			float frequency = 1;
			float noiseHeight = 0;

			for (int i = 0; i < octaves; i++) {
				float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
				float sampleY = randomY / scale * frequency + octaveOffsets[i].y;

				float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;// *2 -1 so that we get some negative values
				noiseHeight += perlinValue * amplitude;
				amplitude *= persistance;
				frequency *= lacunarity;
			}

			if (noiseHeight > maxNoiseHeight) {
				maxNoiseHeight = noiseHeight;
			} else if (noiseHeight < minNoiseHeight) {
				minNoiseHeight = noiseHeight;
			}
			noiseMap [x] = noiseHeight;
		}

		for (int x = 0; x < mapWidth; x++) {
			noiseMap[x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x]); // InverseLerp normalizes the value in range 0-1
		}

		return noiseMap;

	}
}
