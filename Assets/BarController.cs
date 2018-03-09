using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {
	
	[SerializeField]
	Text satisfaction;
	[SerializeField]
	Text time;

	string satText;
	string timeText;

	public float timeStart = 8;

	void Start () {

		satText = "Satisfaction: ";
		timeText = "Time: ";
		
	}
	
	void Update () {
		satText = "Satisfaction: " + GameManager.instance.satisfaction;
		timeText = "Time: " + GameManager.instance.timer.ToString("F2") + timeStart + ":00";
		satisfaction.text = (satText);
		time.text = (timeText);
	}
}
