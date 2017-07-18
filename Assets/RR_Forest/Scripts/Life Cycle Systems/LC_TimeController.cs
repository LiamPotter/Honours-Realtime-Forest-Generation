using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LC_TimeController : MonoBehaviour {
	public static LC_TimeController instance;
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
