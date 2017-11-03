using UnityEngine;
using FYFY;
using UnityEngine.UI ;

public class UISystem : FSystem {

	//pour ne faire qu'une fois les ajouts d'evenements
	private bool addEvent = true; 

	//modifications sur les projectiles
	private Family _projectile = FamilyManager.getFamily (new AllOfComponents(typeof(Move)));

	//get texts
	Text vx_text = GameObject.FindGameObjectWithTag("Vx_Text").GetComponent<Text> ();
	Text vy_text = GameObject.FindGameObjectWithTag("Vy_Text").GetComponent<Text> ();

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
			shoot.onClick.AddListener (triggerShoot);
			vx_slider.onValueChanged.AddListener (delegate {
				updateVxValue();
			});
			vy_slider.onValueChanged.AddListener (delegate {
				updateVyValue();
			});
			addEvent = false;
		}
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
		updateArrow ();
	}

	public void updateVyValue() {
		vy_text.text = "Vy = "+vy_slider.value.ToString("F1");
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

	public void updateArrow() {
		//angle
		direction_vector.transform.eulerAngles = new Vector3(0,0,(Mathf.Acos(vx_slider.value/Mathf.Sqrt(Mathf.Pow(vx_slider.value,2)+Mathf.Pow(vy_slider.value,2)))*Mathf.Rad2Deg));
		//puissance
		direction_vector.transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow (vx_slider.value, 2) + Mathf.Pow (vy_slider.value, 2)) / 2,direction_vector.transform.localScale.y,0f);
	}

	//			Vector2 movement = new Vector2 (Mathf.Cos (mo.angle*Mathf.PI/180), Mathf.Sin (mo.angle*Mathf.PI/180));
	//			dp.set_calculated_velocity(movement * mo.vitesse);

	// angle = cos-1(x)
}
