using UnityEngine;
using FYFY;
using UnityEngine.UI ;
using System.Collections.Generic;
using System.Linq;

public class GivenSpeedUISystem : FSystem {

	//pour ne faire qu'une fois les ajouts d'evenements
	private bool addEvent = true; 

	//modifications sur les projectiles
	private Family _projectile = FamilyManager.getFamily (new AllOfComponents(typeof(Move)));

	private Vector3 vitesse;

	//get score
	Text score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();

	//get buttons
	Button shoot = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Button> () ;
	Button distance = GameObject.FindGameObjectWithTag("distance").GetComponent<Button> () ;
	Button placer = GameObject.FindGameObjectWithTag("placer").GetComponent<Button> () ;
	Button mission_but = GameObject.FindGameObjectWithTag("mission_button").GetComponent<Button>() ;
	Text res_dist = GameObject.FindGameObjectWithTag("result_dist").GetComponent<Text> () ;

	//canvas
	Canvas mission = GameObject.FindGameObjectWithTag("mission").GetComponent<Canvas>();
	bool placingReward = false;
	GameObject reward = new GameObject();

	//two points for measuring a distance
	public List<GameObject> points = new List<GameObject>();
	bool measuring = false;

	// arrow sprite
	GameObject direction_vector = GameObject.FindGameObjectWithTag("arrow");


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		if (addEvent == true) {
			//ajout des evenements a ne faire qu'une fois
			shoot.onClick.AddListener (triggerShoot);
			distance.onClick.AddListener (measureDistance);
			mission_but.onClick.AddListener (hideMission);
			placer.onClick.AddListener (placeReward);
			//updateArrow ();

			vitesse = _projectile.First ().GetComponent<Move> ().vitesse;

			addEvent = false;
		}
		foreach (GameObject go in _projectile) {
			if (measuring) {
				measureDistance ();
			}

			if (placingReward) {
				placeReward ();
			}
		}

		updateScore ();
	}
		
	public void placeReward(){

		if (placingReward == false && reward.tag.Equals("reward")) {

			GameObjectManager.unbind (reward);
			GameObject.Destroy (reward);
		}
		placingReward = true;

		if (Input.GetMouseButtonDown (0)) { 
			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);

			GameObject go = GameObject.FindGameObjectWithTag ("reward");
			reward = Object.Instantiate<GameObject> (go);
			GameObjectManager.bind (reward);
			reward.transform.position = pos;

			placingReward = false;
			shoot.interactable = true;
		}
	}

	// lance le projectile
	public void triggerShoot() {
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();
			mv.inMovement = true;
		}

	}

	public void measureDistance(){

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
			Vector3 pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);

			GameObject go = GameObject.FindGameObjectWithTag ("cross");
			GameObject newc = Object.Instantiate<GameObject> (go);
			GameObjectManager.bind (newc);
			points.Add (newc);
			newc.transform.position = pos;
		}

		if (points.Count == 2) {
			float dist = Vector3.Distance (points [0].transform.position, points [1].transform.position)/2;
			res_dist.text = dist.ToString("F1")+" m";
			measuring = false;
		}
	}

	public void updateArrow() {

		//angle
		direction_vector.transform.eulerAngles = new Vector3(0,0,(Mathf.Acos(vitesse.x/Mathf.Sqrt(Mathf.Pow(vitesse.x,2)+Mathf.Pow(vitesse.y,2)))*Mathf.Rad2Deg));
		//puissance
		direction_vector.transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow (vitesse.x, 2) + Mathf.Pow (vitesse.y, 2)) / 2,direction_vector.transform.localScale.y,0f);
	}


	public void updateScore() {
		TotalScore ts = GameObject.FindGameObjectWithTag ("total").GetComponent<TotalScore> ();
		score.text = "Score :  "+ts.score_total;
	}

	public void hideMission(){
		mission.sortingOrder = -10 ;
	}
}
