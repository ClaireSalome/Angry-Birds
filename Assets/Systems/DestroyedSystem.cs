using UnityEngine;
using FYFY;

public class NextLevelSystem : FSystem {

	private GameObject[] structures = GameObject.FindGameObjectsWithTag("wood_struct");


	//pour compter le nombre de structures détruites
	private Family dest = FamilyManager.getFamily (new AllOfComponents(typeof(DestroyedStruct)));

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		GameObject nextLvl = dest.First ();

		foreach(GameObject go in structures) {

			Destroyed d = go.GetComponent<Destroyed> ();
			if(d.destroyed == false && d.transform.position.y < -2.5f) {
				d.destroyed = true;
				nextLvl.GetComponent<DestroyedStruct> ().nb_destroyed_struct++;
			}
		}

		if (nextLvl.GetComponent<DestroyedStruct> ().nb_destroyed_struct == structures.Length) {
			//NIVEAU TERMINE
			Debug.Log("GAGNE");
		}
	
	}
}