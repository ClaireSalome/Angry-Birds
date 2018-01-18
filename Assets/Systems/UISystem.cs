using UnityEngine;
using FYFY;
using UnityEngine.UI ;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class UISystem : FSystem {

	//pour ne faire qu'une fois les ajouts d'evenements
	private bool addEvent = true; 

	//modifications sur les projectiles
	private Family _projectile = FamilyManager.getFamily (new AllOfComponents(typeof(Move)));

	//get texts
	Text vx_text = GameObject.FindGameObjectWithTag("Vx_Text").GetComponent<Text> ();
	Text vy_text = GameObject.FindGameObjectWithTag("Vy_Text").GetComponent<Text> ();
	Text score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
	Text res_dist = GameObject.FindGameObjectWithTag("result_dist").GetComponent<Text> () ;

	//get sliders
	Slider vx_slider = GameObject.FindGameObjectWithTag("Vx_Slider").GetComponent<Slider>() ;
	Slider vy_slider = GameObject.FindGameObjectWithTag("Vy_Slider").GetComponent<Slider>() ;

	//get buttons
	Button shoot = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Button> () ;
	Button distance = GameObject.FindGameObjectWithTag("distance").GetComponent<Button> () ;
	Button mission_but = GameObject.FindGameObjectWithTag("mission_button").GetComponent<Button>() ;
	Button accueil = GameObject.Find("accueil").GetComponent<Button>();
	Button replay = GameObject.Find("replay").GetComponent<Button>();
	Button aide = GameObject.Find("help").GetComponent<Button>();
	Button close = GameObject.Find ("close").GetComponent<Button> ();

	//canvas
	Canvas mission = GameObject.FindGameObjectWithTag("mission").GetComponent<Canvas>();
	Canvas help = GameObject.Find ("Help_Canvas").GetComponent<Canvas> ();

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

			vx_slider.onValueChanged.AddListener (delegate {
				updateVxValue();
			});
			vy_slider.onValueChanged.AddListener (delegate {
				updateVyValue();
			});

			updateArrow ();

			addEvent = false;
		}
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();

			// bloque les sliders pendant le tir
			if (mv.inMovement) {
				vx_slider.enabled = false;
				vy_slider.enabled = false;
				shoot.interactable = false; 
			} else { 
				vx_slider.enabled = true;
				vy_slider.enabled = true;
				shoot.interactable = true; 
			}

			// on attend la selection des points par l'utilisateur 
			if (measuring) {
				distance.interactable = false;
				measureDistance ();
			}
		}
			
		updateScore ();
	}


	//update the slider value displayed at screen -> update speed value
	public void updateVxValue() {
		vx_text.text = "Vx = "+vx_slider.value.ToString("F1")+" m/s";
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();
			mv.vitesse.x = vx_slider.value;
			mv.vitesse_init.x = vx_slider.value;
		}
		updateArrow ();
	}

	public void updateVyValue() {
		vy_text.text = "Vy = "+vy_slider.value.ToString("F1")+" m/s";
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();
			mv.vitesse.y = vy_slider.value;
			mv.vitesse_init.y = vy_slider.value;
		}
		updateArrow ();	
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
			GameObject newc = Object.Instantiate<GameObject> (go);
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
			distance.interactable = true;
			dist = Vector3.Distance (points [0].transform.position, points [1].transform.position)/2;
			res_dist.text = dist.ToString("F1")+" m";
			measuring = false;
		}
	}

	// mise a jour de l'affichage du vecteur vitesse
	public void updateArrow() {
		//angle
		direction_vector.transform.eulerAngles = new Vector3(0,0,(Mathf.Acos(vx_slider.value/Mathf.Sqrt(Mathf.Pow(vx_slider.value,2)+Mathf.Pow(vy_slider.value,2)))*Mathf.Rad2Deg));
		//puissance
		direction_vector.transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow (vx_slider.value, 2) + Mathf.Pow (vy_slider.value, 2)) / 2,direction_vector.transform.localScale.y,0f);
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
