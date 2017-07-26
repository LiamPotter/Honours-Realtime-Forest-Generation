using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
public class TG_HeightGeneration : MonoBehaviour {

	
	public bool DebugOnClick; //should the terrain be re-generate whenever the user clicks
	
	public bool RandomizeValues; //should the generator's frequency and amplitude be randomized?

	//public float[] randomValues = new float[9];
	[HideIf("RandomizeValues", true)]
	public float frequency = 0.006f; //the frequency used to generate the base perlin texture
	[HideIf("RandomizeValues", true)]
	public float amplitude = 0.01f; //the amplitude used to generate the base perlin texture

	[ShowIf("RandomizeValues",true)]
	[MinMaxSlider(0.001f, 0.1f, false)]
	public Vector2 baseRandomFrequency;

	[ShowIf("RandomizeValues", true)]
	[MinMaxSlider(0.001f, 0.1f, false)]
	public Vector2 baseRandomAmplitude;

	public bool useMountain1; //should the first mountain be generated?

	[BoxGroup("Mountain1",true,true,0)]
	[ShowIf("useMountain1", true)]
	public float mountainFrec1 = 0.021f; //the frequency used to generate the first mountain perlin texture

	[BoxGroup("Mountain1", true, true, 0)]
	[ShowIf("useMountain1", true)]
	public float mountainAmp1 = 0.017f; //the amplitude used to generate the first mountain perlin texture

	[BoxGroup("Mountain1", true, true, 0)]
	[ShowIf("useMountain1", true)]
	[MinMaxSlider(0.001f, 0.1f, false)]
	public Vector2 mountain1RandomFrequency;

	[BoxGroup("Mountain1", true, true, 0)]
	[ShowIf("useMountain1",true)]
	[MinMaxSlider(0.001f, 0.1f, false)]
	public Vector2 mountain1RandomAmplitude;
	
	public bool useMountain2; //should the second mountain be generated?

	[BoxGroup("Mountain2", true, true, 0)]
	[ShowIf("useMountain2", true)]
	public float mountainFrec2 = 0.0115f;//the frequency used to generate the second mountain perlin texture

	[BoxGroup("Mountain2", true, true, 0)]
	[ShowIf("useMountain2", true)]
	public float mountainAmp2 = 0.019f;//the amplitude used to generate the second mountain perlin texture

	[BoxGroup("Mountain2", true, true, 0)]
	[ShowIf("useMountain2", true)]
	[MinMaxSlider(0.001f, 0.1f, false)]
	public Vector2 mountain2RandomFrequency;

	[BoxGroup("Mountain2", true, true, 0)]
	[ShowIf("useMountain2", true)]
	[MinMaxSlider(0.001f, 0.1f, false)]
	public Vector2 mountain2RandomAmplitude;

	public Terrain terr; //the refence to the terrain this class will be effecting

	void Start()
	{
		if (!terr)
			terr = FindObjectOfType<Terrain>();
		Generate();
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0)&& DebugOnClick)
			Generate();
	}
	//the method that will be generating the heights
	void Generate()
	{
		if(RandomizeValues)
		{
			frequency = Random.Range(baseRandomFrequency.x, baseRandomFrequency.y);
			amplitude = Random.Range(baseRandomAmplitude.x, baseRandomAmplitude.y);
			mountainFrec1 = Random.Range(mountain1RandomFrequency.x, mountain1RandomFrequency.y);
			mountainAmp1 = Random.Range(mountain1RandomAmplitude.x, mountain1RandomAmplitude.y);
			mountainFrec2 = Random.Range(mountain2RandomFrequency.x, mountain2RandomFrequency.y);
			mountainAmp2 = Random.Range(mountain2RandomAmplitude.x, mountain2RandomAmplitude.y);
		}
		int xRes = terr.terrainData.heightmapWidth; //width of the terrain heightmap
		int zRes = terr.terrainData.heightmapHeight; //height of the terrain heightmap

		float[,] heights = new float[xRes, zRes]; //2D array to be used when applying to the terrain heightmap

		Perlin perlin = new Perlin(); //libNoise perlin texture generator for the initial generation and the first mountain
		Perlin rigged = new Perlin(); //libNoise perlin texture generator for the second mountain

		Noise2D noisemap = new Noise2D(xRes, zRes, perlin); //libNoise 2D Noise Map using the perlin texture generator

		//Initial generation

		for (double z = 0; z < zRes; z++)
		{

			for (double x = 0; x < xRes ; x++)
			{

				double height = 0d; //height value used in generation

				height += noisemap.Generator.GetValue(x * frequency, 0d, z * frequency) * amplitude; //noiseMap Generator is added to height used amplitude and frequency

				heights[(int)x, (int)z] = (float)height; //height assigned to the 2D array of heights

			}

		}

		terr.terrainData.SetHeights(0, 0, heights); // terrainData is affected using SetHeights and the 2D array of heights

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
			SendMessage("DoneHeightGeneration"); //send message to the next generator stating generation is finished
	}
	
	
}
