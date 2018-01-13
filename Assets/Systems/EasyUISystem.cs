using UnityEngine;
using FYFY;
using UnityEngine.UI ;
using System.Collections;

public class EasyUISystem : FSystem {

	//pour ne faire qu'une fois les ajouts d'evenements
	private bool addEvent = true; 

	//modifications sur les projectiles
	private Family _projectile = FamilyManager.getFamily (new AllOfComponents(typeof(Move)));
	private Family _trajectoryPoints = FamilyManager.getFamily( new AllOfComponents(typeof(PointPosition)));
		
	//get texts
	Text vx_text = GameObject.FindGameObjectWithTag("Vx_Text").GetComponent<Text> ();
	Text vy_text = GameObject.FindGameObjectWithTag("Vy_Text").GetComponent<Text> ();
	Text score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();

	//get sliders
	Slider vx_slider = GameObject.FindGameObjectWithTag("Vx_Slider").GetComponent<Slider>() ;
	Slider vy_slider = GameObject.FindGameObjectWithTag("Vy_Slider").GetComponent<Slider>() ;

	//get buttons
	Button shoot = GameObject.FindGameObjectWithTag("Shoot").GetComponent<Button> () ;

	// arrow sprite
	GameObject direction_vector = GameObject.FindGameObjectWithTag("arrow");


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		if (addEvent == true) {
			//ajout des evenements a ne faire qu'une fois
			shoot.onClick.AddListener (triggerShoot);

			vx_slider.onValueChanged.AddListener (delegate {
				updateVxValue();
			});
			vy_slider.onValueChanged.AddListener (delegate {
				updateVyValue();
			});
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
		}

		updateScore ();
	}


	//update the slider value displayed at screen 
	//update speed value
	public void updateVxValue() {
		vx_text.text = "Vx = "+vx_slider.value.ToString("F1");
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();
			mv.vitesse.x = vx_slider.value;
			mv.vitesse_init.x = vx_slider.value;
		}
		//updateArrow ();
		updateTrajectory ();
	}

	public void updateVyValue() {
		vy_text.text = "Vy = "+vy_slider.value.ToString("F1");
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();
			mv.vitesse.y = vy_slider.value;
			mv.vitesse_init.y = vy_slider.value;
		}
		//updateArrow ();	
		updateTrajectory ();
	}

	public void updateTrajectory(){
		float vx = vx_slider.value;
		float vy = vy_slider.value;
		foreach (GameObject go in _projectile) {
			Move mv = go.GetComponent<Move> ();

			//on supprime les points concstituant l'ancienne trajectoire
			foreach (GameObject pt in mv.trajectoryPoints) {
				GameObjectManager.unbind (pt);
				GameObject.Destroy (pt);
			}
			mv.trajectoryPoints.Clear ();
			DataProjectile dp = go.GetComponent<DataProjectile> ();
			GameObject point = _trajectoryPoints.First ();
			float dt = 0;
			float S = Mathf.PI * Mathf.Pow(dp.rayon,2);
			for (int i = 0; i < mv.numOfTrajectoryPoints; i++) {
				GameObject newp = Object.Instantiate<GameObject> (point);
				GameObjectManager.bind (newp);
				mv.trajectoryPoints.Add (newp);
//				float dy = vy * dt + (mv.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
//				float dx = vx * dt;
				float dx =  (vx * dt) - (0.5f*S*1.2f*Mathf.Pow(vx,2)*Mathf.Pow(dt,2)) ;
				float dy = (vy * dt) + (mv.earth_gravity.y / 2f) * Mathf.Pow (dt, 2) -  (0.5f * S * 1.2f * Mathf.Pow (vy, 2) * Mathf.Pow(dt,2));
				Vector3 pos = new Vector3 (dx, dy, 0);
				newp.transform.position += pos;
				dt += 0.1f;
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
}
