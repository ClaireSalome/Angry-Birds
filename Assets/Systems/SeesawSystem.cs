using UnityEngine;
using FYFY;
using FYFY_plugins.CollisionManager ;
using FYFY_plugins.TriggerManager ;

public class SeesawSystem : FSystem {

	private Family _incollision = FamilyManager.getFamily(new AllOfComponents(typeof(InCollision2D)));
	private Family _projectiles = FamilyManager.getFamily(new AllOfComponents(typeof(DataProjectile)));
	private Family _rewards = FamilyManager.getFamily(new AllOfComponents(typeof(Collect)));

	//récupérer le score total pour les récompenses
	private TotalScore total = GameObject.FindGameObjectWithTag("total").GetComponent<TotalScore>() ;

	protected override void onProcess(int familiesUpdateCount) {
	
		foreach (GameObject go in _incollision) {
			
			InCollision2D col = go.GetComponent<InCollision2D> ();
			CircleCollider2D cc = go.GetComponent<CircleCollider2D> ();

			foreach (GameObject target in col.Targets) {

				if (target != null && target.tag.Equals ("seesaw")) {
					BoxCollider2D boxc = target.GetComponent<BoxCollider2D> ();

					// si le projectile principale tombe sur le bon cote de la balancoire a bascule
					if (boxc.IsTouching (cc)) {
						// le deuxieme projectile est envoye vers le haut
						foreach (GameObject proj in _projectiles) {
							if (proj.tag.Equals ("projectile")) {
								Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();
								rb.AddForce (new Vector2 (0, 60));
							}
						}
					}
				}
					
			}
		}

		foreach (GameObject proj in _projectiles) {

			CircleCollider2D cc = proj.GetComponent<CircleCollider2D> ();

			foreach (GameObject reward in _rewards) {

				PolygonCollider2D polyc = reward.GetComponent<PolygonCollider2D> ();
				if (polyc.IsTouching (cc)) {
					Collect co = reward.GetComponent<Collect> ();
					total.score_total += co.reward;
					GameObjectManager.unbind (reward);
					GameObject.Destroy (reward);
				}
			}


		}
	}
}

