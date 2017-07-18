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

	private float toLifeTimeValue; //The value the user's age should be trying to reach

	private float previousLifeTime; //The user's previous life time, gets used whenever the lifetime should change
	[HideInInspector]
	public bool shouldBeAgeing; //Should the user be ageing at all?

	private float lifeTimeTimer; //Used to control the lerping from 'currentLifeTime' to 'toLifeTimeValue'

	private LC_TimeController timeController; //The controller that controls all ageing events

	void Start () {
		timeController = LC_TimeController.instance;
		timeController.OnTimerInterval += OnTimeControllerInterval;
		if(randomizeLifeTime)
		{
			lifeTimeScale = (float)System.Math.Round(Random.Range(0.5f, 2f),2);
			ageingSpeed = (float)System.Math.Round(Random.Range(0.75f, 1.5f),2);
		}
	}

	void Update()
	{
		if(shouldBeAgeing)
		{
			if (currentLifeTime >= maxLifeTime)
				shouldBeAgeing = false;
			lifeTimeTimer += Time.deltaTime*ageingSpeed;
			currentLifeTime = Mathf.Lerp(previousLifeTime, toLifeTimeValue, lifeTimeTimer);
			if (currentLifeTime == toLifeTimeValue)
			{
				if (GetComponent<LC_Scaler>())
					GetComponent<LC_Scaler>().StartScaling();
				agePercentage = (float)System.Math.Round(currentLifeTime / maxLifeTime, 2);
				lifeTimeTimer = 0;
				shouldBeAgeing = false;
			}
		}
	}
	private void OnTimeControllerInterval()
	{
		toLifeTimeValue += (timeController.timerInterval * lifeTimeScale);
		previousLifeTime = currentLifeTime;
		shouldBeAgeing = true;
	}
}
