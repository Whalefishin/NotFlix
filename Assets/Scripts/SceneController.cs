using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void LoadMenu() {
		SceneManager.LoadScene ("StartScene");
	}

	public void LoadMain() {
		SceneManager.LoadScene ("Main");
	}

	public void LoadEnd(float satisfaction) {
		SceneManager.LoadScene ("EndScene");
	}

}
