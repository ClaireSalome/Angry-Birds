using UnityEngine;
using FYFY;
using System.Collections;

// system pour les niveaux où il faut détruire les structures

public class NextLevelSystem : FSystem {


	//pour compter le nombre de structures détruites
	private Family structures = FamilyManager.getFamily(new AllOfComponents(typeof(Destroyed))) ;

	private Family dest = FamilyManager.getFamily (new AllOfComponents(typeof(DestroyedStruct)));

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		GameObject nextLvl = dest.First ();
		TotalScore ts = FamilyManager.getFamily(new AllOfComponents(typeof (TotalScore))).First().GetComponent<TotalScore> ();

		foreach(GameObject go in structures) {

			Destroyed d = go.GetComponent<Destroyed> ();
			// si la position de la structure pass ene dessous de la limite, elle est détruite
			if(d.destroyed == false && d.transform.position.y < nextLvl.GetComponent<DestroyedStruct> ().destroyed_treshold) {
				d.destroyed = true;
				//la compter comme détruite
				nextLvl.GetComponent<DestroyedStruct> ().nb_destroyed_struct++;
			}
		}

		if (nextLvl.GetComponent<DestroyedStruct> ().nb_destroyed_struct == structures.Count) {
			//NIVEAU TERMINE : affichage d'un message + mise a jour du score
			if (nextLvl.GetComponent<DestroyedStruct> ().ended_lvl == false) {
				ts.score_total += 50;
				// bonus si on a effectué le nombre de tir minimum
				// 100 - (différence entre min et nb tir)*20 ;
				int score = 100 -(Mathf.Abs(nextLvl.GetComponent<DestroyedStruct> ().nb_shoot - nextLvl.GetComponent<DestroyedStruct> ().nb_min_shoot))*20;
				score = (score < 0) ? 0 : score;
				Debug.Log (score);
				ts.score_total += score;
				nextLvl.GetComponent<DestroyedStruct> ().ended_lvl = true ;
				GameObject winText = GameObject.FindGameObjectWithTag("win") ;
				winText.GetComponent<Canvas>().sortingOrder = 2;
				nextLvl.AddComponent<ChangeLevel> ();
			}

		}
	
	}
}

// pour avoir un délai entre la réussite du niveau et le changement de scène
public class ChangeLevel : MonoBehaviour
{
	private Family dest = FamilyManager.getFamily (new AllOfComponents(typeof(DestroyedStruct)));

	public void Start(){
		StartCoroutine(waitForIt());
	}

	IEnumerator waitForIt() {
		GameObject nextLvl = dest.First ();
		yield return new WaitForSeconds(4f);
		GameObjectManager.loadScene (nextLvl.GetComponent<DestroyedStruct>().level_to_load);
	}


}