using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LC_TimeController : MonoBehaviour {
	public static LC_TimeController instance;
	public bool DebugSpeedUpTime;
	public float timerInterval;
	public List<LC_TimeUser> timerUsers = new List<LC_TimeUser>();
	private bool runTimer=true;
	public delegate void TimerDelegate();
	public event TimerDelegate OnTimerInterval;

	void Awake ()
	{
		foreach (var timeUser in FindObjectsOfType<LC_TimeUser>())
		{
			if(!timerUsers.Contains(timeUser))
				timerUsers.Add(timeUser);
		}
		instance = this;
		
		StartCoroutine(Start());
	}
	void Update()
	{
		if(DebugSpeedUpTime)
		{
			if (Input.GetKeyDown(KeyCode.T))
				Time.timeScale = 20;
			if (Input.GetKeyUp(KeyCode.T))
				Time.timeScale = 1;
		}
	}
	IEnumerator Start()
	{
		while (runTimer)
		{
			yield return new WaitForSeconds(timerInterval);
			if (OnTimerInterval != null)
				OnTimerInterval.Invoke();
			//print(Time.time);
		}
	}

	void OnDisable()
	{
		runTimer = false;
	}
}
