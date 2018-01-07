using UnityEngine;
using FYFY;
using System.Collections;

public class TrajectoireEauSystem : FSystem {
	//système pour calculer les vecteurs vitesses sur les axes
	//en fonction de angle et vitesse souhaités


	// _projectile = famille des entités ayant le composant Move
	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));

	// coeff frottement solide
	private float mu = 0.5f;

	//pour frottement de l'eau
	private float mvEau = 1000f;
	private float viscosite_eau = 0.0001f;


	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in _projectile) {

			Move mo = go.GetComponent<Move> ();
			DataProjectile dp = go.GetComponent<DataProjectile> ();
			Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();

			float dt = Time.deltaTime;

			if (mo.inMovement == true) {

				//si le projectile sort du champ de la caméra
				if (go.transform.position.x > 3f * Camera.main.orthographicSize) {
					mo.inMovement = false;
					mo.new_projectile = true;
				}

				//sigma pour Stokes
				float sigma = 6 * Mathf.PI * dp.rayon;

				//equations du cours
				float delta_x =  (mo.vitesse.x * dt) - sigma*viscosite_eau*mo.vitesse.x*dt;
				float delta_y = 0f;

				//si le projectile n'a pas touché le sol
				//TODO attention à l'usage de la masse : à revoir
				if (mo.groundContact == false) {
					delta_y = (mo.vitesse.y * dt) + (dp.masse * mo.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
					delta_y -= sigma * viscosite_eau * mo.vitesse.y * dt;
					mo.vitesse.y += dp.masse * mo.earth_gravity.y * dt;
					mo.vitesse.y -= sigma * viscosite_eau * mo.vitesse.y;
					//go.transform.eulerAngles = new Vector3 (0, 0, mo.vitesse.y*Mathf.Rad2Deg );
				} 
				else {
					//le projectile a touché le sol -> force de frottement
					delta_x += mu * (dp.masse * mo.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
					delta_x -= sigma * viscosite_eau * mo.vitesse.x * dt;
					mo.vitesse.x += mu * mo.earth_gravity.y * dt;
					mo.vitesse.x -= sigma * viscosite_eau * mo.vitesse.x ;
					//go.transform.eulerAngles = new Vector3 (0, 0, mo.vitesse.x*Mathf.Rad2Deg );
					}
				// si la vitesse est nulle, le projectile ne bouge plus
				if (mo.vitesse.x <= 0f && mo.vitesse.y <= 0f) {
					mo.inMovement = false;
					//faire une pause pour l'affichage
					go.AddComponent<WaitEau>();
				}

				go.transform.position += new Vector3 (delta_x, delta_y, 0f);

			}

		}

	}

}


// MonoBehaviour pour l'attente entre l'arret du projectile et re initialisation de la position
public class WaitEau : MonoBehaviour
{
	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));

	public void Start(){
		StartCoroutine(resetPro());
	}

	IEnumerator resetPro() {
		Move mo = _projectile.First ().GetComponent<Move> ();
		yield return new WaitForSeconds(1.5f);
		mo.new_projectile = true;
	}


}