using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
//using UnityEngine.Random;

public class GameManager : MonoBehaviour {

	//public SceneManager sm;
	public GameObject WatchAnim;
	public Animation WatchAnimAnim;

	AudioSource myAudioSource;



	public List<GameObject> movies;
	public float timer;
	public static GameManager instance;

	//the current movie selected
	public GameObject movie_selected;

	//Preview Screen
	public GameObject preview_image;
	public GameObject preview_title;
	public GameObject preview_description;
	public Sprite original_image;

	//Prompt Button
	public GameObject prompt_button;

	//bars
	public GameObject bar_parent;
	public GameObject satisfaction_bar;
	public GameObject time_bar;



	/// <summary>
	/// How long should each movie last?
	/// </summary>
	[SerializeField]
	float timePerMovie = 2;
	/// <summary>
	/// How long is the day?
	/// </summary>
	[SerializeField]
	float maxTime;
	GameObject targetMovie;

	public float satisfaction;

	/// <summary>
	/// Can the player only receive a certain max amount of satisfaction?
	/// </summary>
	[SerializeField]
	float maxSatisfaction;

	/// <summary>
	/// How much satisfaction does the player get for a correct movie?
	/// </summary>
	[SerializeField]
	float correctMovieSatisfaction;

	/// <summary>
	/// How much satisfaction does the player get for an incorrect movie?
	/// </summary>
	[SerializeField]
	float wrongMovieSatisfaction;

	List<int> usedMovies;

	// Use this for initialization
	void Start () {
		WatchAnim.SetActive(false);
		WatchAnimAnim = WatchAnim.GetComponent<Animation> ();
		myAudioSource = GetComponent<AudioSource> ();
		original_image = preview_image.GetComponent<Image> ().sprite;
		// Replace with appropriate callback from the input manager or whatever.
		//		InputManager.OnMouseClick += HandleMovieConfirmed;
		Initialize();
		StartCoroutine (SelectMovie ());
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if (timer >= maxTime) {
			EndTheDay ();
		}

	}

	void Initialize() {
		//Have only one GameManager across different scenes
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy (gameObject);
			return;
		}

		//movies = new List<GameObject> ();
		ChooseMovie();
		timer = 0;
		satisfaction = 0;
	}

	// Get a random movie.
	void ChooseMovie() {
		bool movieChosen = false;
		int id = -1;
		while (movieChosen == false) {
			id = Random.Range (0, movies.Count - 1);
			if (usedMovies == null) {
				usedMovies = new List<int> ();
			}
			movieChosen = usedMovies.Contains (id) == false;
		}
		//print ("movie chosen:");
		//print (id);
		id = 0;
		targetMovie = movies [id];
		DialogueTrigger prompt = prompt_button.GetComponent<DialogueTrigger> ();
		prompt.dialogue.sentences = targetMovie.GetComponent<MovieScript> ().prompt;

		if (targetMovie.GetComponent<MovieScript>().Id == -1) {
			Debug.Log ("ERROR: Choosing movie failed. Is something wrong with the chosen movie?");
			Debug.Break ();
		}
		if (usedMovies.Contains(id)) {
//			Debug.Log("ERROR: This movie has already been chosen. Did you forget to add it to the usedMovies list?");
//			Debug.Break ();
		}

		usedMovies.Add (id);
	}

	// Checks if the id is the target movie, and responds.
	void CheckSelection(int id) {
		if (id == targetMovie.GetComponent<MovieScript>().Id) {
			satisfaction += correctMovieSatisfaction;
		} else {
			satisfaction += wrongMovieSatisfaction;
		}
	}

	// Tells the scene manager to go to the watch scene with the current movie.
	void HandleWatchMovie(int id) {
/*		GameObject m = movies [id];
		timer += timePerMovie;*/
		Debug.Log ("Watching movie");
		StartCoroutine(WatchTheMovie(id));
//		yield return WatchTheMovie (id);
		// Change the scene
//		 eg. SceneManager.WatchMovie();
//		SceneManager.

		// Select a new movie
		ChooseMovie();

		// Change the scene
		// eg. SceneManager.MovieSelectionScene();
	}
		
	public IEnumerator WatchTheMovie(int id) {
		Debug.Log ("Watching The Movie ienum");
		GameObject m = movies[id];
		timer += timePerMovie;
		WatchAnim.SetActive(true);
		WatchAnimAnim.Play ();
		myAudioSource.clip = movies [id].GetComponent<MovieScript>().myClip;
//		myAudioSource.clip
//		Debug.Log ();
//		Debug.Log(
		myAudioSource.Play ();
//		WatchAnim.GetComponent
		//play sound
		yield return new WaitForSeconds(myAudioSource.clip.length);
		//stop sound
		myAudioSource.Stop();
		WatchAnimAnim.Stop();
		WatchAnim.SetActive(false);
	}

	// When the player chooses a movie to watch, this gets called.
	public void HandleMovieConfirmed(int selectedMovie) {
		// Tell the UI Manager?
		CheckSelection (selectedMovie);
		RestorePreviewScreen ();
		HandleWatchMovie (selectedMovie);
	}

	void EndTheDay() {
		// Send stats to 
		// Tell scene manager to go to end scene.
		// eg. SceneManager.EndScene(usedMovies, satisfactionMeter) {}
	}

	public void WatchMovieSelected(){
		if (movie_selected != null) {
			int movie_id = movie_selected.GetComponent<MovieScript> ().id;
			HandleMovieConfirmed (movie_id);
		} else {
			Debug.Log ("Select a movie first!");
		}
	}


	public IEnumerator SelectMovie(){
		while (true) {
			if (Input.GetMouseButtonDown (0)) {
				Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				RaycastHit2D hit=Physics2D.Raycast(rayPos, Vector2.zero, 0f);
				if (hit) {
					//print (hitInfo.transform.gameObject);
					GameObject selected = hit.transform.gameObject;
					if (selected.GetComponent<MovieScript> () != null) { 
						//print ("selected movie: ");
						//print (selected.GetComponent<MovieScript> ().title);
						movie_selected = selected;
						ChangePreviewScreen ();
						yield return null;
					} else {
						print ("Game object is not a movie");
						yield return null;
					}
				} else {
					yield return null;
				}
			} else {
				yield return null;
			}
		}
	}

	void ChangePreviewScreen(){
		MovieScript script = movie_selected.GetComponent<MovieScript> ();

		//image
		Sprite background = movie_selected.GetComponent<SpriteRenderer> ().sprite;
		preview_image.GetComponent<Image> ().sprite = background;

		//title
		string title = script.title;
		preview_title.GetComponent<Text> ().text = title;

		//description
		string description = script.desc;
		preview_description.GetComponent<Text> ().text = description;
	}

	void RestorePreviewScreen(){
		preview_image.GetComponent<Image> ().sprite = original_image;
		preview_title.GetComponent<Text> ().text = "";
		preview_description.GetComponent<Text> ().text = "Choose something to watch!";
	}

}
