using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LC_Scaler : MonoBehaviour {

	private LC_TimeUser timeUser;

	public Vector3 scaleWeight = Vector3.one; //How much the user's age should effect it's scale

	public Vector3 maxScale =Vector3.one; //The max scale this user can get to

	public Vector3 initialScale = new Vector3(0.1f,0.1f,0.1f); //The user's initial scale, when they first get placed

	//[Range(0.01f,4f)]
	public float scaleSpeed=1; //How fast the user should scale to it's wanted scale;

	public Vector3 currentScale; //The user's currently used scale

	private Vector3 previousScale; //The user's previous scale, gets set whenever the user ages

	private Vector3 wantedScale = new Vector3(); //The scale the user will try to get to

	private bool shouldScale; //Should the user be scaleing right now?

	private float scaleTimer; //The timer used when lerping from 'currentScale' to 'wantedScale'
	
	void Awake()
	{
		timeUser = GetComponent<LC_TimeUser>();
		currentScale = initialScale;
		transform.localScale = currentScale;
		wantedScale = currentScale;

	}
	// Update is called once per frame
	void Update ()
	{
		//scaleTimer += (Time.deltaTime * scaleSpeed)*timeUser.agePercentage;
		currentScale = Vector3.Lerp(initialScale, maxScale, timeUser.agePercentage);
		transform.localScale = currentScale;
		//if (shouldScale)
		//{
		//	//scaleSpeed = Mathf.Abs(Mathf.Tan(timeUser.ageingSpeed+timeUser.lifeTimeScale));

		//	//scaleTimer += Time.deltaTime*scaleSpeed ;
		//	currentScale = Vector3.Lerp(previousScale, wantedScale, scaleTimer);
			
		//	//currentScale = Vector3.Lerp(currentScale, wantedScale, Time.deltaTime * scaleSpeed);
		//	transform.localScale = currentScale;
		//	if (currentScale == wantedScale)
		//	{
		//		scaleTimer = 0;
		//		shouldScale = false;
		//	}
		//}
		//else wantedScale = new Vector3();
	}
	public void StartScaling()
	{
		if (currentScale.magnitude >= maxScale.magnitude)
		{
			currentScale = maxScale;
			enabled = false;
			return;
		}
		previousScale = currentScale;
		float x = 0, y = 0, z =0;

		x = maxScale.x * Mathf.Clamp(timeUser.agePercentage, 0.1f,1f);
		y = maxScale.y * Mathf.Clamp(timeUser.agePercentage, 0.1f,1f);
		z = maxScale.z * Mathf.Clamp(timeUser.agePercentage, 0.1f, 1f);
		wantedScale = new Vector3(x,y,z);
		shouldScale = true;
	}
}
