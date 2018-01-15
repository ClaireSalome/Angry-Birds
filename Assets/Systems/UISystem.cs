﻿using UnityEngine;
using FYFY;
using UnityEngine.UI ;
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

	//get sliders
	Slider vx_slider = GameObject.FindGameObjectWithTag("Vx_Slider").GetComponent<Slider>() ;
	Slider vy_slider = GameObject.FindGameObjectWithTag("Vy_Slider").GetComponent<Slider>() ;

	//get buttons
	Button shoot = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Button> () ;
	Button distance = GameObject.FindGameObjectWithTag("distance").GetComponent<Button> () ;
	Button mission_but = GameObject.FindGameObjectWithTag("mission_button").GetComponent<Button>() ;
	Text res_dist = GameObject.FindGameObjectWithTag("result_dist").GetComponent<Text> () ;

	//canvas
	Canvas mission = GameObject.FindGameObjectWithTag("mission").GetComponent<Canvas>();


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
			if (mv.inMovement) {
				vx_slider.enabled = false;
				vy_slider.enabled = false;
			} else {
				vx_slider.enabled = true;
				vy_slider.enabled = true;
			}

			if (measuring) {
				measureDistance ();
			}
		}
			
		updateScore ();
	}


	//update the slider value displayed at screen 
	//update speed value
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
		direction_vector.transform.eulerAngles = new Vector3(0,0,(Mathf.Acos(vx_slider.value/Mathf.Sqrt(Mathf.Pow(vx_slider.value,2)+Mathf.Pow(vy_slider.value,2)))*Mathf.Rad2Deg));
		//puissance
		direction_vector.transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow (vx_slider.value, 2) + Mathf.Pow (vy_slider.value, 2)) / 2,direction_vector.transform.localScale.y,0f);
	}


	public void updateScore() {
		TotalScore ts = GameObject.FindGameObjectWithTag ("total").GetComponent<TotalScore> ();
		score.text = "Score :  "+ts.score_total;
	}

	public void hideMission(){
		mission.sortingOrder = -10 ;
	}
}
