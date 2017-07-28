using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureDebugger : MonoBehaviour {

	Terrain terr;
	TerrainData retrievedTerrainData;
	void Start()
	{
		terr = FindObjectOfType<Terrain>();
		retrievedTerrainData = terr.terrainData;
	}
	// Update is called once per frame
	void Update ()
	{
		RaycastHit hit;
		Physics.Raycast(transform.position, Vector3.down, out hit);
		CanPlaceOnSplat(hit.point);
	}
	private void CanPlaceOnSplat(Vector3 point)
	{
		int surfaceIndex = GetMainTexture(point);
		string surfaceTexture = retrievedTerrainData.splatPrototypes[surfaceIndex].texture.name;
		Debug.Log("CurrentSplat is: " + surfaceTexture);
	
	}
	private float[] GetTextureMix(Vector3 WorldPos)
	{
		// returns an array containing the relative mix of textures
		// on the main terrain at this world position.

		// The number of values in the array will equal the number
		// of textures added to the terrain.

		// calculate which splat map cell the worldPos falls within (ignoring y)
		int mapX = (int)(((WorldPos.x - terr.transform.position.x) / retrievedTerrainData.size.x)
			* retrievedTerrainData.alphamapWidth);
		int mapZ = (int)(((WorldPos.z - terr.transform.position.z) / retrievedTerrainData.size.z)
			* retrievedTerrainData.alphamapHeight);

		// get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
		float[,,] splatmapData = retrievedTerrainData.GetAlphamaps(mapX, mapZ, 1, 1);

		// extract the 3D array data to a 1D array:
		float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

		for (int n = 0; n < cellMix.Length; n++)
		{
			cellMix[n] = splatmapData[0, 0, n];
		}
		return cellMix;
	}
	public int GetMainTexture(Vector3 WorldPos)
	{
		// returns the zero-based index of the most dominant texture
		// on the main terrain at this world position.
		float[] mix = GetTextureMix(WorldPos);

		float maxMix = 0;
		int maxIndex = 0;

		// loop through each mix value and find the maximum
		for (int n = 0; n < mix.Length; n++)
		{
			if (mix[n] > maxMix)
			{
				maxIndex = n;
				maxMix = mix[n];
			}
		}
		return maxIndex;
	}

}
