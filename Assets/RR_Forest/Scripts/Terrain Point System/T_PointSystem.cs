using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_PointSystem : MonoBehaviour {

	public Terrain WantedTerrain;
	public LayerMask terrainLayerMask;
	private TerrainData retrievedTerrainData;
	public GameObject PointPrefab;
	public List<GameObject> TreePrefabs;
	public int PointAmount =5;
	public float ClusterAmount = 0.25f;
	public float TreeMinimumDistance;

	public List<GameObject> GrassPrefabs;
	public float GrassMiniumumDistance;
	public Vector3 grassOffset;
	//public Dictionary<int, T_Point> Points = new Dictionary<int, T_Point>();
	public List<T_Point> Points = new List<T_Point>();
	private float terrainRows;
	private float terrainColumns;
	
	void Start ()
	{
		GrabTerrainData();
		//CalculateRowsAndColumns();
		//PlacePoints();
		//PlacePoissonPoints(TreeMinimumDistance, TreePrefabs,Vector3.zero);
		//PlacePoissonPoints(GrassMiniumumDistance, GrassPrefabs,grassOffset,100,100);
		//Debug.Log(Points.Count);
	}
	
	private void GrabTerrainData()
	{
		retrievedTerrainData = WantedTerrain.terrainData;
	}
	private void CalculateRowsAndColumns()
	{
		terrainRows = (Mathf.Sqrt(retrievedTerrainData.size.x));
		terrainColumns = (Mathf.Sqrt(retrievedTerrainData.size.z));
		//gridPositions = new List<Vector3>((terrainColumns+1) * (terrainRows+1));
	}
	#region First Attempt, Random Placement
	//The first attempt at random placement, it places points and randomly places between it's own position
	//and the position of the next point

	private void PlacePoints()
	{
		for (int i = 0, z = 0; z <= terrainColumns; z++)
		{
			for (int x = 0; x <= terrainRows; x++, i++)
			{
				Vector3 tGridPos = new Vector3(
					x * terrainRows,
					retrievedTerrainData.GetHeight(Mathf.CeilToInt(x * terrainRows), Mathf.CeilToInt(z * terrainColumns)),
					z * terrainColumns);
				Vector3 nextGridPos;
				if (x + 1 < terrainRows && z + 1 < terrainColumns)
				{
					nextGridPos = new Vector3(x + 1 * terrainRows,
					retrievedTerrainData.GetHeight(Mathf.CeilToInt(x * terrainRows), Mathf.CeilToInt(z * terrainColumns)),
					z + 1 * terrainColumns);
				}
				else nextGridPos = Vector3.zero;
				Vector3 randGridPos = new Vector3(
					tGridPos.x + (Random.Range(TreeMinimumDistance, nextGridPos.x) * ClusterAmount),
					tGridPos.y,
					tGridPos.z + (Random.Range(TreeMinimumDistance, nextGridPos.z) * ClusterAmount)
					);
				randGridPos.y = retrievedTerrainData.GetHeight(Mathf.CeilToInt(randGridPos.x), Mathf.CeilToInt(randGridPos.z));
				Points.Add(new T_Point(randGridPos));
				Instantiate(PointPrefab, Points[i].vectorPosition, PointPrefab.transform.rotation, transform);
			}
		}
	}
	#endregion
	private void PlacePoissonPoints(float minDistance,List<GameObject> prefabsToSpawn,Vector3 offset)
	{
		RaycastHit tHit;
		Vector3 rayDirection;
		Vector3 tPosition;
		GameObject instObject;

		PoissonDiscSampler pSampler = new PoissonDiscSampler(retrievedTerrainData.size.x, retrievedTerrainData.size.z, minDistance);
		foreach (Vector2 sample in pSampler.Samples())
		{
			Points.Add(new T_Point((int)sample.x, Mathf.CeilToInt(retrievedTerrainData.GetHeight((int)sample.x, (int)sample.y)), (int)sample.y));
		}
		for (int i = 0; i < Points.Count; i++)
		{

			tPosition = GetActualTerrainHeight(Points[i].vectorPosition, out tHit, out rayDirection);
			instObject = Instantiate(GrabRandomFromList(prefabsToSpawn), tPosition + offset, Quaternion.identity, transform);
			Vector3 eTest = SetPointRotation(tHit, rayDirection, instObject.transform.rotation.eulerAngles);
			Quaternion euler = Quaternion.Euler(eTest.x, eTest.y, eTest.z);

			//instObject.transform.up = tHit.normal;
			
			//instObject.transform.up = tHit.normal+new Vector3(0,Random.Range(0,360),0);
			//instObject.transform.localRotation.SetLookRotation(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), tHit.normal);
		}
	}
	
	private void PlacePoissonPoints(float minDistance, List<GameObject> prefabsToSpawn,Vector3 offset,int xDim,int yDim)
	{
		RaycastHit tHit;
		Vector3 rayDirection;
		Vector3 tPosition;
		GameObject instObject;

		PoissonDiscSampler pSampler = new PoissonDiscSampler(xDim, yDim, minDistance);
		foreach (Vector2 sample in pSampler.Samples())
		{
			Points.Add(new T_Point((int)sample.x, Mathf.CeilToInt(retrievedTerrainData.GetHeight((int)sample.x, (int)sample.y)), (int)sample.y));
		}
		for (int i = 0; i < Points.Count; i++)
		{

			tPosition = GetActualTerrainHeight(Points[i].vectorPosition, out tHit, out rayDirection);
			instObject = Instantiate(GrabRandomFromList(prefabsToSpawn), tPosition + offset, Quaternion.identity, transform);
			Vector3 eTest = SetPointRotation(tHit, rayDirection, instObject.transform.rotation.eulerAngles);
			Quaternion euler = Quaternion.Euler(eTest.x, eTest.y, eTest.z);
			//Debug.Log(euler);
			instObject.transform.up = tHit.normal;
		}
	}
	private GameObject GrabRandomFromList(List<GameObject> pList)
	{
		int r = Random.Range(0, pList.Count);
		return pList[r];
	}
	private Vector3 GetActualTerrainHeight(Vector3 initPos,out RaycastHit rayHit,out Vector3 rayDirection)
	{
		RaycastHit hit;
		Ray tRay = new Ray(initPos+(Vector3.up*10), Vector3.down);
		if(Physics.Raycast(tRay,out hit,200f,terrainLayerMask.value))
		{
			rayHit = hit;
			rayDirection = tRay.direction;
			return hit.point;
		}
		rayHit = hit;
		rayDirection = tRay.direction;
		return initPos;
	}
	private Vector3 SetPointRotation(RaycastHit inputHit,Vector3 rayDir,Vector3 initialRotation)
	{
		Vector3 tRotation;
		tRotation = Vector3.Reflect(rayDir,inputHit.normal);
		return tRotation;
	}
	public void DoneSplatPlacement()
	{
		Debug.Log("Done Splat Placement, starting poisson placement");
		PlacePoissonPoints(TreeMinimumDistance, TreePrefabs, Vector3.zero);
	}
	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.blue;
	//	for (int i = 0; i < Points.Count; i++)
	//	{
	//		Gizmos.DrawCube(new Vector3(Points[i].xPosition,Points[i].yPosition,Points[i].zPosition), Vector3.one*2);
	//	}
	//}

}
