using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class TG_HeightGeneration : MonoBehaviour {

	public bool DebugOnClick;

	[SerializeField]
	float frequency = 0.006f;
	[SerializeField]
	float amplitude = 0.01f;
	public bool useMountain1;
	[SerializeField]
	float mountainFrec1 = 0.021f;
	[SerializeField]
	float mountainAmp1 = 0.017f;
	public bool useMountain2;
	[SerializeField]
	float mountainFrec2 = 0.0115f;
	[SerializeField]
	float mountainAmp2 = 0.019f;
	public Terrain terr;

	void Start()
	{
		Generate();	
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0)&& DebugOnClick)
			Generate();
	}
	void Generate()
	{
		int xRes = terr.terrainData.heightmapWidth;
		int zRes = terr.terrainData.heightmapHeight;

		float[,] heights = new float[xRes, zRes];

		Perlin perlin = new Perlin();
		Perlin rigged = new Perlin();
		Billow bill = new Billow();
		Voronoi voro = new Voronoi();

		Noise2D noisemap = new Noise2D(xRes, zRes, perlin);

		//Continents---

		for (double z = 0; z < zRes; z++)
		{

			for (double x = 0; x < xRes ; x++)
			{

				double height = 0d;

				height += noisemap.Generator.GetValue(x * frequency, 0d, z * frequency) * amplitude;

				heights[(int)x, (int)z] = (float)height;

			}

		}

		terr.terrainData.SetHeights(0, 0, heights);

		//--------

		//Mountains---

		if (useMountain1)
		{
			Noise2D noisemap2 = new Noise2D(xRes/2, zRes/2, perlin);

			float[,] heights2 = new float[xRes, zRes];

			for (double z = 0; z < zRes; z++)
			{

				for (double x = 0; x < xRes; x++)
				{

					double height = 0d;

					//if (terr.terrainData.GetHeight((int)x, (int)z) > 1f)
					//{

					height += 0.02f + noisemap2.Generator.GetValue(x * mountainFrec1, 0d, z * mountainFrec1) * mountainAmp1;

					//}

					heights2[(int)x, (int)z] = (float)height;

				}

			}

			terr.terrainData.SetHeights(0, 0, heights2);
		}
		//--------*/

		//Mountains2------



		if (useMountain2)
		{
			Noise2D noisemap3 = new Noise2D(xRes, zRes, rigged);

			float[,] heights3 = new float[xRes, zRes];

			for (double z = 0; z < zRes; z++)
			{

				for (double x = 0; x < xRes; x++)
				{

					double height = 0d;

					//if (terr.terrainData.GetHeight((int)x, (int)z) > 0.1f)
					//{

					height += 0.02f + noisemap3.Generator.GetValue(x * mountainFrec2, 0d, z * mountainFrec2) * mountainAmp2;

					//}

					heights3[(int)x, (int)z] = (float)height;

				}

			}

			terr.terrainData.SetHeights(0, 0, heights3);

			//-----------*/
		
		}
		if(GetComponent<TG_SplatGeneration>().enabled)
			SendMessage("DoneHeightGeneration");
	}
	
}
