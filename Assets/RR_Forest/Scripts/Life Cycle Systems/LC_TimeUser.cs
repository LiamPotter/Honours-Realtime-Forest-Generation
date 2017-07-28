using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class LC_TimeUser : MonoBehaviour {

	public float currentLifeTime; //How old the user is

	public bool randomizeLifeTime; //Should the user randomize it's lifeTimeScale and ageingSpeed?

	public float maxLifeTime=500; //The maximum age the user can get to

	[Range(0.1f,10)]
	public float lifeTimeScale; //How large the user's age should be
	[Range(0.01f, 5)]
	public float ageingSpeed; //How fast the user goes from one age to the next

	public float agePercentage; //The value of currentLifeTime divided by maxLifeTime

	protected float toLifeTimeValue; //The value the user's age should be trying to reach

	protected float previousLifeTime; //The user's previous life time, gets used whenever the lifetime should change
	[HideInInspector]
	public bool shouldBeAgeing; //Should the user be ageing at all?

	protected float lifeTimeTimer; //Used to control the lerping from 'currentLifeTime' to 'toLifeTimeValue'

	protected LC_Scaler scaler; //The potential scaler component that CAN use this TimeUser

	void Start () {
		if (GetComponent<LC_Scaler>())
			scaler = GetComponent<LC_Scaler>();
		//timeController.OnTimerInterval += OnTimeControllerInterval;
		if(randomizeLifeTime)
		{
			lifeTimeScale = (float)System.Math.Round(Random.Range(0.25f, 2.5f),2);
			ageingSpeed = (float)System.Math.Round(Random.Range(0.25f, 2.5f),2);
			maxLifeTime = (float)System.Math.Round(Random.Range(700f, 3000f), 2);
		}
	}
	void OnDisable()
	{
		//timeController.OnTimerInterval -= OnTimeControllerInterval;
	}
	void Update()
	{
		OnTimeControllerInterval();
		agePercentage = currentLifeTime / maxLifeTime;
		//if (shouldBeAgeing)
		//{

		//	//lifeTimeTimer += Time.deltaTime * ageingSpeed;
		//	//currentLifeTime = Mathf.Lerp(previousLifeTime, toLifeTimeValue, lifeTimeTimer);
		//	//if (currentLifeTime == toLifeTimeValue)
		//	//{
		//		//if (scaler)
		//		//	scaler.StartScaling();
	
		//		//lifeTimeTimer = 0;
		//		//shouldBeAgeing = false;
		//	//}
		//}
	}
	private void OnTimeControllerInterval()
	{
		if (currentLifeTime >= maxLifeTime)
		{
			currentLifeTime = maxLifeTime;
			shouldBeAgeing = false;
			enabled = false;
			return;
		}
		currentLifeTime += Time.deltaTime * ageingSpeed;
		//previousLifeTime = currentLifeTime;
		shouldBeAgeing = true;

	}
}
