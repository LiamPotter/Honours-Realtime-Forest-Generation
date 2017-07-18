using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHeightDebugger : MonoBehaviour {
	private Terrain terrainToDebug;

	private RaycastHit rayHit;
	void Start()
	{
		terrainToDebug = FindObjectOfType<Terrain>();
	}
	// Update is called once per frame
	void Update ()
	{
		if(Physics.Raycast(transform.position,Vector3.down,out rayHit))
		{
			if(rayHit.collider.gameObject==terrainToDebug.gameObject)
			{
				Debug.Log(terrainToDebug.terrainData.GetHeight((int)rayHit.point.x, (int)rayHit.point.z));
			}
		}	
	}
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, Vector3.down * 100);
		if (rayHit.collider != null)
			Gizmos.DrawSphere(rayHit.point, 0.25f);

	}
}
