using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TG_SplatGeneration : MonoBehaviour {
	public Terrain theTerrain;
	private TerrainData terrainData;
	[SerializeField]
	private int splatForX, splatForY, splatForZ;
	[SerializeField]
	private float blendScale;
	[Range(0,10)]
	public float splatHeight;

	void Start () {
		terrainData = theTerrain.terrainData;
		if (!GetComponent<TG_HeightGeneration>().enabled)
			AssignSplats();

	}
	void Update()
	{
		if (!GetComponent<TG_HeightGeneration>().enabled && Input.GetMouseButtonDown(0))
			AssignSplats();
	}
	void AssignSplats()
	{
		float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

		for (int y = 0; y < terrainData.alphamapHeight; y++)
		{
			for (int x = 0; x < terrainData.alphamapWidth; x++)
			{
				float height = terrainData.GetHeight(x, y);
				Vector3 splat = new Vector3(0, 1, 0);
				
				if(height>splatHeight)
				{
					splat = Vector3.Lerp(splat, new Vector3(0, 0, 1), (height -splatHeight) * blendScale);
				}
				else
				{
					splat = Vector3.Lerp(splat, new Vector3(1, 0, 0), height * blendScale);
				}
				splat.Normalize();
				splatmapData[x, y, splatForX] = splat.x;
				splatmapData[x, y, splatForY] = splat.y;
				splatmapData[x, y, splatForZ] = splat.z;
			}
		}

		// Finally assign the new splatmap to the terrainData:
		terrainData.SetAlphamaps(0, 0, splatmapData);
		if(GetComponent<TG_SplatPlacement>().enabled)
			SendMessage("DoneSplatGeneration");
	}
	public void DoneHeightGeneration()
	{
		Debug.Log("Message Sent and Recieved");
		AssignSplats();
	}
}
