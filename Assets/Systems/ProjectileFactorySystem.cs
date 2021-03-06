﻿using UnityEngine;
using FYFY;

public class ProjectileFactorySystem : FSystem {

	private Family _projectile = FamilyManager.getFamily(new AllOfComponents(typeof(Move))) ;

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		GameObject proj = _projectile.First ();

		if (proj != null) {

			Move mo = proj.GetComponent<Move> ();
			Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();

			//l'ancien projectile ne bouge plus, on en crée un nouveau
			if (mo.new_projectile == true) {
				rb.isKinematic = true;
				rb.AddForce (new Vector2 (0, 0));
				//récupération des anciens paramètres
				mo.groundContact = false;
				mo.new_projectile = false;
				mo.transform.position = mo.init_position;
				mo.vitesse = mo.vitesse_init ;
				mo.idStructure.Clear ();
				mo.stone_touched = false;

				proj.transform.eulerAngles = new Vector3 (0, 0, 1);

			}

		}
	}
}