using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Point{
	public int xPosition;
	public int yPosition;
	public int zPosition;
	public Vector3 vectorPosition;
	public T_Point(int x,int y, int z)
	{
		xPosition = x;
		yPosition = y;
		zPosition = z;
		vectorPosition = new Vector3(x, y, z);
	}
	public T_Point(Vector3 vector)
	{
		xPosition = (int)vector.x;
		yPosition = (int)vector.y;
		zPosition = (int)vector.z;
		vectorPosition = vector;
	}
	public override string ToString()
	{
		return "Point X: "+xPosition+", Point Y: "+yPosition+", Point Z: "+zPosition;
	}

}
