using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2D : MonoBehaviour {

	private int[,] matrix;
	private int startX, startY;
	private int wantedPoint;
	private Renderer tRend;
	private Material tMat;
	// Use this for initialization
	void Start ()
	{
		startX = 10;
		startY = 10;
		matrix = new int[startX, startY];
		wantedPoint = 10;
		for (int i = 0; i < startX; i++)
		{
			for (int j = 0; j < startY; j++)
			{
				matrix[i, j] = i+j;
			}
		}
		
		FindWantedPoint(wantedPoint);
	}
	void FindWantedPoint(int wantedPoint)
	{
		for (int x = 0; x < startX; x++)
		{
			for (int y = 0; y < startY; y++)
			{
				if (matrix[x, y] == wantedPoint)
				{
					Debug.Log(matrix[x,y]);
					return;
				}
			}
		}
		Debug.Log("Can't find wantedPoint");
	}
	// Update is called once per frame
	void Update ()
	{
			
	}
}
