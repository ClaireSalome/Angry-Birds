using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class UIMenu : FSystem {

    //pour ne faire qu'une fois les ajouts d'evenements
    private bool addEvent = true;

    //get buttons
    Button start = GameObject.FindGameObjectWithTag("start").GetComponent<Button>();
    Button rules = GameObject.FindGameObjectWithTag("rules").GetComponent<Button>();
    Button about = GameObject.FindGameObjectWithTag("about").GetComponent<Button>();


    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount)
    {
        if (addEvent == true)
        {
            //ajout des evenements a ne faire qu'une fois
            start.onClick.AddListener(startGame);

            addEvent = false;
        }
        
    }

    public void startGame()
    {
        GameObjectManager.loadScene("menu_niveau");
    }
}
