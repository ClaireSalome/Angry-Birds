using UnityEngine;
using FYFY;
using System.Collections;

public class TrajectoireAirSystem : FSystem {
	//système pour calculer les vecteurs vitesses sur les axes
	//en fonction de angle et vitesse souhaités


	// _projectile = famille des entités ayant le composant Move
	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));

	// coeff frottement solide
	private float mu = 0.5f;

	//pour frottement de l'air
	private float Cx = 1f;
	private float mvAir = 1.2f;


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

				//surface du projectile
				float S = Mathf.PI * Mathf.Pow(dp.rayon,2);
				//volume du projectile
				float V = (4f/3f)*Mathf.PI*Mathf.Pow(dp.rayon,3);

				//equations du cours
				float delta_x =  (mo.vitesse.x * dt) - ((Cx*S*mvAir*Mathf.Pow(mo.vitesse.x,2)*Mathf.Pow(dt,2))/(dp.masse*4f)) ;
				float delta_y = 0f;
				mo.vitesse.x -= (Cx * S * mvAir * Mathf.Pow (mo.vitesse.x, 2)*dt)/(2f*dp.masse);
				if (mo.vitesse.x <= 0f) {
					mo.vitesse.x = 0f;
				}


				//si le projectile n'a pas touché le sol
				if (mo.groundContact == false) {
					delta_y = (mo.vitesse.y * dt) + (mo.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
					delta_y -= (Cx * S * mvAir * Mathf.Pow (mo.vitesse.y, 2) * Mathf.Pow(dt,2))/(4f*dp.masse);
					delta_y += ((-mvAir*V*mo.earth_gravity.y)/2f) * Mathf.Pow(dt,2) ;

					mo.vitesse.y +=  mo.earth_gravity.y * dt;
					mo.vitesse.y -= (Cx * S * mvAir * Mathf.Pow (mo.vitesse.y, 2) *dt )/(2f*dp.masse);//*dt ?
					mo.vitesse.y += -mvAir*V*mo.earth_gravity.y*dt ;
					go.transform.eulerAngles = new Vector3 (0, 0, mo.vitesse.y*Mathf.Rad2Deg );
				} 
				else {
					//le projectile a touché le sol -> force de frottement
					delta_x += mu * (mo.earth_gravity.y / 2f) * Mathf.Pow (dt, 2);
					mo.vitesse.x += mu * mo.earth_gravity.y * dt;
					go.transform.eulerAngles = new Vector3 (0, 0, mo.vitesse.x*Mathf.Rad2Deg );
					// si la vitesse est nulle, le projectile ne bouge plus
					if (mo.vitesse.x <= 0f && mo.vitesse.y <= 0f) {
						mo.inMovement = false;
						//faire une pause pour l'affichage
						go.AddComponent<WaitAir>();
					}
				}

				go.transform.position += new Vector3 (delta_x, delta_y, 0f);

			}

		}

	}

}


// MonoBehaviour pour l'attente entre l'arret du projectile et re initialisation de la position
public class WaitAir : MonoBehaviour
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