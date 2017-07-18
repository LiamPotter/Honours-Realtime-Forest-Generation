using UnityEngine;
using System.Collections;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

/// <summary>
/// See http://libnoise.sourceforge.net/tutorials/tutorial5.html for an explanation
/// </summary>
public class Tutorial5 : MonoBehaviour 
{
	[SerializeField] Gradient _gradient;
	
	[SerializeField] float _left = 6;
	
	[SerializeField] float _right = 10;
	
	[SerializeField] float _top = 1;
	
	[SerializeField] float _bottom = 5;


	[SerializeField]
	private Terrain t;

	void Start() 
	{
		Generate();
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			Generate();
	}
	void Generate()
	{
		// STEP 1
		// Gradient is set directly on the object
		var mountainTerrain = new RidgedMultifractal();
		//RenderAndSetImage(mountainTerrain);

		// STEP 2
		var baseFlatTerrain = new Billow();
		baseFlatTerrain.Frequency = 2.0;
		RenderAndSetImage(baseFlatTerrain);



		// STEP 3
		var flatTerrain = new ScaleBias(0.125, -0.75, baseFlatTerrain);
		RenderAndSetImage(flatTerrain);


		// STEP 4
		var terrainType = new Perlin();
		terrainType.Frequency = 0.5;
		terrainType.Persistence = 0.25;

		var finalTerrain = new Select(flatTerrain, mountainTerrain, terrainType);
		finalTerrain.SetBounds(0, 1000);
		finalTerrain.FallOff = 0.125;
		ApplyToTerrain(finalTerrain);
		RenderAndSetImage(finalTerrain);
	}
	void RenderAndSetImage(ModuleBase generator)
	{
		var heightMapBuilder = new Noise2D(256, 256, generator);
		heightMapBuilder.GeneratePlanar(_left, _right, _top, _bottom);
		var image = heightMapBuilder.GetTexture(_gradient);
		GetComponent<Renderer>().material.mainTexture = image;
	}
	void ApplyToTerrain(ModuleBase generator)
	{
		var heightMapBuilder = new Noise2D(t.terrainData.heightmapWidth, t.terrainData.heightmapHeight, generator);
		heightMapBuilder.GeneratePlanar(_left, _right, _top, _bottom);
		t.terrainData.SetHeights(0, 0, heightMapBuilder.GetNormalizedData(true,0,0));
	}
}
