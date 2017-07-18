using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TG_SplatPlacement : MonoBehaviour {
	//Terrain to place on
	public Terrain terrain;
	//value of detail index to spawn
	public int detailIndexToMassPlace;
	//amount of splat textures on the terrain the system will affect
	public int[] splatTextureIndicesToAffect;
	//how many details will be placed per pixel on each splatmap
	public int detailCountPerDetailPixel = 0;
	//how many details will be placed based on the power of each pixel
	[Range(1,5)]
	public int detailPlacementPower;

	//the resolution of the detail maps
	private int detailResolution;
	// Use this for initialization
	void Start () {
		detailResolution = terrain.terrainData.detailResolution;
		ClearSplat();
		//PlaceOnSplat();
	}
	void OnDisable()
	{
		//ClearSplat();
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void PlaceOnSplat()
	{
		if (!terrain)
		{
			Debug.Log("You have not selected a terrain object");
			return;
		}

		if (detailIndexToMassPlace >= terrain.terrainData.detailPrototypes.Length)
		{
			Debug.Log("You have chosen a detail index which is higher than the number of detail prototypes in your detail libary. Indices starts at 0");
			return;
		}

		if (splatTextureIndicesToAffect.Length > terrain.terrainData.splatPrototypes.Length)
		{
			Debug.Log("You have selected more splat textures to paint on, than there are in your libary.");
			return;
		}

		for (int i = 0; i < splatTextureIndicesToAffect.Length; i++)
		{
			if (splatTextureIndicesToAffect[i] >= terrain.terrainData.splatPrototypes.Length)
			{
				Debug.Log("You have chosen a splat texture index which is higher than the number of splat prototypes in your splat libary. Indices starts at 0");
				return;
			}
		}

		if (detailCountPerDetailPixel > 16)
		{
			Debug.Log("You have selected a non supported amount of details per detail pixel. Range is 0 to 16");
			return;
		}
		//get size of the terrainAlphamap
		int alphamapWidth = terrain.terrainData.alphamapWidth;
		int alphamapHeight = terrain.terrainData.alphamapHeight;
		//get the size of the detail map
		int detailWidth = terrain.terrainData.detailResolution;
		int detailHeight = detailWidth;

		//aplhamap width divided by detail width
		float resolutionDiffFactor = (float)alphamapWidth / detailWidth;

		//a reference to the terrains splatmap
		float[,,] splatmap = terrain.terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);

		//new two dimensional array for the new detail layer
		int[,] newDetailLayer = new int[detailWidth, detailHeight];

		//loop through splatTextures
		for (int i = 0; i < splatTextureIndicesToAffect.Length; i++)
		{

			//find where the texture is present
			for (int j = 0; j < detailWidth; j++)
			{

				for (int k = 0; k < detailHeight; k++)
				{
					if (j % detailPlacementPower == 1&&k% detailPlacementPower == 1||detailPlacementPower==1)
					{
						float alphaValue = splatmap[(int)(resolutionDiffFactor * j), (int)(resolutionDiffFactor * k), splatTextureIndicesToAffect[i]];
						newDetailLayer[j, k] = (int)Mathf.Round(alphaValue * ((float)detailCountPerDetailPixel)) + newDetailLayer[j, k];
					}
				}

			}

		}
		//assign terrain detail layer
		terrain.terrainData.SetDetailLayer(0, 0, detailIndexToMassPlace, newDetailLayer);
		if (GetComponent<T_PointSystem>().enabled)
			SendMessage("DoneSplatPlacement");
	}
	//clear the detail layers
	public void ClearSplat()
	{
		int detailWidth = detailResolution;
		int detailHeight = detailWidth;
		int[,] newDetailLayer = new int[detailWidth, detailHeight];
		if (!terrain)
			terrain = FindObjectOfType<Terrain>();
		terrain.terrainData.SetDetailLayer(0, 0, 0, newDetailLayer);
	}
	public void DoneSplatGeneration()
	{
		ClearSplat();
		PlaceOnSplat();
	}
}
