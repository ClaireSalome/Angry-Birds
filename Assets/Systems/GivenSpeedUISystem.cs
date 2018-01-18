using UnityEngine;
using FYFY;
using System;
using UnityEngine.UI ;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;


public class GivenSpeedUISystem : FSystem {

	//pour ne faire qu'une fois les ajouts d'evenements
	private bool addEvent = true; 

	//modifications sur les projectiles
	private Family _projectile = FamilyManager.getFamily (new AllOfComponents(typeof(Move)));

	//get score
	Text score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
	Text res_dist = GameObject.FindGameObjectWithTag("result_dist").GetComponent<Text> () ;

	//get buttons
	Button shoot = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Button> () ;
	Button distance = GameObject.FindGameObjectWithTag("distance").GetComponent<Button> () ;
	Button mission_but = GameObject.FindGameObjectWithTag("mission_button").GetComponent<Button>() ;
	Button accueil = GameObject.Find("accueil").GetComponent<Button>();
	Button replay = GameObject.Find("replay").GetComponent<Button>();
	Button placer;
	Button aide = GameObject.Find("help").GetComponent<Button>();
	Button close = GameObject.Find ("close").GetComponent<Button> ();

	//canvas
	Canvas mission = GameObject.FindGameObjectWithTag("mission").GetComponent<Canvas>();
	Canvas help = GameObject.Find ("Help_Canvas").GetComponent<Canvas> ();

	//pour modifier la masse
	InputField masse;
	bool massEditable = false;

	//pour placer une recompense
	GameObject reward = GameObject.FindGameObjectWithTag ("reward");
	bool placingReward = false;

	//two points for measuring a distance
	public List<GameObject> points = new List<GameObject>();
	bool measuring = false;

	// arrow sprite
	GameObject direction_vector = GameObject.FindGameObjectWithTag("arrow");


	protected override void onProcess(int familiesUpdateCount) {
		if (addEvent == true) {
			//ajout des evenements a ne faire qu'une fois
			shoot.onClick.AddListener (triggerShoot);
			distance.onClick.AddListener (measureDistance);
			mission_but.onClick.AddListener (hideMission);
			accueil.onClick.AddListener (home);
			replay.onClick.AddListener (reloadScene);
			aide.onClick.AddListener (showHelp);
			close.onClick.AddListener (hideHelp);


			if (GameObject.FindGameObjectsWithTag ("placer").Length != 0) {
				placer = GameObject.FindGameObjectWithTag("placer").GetComponent<Button>() ;
				placer.onClick.AddListener (placeReward);
			}

			if (GameObject.FindGameObjectsWithTag ("masse").Length != 0) {
				masse = GameObject.FindGameObjectWithTag("masse").GetComponent<InputField>() ;
				massEditable = true;
			}

			addEvent = false;
		}
		foreach (GameObject go in _projectile) {

//			Move mv = go.GetComponent<Move> ();
//			if (mv.inMovement) {
//				.enabled = false;
//				.enabled = false;
//			} else {
//				.enabled = true;
//				.enabled = true;
//			}

			// on attend la selection des points par l'utilisateur 
			if (measuring) {
				measureDistance ();
			}

			if (placingReward) {
				placeReward ();
			}

			if (massEditable) {
				updateMass ();
			}
		}

		updateScore ();
	}

	// affiche/deplace la recompense au point selectionne par l'utilisateur
	public void placeReward(){

		placingReward = true;

		if (Input.GetMouseButtonDown (0)) { 
			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);

			if (pos.x > -7) {
				reward.transform.position = pos;

				placingReward = false;
				shoot.interactable = true;
			}
		}
	}

	// lance le projectile
	public void triggerShoot() {
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();
			mv.inMovement = true;
		}

	}
	// recupere deux points selectionnes par l'utilisateur et affiche la distance entre les deux 
	public void measureDistance(){

		// on efface les points precedents si besoin
		if (measuring == false) {
			foreach (GameObject pt in points) {
				GameObjectManager.unbind (pt);
				GameObject.Destroy (pt);
			}
			points.Clear ();
			res_dist.text = "0 m";
			measuring = true;
		}

		if (Input.GetMouseButtonDown (0)) { 
			// on recupere la position du clic en coordonnees pixels, on les transforme en coordonnees du monde 
			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);

			// on affiche une croix au point cliqué 
			GameObject go = GameObject.FindGameObjectWithTag ("cross");
			GameObject newc = UnityEngine.Object.Instantiate<GameObject> (go);
			GameObjectManager.bind (newc);
			points.Add (newc);
			newc.transform.position = pos;
		}

		float dist;
		// si un point a ete selectionne, on affiche la distance entre ce point et la souris 
		if (points.Count == 1) {
			dist = Vector3.Distance (points [0].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition))/2; 
			res_dist.text = dist.ToString("F1")+" m"; 
		}

		// si les deux points ont ete selectiones, on affiche la distance entre les deux
		if (points.Count == 2) {
			dist = Vector3.Distance (points [0].transform.position, points [1].transform.position)/2;
			res_dist.text = dist.ToString("F1")+" m";
			measuring = false;
		}
	}

	public void updateMass(){
		if (masse.text != "") {
			DataProjectile dp = _projectile.First ().GetComponent<DataProjectile> ();
			dp.masse = Convert.ToSingle (masse.text);
			shoot.interactable = true;
		}
	}
		
	// mise a jour du score
	public void updateScore() {
		TotalScore ts = GameObject.FindGameObjectWithTag ("total").GetComponent<TotalScore> ();
		score.text = "Score :  "+ts.score_total;
	}

	// cacher le pop-up de la mission
	public void hideMission(){
		mission.sortingOrder = -10 ;
		GameObject.Find ("newton").GetComponent<SpriteRenderer>().sortingOrder = -9;

	}

	public void home(){
		GameObjectManager.loadScene ("menu_niveau");
	}

	public void reloadScene(){
		GameObjectManager.loadScene ( SceneManager.GetActiveScene ().buildIndex);
	}

	public void showHelp(){
		help.sortingOrder = 9;
		GameObject.Find ("help_background").GetComponent<SpriteRenderer> ().sortingOrder = 9;
	}

	public void hideHelp(){
		help.sortingOrder = -9;
		GameObject.Find ("help_background").GetComponent<SpriteRenderer> ().sortingOrder = -9;
	}
}
