using UnityEngine;
using FYFY;
using System.Collections;

public class GeyserSystem : FSystem {

	private Family _particles = FamilyManager.getFamily(new AllOfComponents(typeof(ParticleSystem)));
	private Family _projectiles = FamilyManager.getFamily(new AllOfComponents(typeof(Move)));


	protected override void onProcess(int familiesUpdateCount) {

		GameObject projectile = _projectiles.First ();
		foreach (GameObject particle in _particles) {
		
			ParticleSystem ps = particle.GetComponent<ParticleSystem> ();
			Explosion exp = particle.GetComponent<Explosion> ();

			if (exp != null) {
				exp.reloadProgress += Time.deltaTime;
				// activer le geyser
				if (exp.reloadProgress >= exp.reloadTime) {
					ps.Play ();
					exp.explosionProgress += Time.deltaTime;

					if (projectile != null && projectile.transform.position.x > particle.transform.position.x - ps.shape.radius &&
						projectile.transform.position.x < particle.transform.position.x + ps.shape.radius ) {
						Rigidbody2D rbody = projectile.GetComponent<Rigidbody2D> ();
						rbody.isKinematic = false;
						rbody.AddForce (new Vector2 (0, 120), ForceMode2D.Impulse);
					}

					// desactiver le geyser
					if (exp.explosionProgress >= exp.explosionTime) {
						exp.reloadProgress = 0f;
						exp.explosionProgress = 0f;
						ps.Stop ();
						Debug.ClearDeveloperConsole ();
					}
				}
			}

		}
	
	}


//	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move))) ;
//
//	private Family _particles = FamilyManager.getFamily(new AllOfComponents(typeof(ParticleSystem)));
//
//	// Use to process your families.
//	protected override void onProcess(int familiesUpdateCount) {
//
//		// gestion des collisions avec le geyser
//		foreach (GameObject proj in _projectile) {
//
//			CircleCollider2D cc = proj.GetComponent<CircleCollider2D> ();
//
//			foreach (GameObject part in _particles){
//				
//				Collider2D collider = part.GetComponent<Collider2D> ();
//
//				if (cc.IsTouching(collider)){
//					Rigidbody2D rbody = proj.GetComponent<Rigidbody2D> ();
//					rbody.AddForce (new Vector2 (0, 60));
//					Debug.Log ("geyser");
//				}
//			}
//
//		}
//
//
//	}

}
	