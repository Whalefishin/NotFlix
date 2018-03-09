using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieScript : MonoBehaviour {

	public int id;

	public int Id {get { return id; }}

	[TextArea(4,10)]
	public string desc;

	public string title;

	[TextArea(3,10)]
	public string[] prompt;

	public AudioClip myClip;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
