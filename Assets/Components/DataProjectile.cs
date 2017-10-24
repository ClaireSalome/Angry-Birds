using UnityEngine;

public class DataProjectile : MonoBehaviour {

	// composant pour répertorier les données physiques du projectile

	//masse du projectile
	public float masse = 25.0f ;

	//volume du projectile ?

	// vitesse du projectile calculée par TrajectoireSystem en fonction de angle et vitesse demandés
	private Vector2 calculated_velocity= Vector2.zero ;

	public Vector2 get_calculated_velocity() {
		return calculated_velocity;
	}

	public void set_calculated_velocity(Vector2 new_velocity) {
		calculated_velocity = new_velocity;
	}
}