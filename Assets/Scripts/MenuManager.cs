using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public List <Button> buttons;
	public Button buttonPrefab;
	private int currentButton;
	public GameObject canv;
	// Use this for initialization
	void Start () {
		currentButton = 0;

		//makes 3 buttons on it's own
		//ideally we'd pull in a Character's info to populate with their specific attacks
		for (int i = 1; i < 4; i++) {
			Button b1 = Instantiate (buttonPrefab) as Button;
			b1.transform.SetParent (canv.transform, false);
			b1.transform.position += new Vector3 (-420f, -30f * i, 0f);
			b1.GetComponentInChildren<Text> ().text = "Attack " + i;
			b1.onClick.AddListener (() => ConfirmAttack ());

			buttons.Add (b1);
		}

		SelectButton ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp (KeyCode.R)) { //select next attack
			DeselectButton ();
			currentButton++;
			if (currentButton >= buttons.Count) {
				currentButton = 0;
			}
			SelectButton ();
		}

		if (Input.GetKeyUp (KeyCode.Q)) { //select previous attack
			DeselectButton ();
			currentButton--;
			if (currentButton < 0) {
				currentButton = buttons.Count - 1;;
			}
			SelectButton ();
		}

		if (Input.GetKeyUp (KeyCode.Space)) { //confirm attack
			buttons [currentButton].onClick.Invoke ();
		}
	}

	public void SelectButton(){

		ColorBlock cb = buttons [currentButton].colors;
		cb.normalColor = new Color(0.5F, 0.6F, 0.7F,1F);
		buttons [currentButton].colors = cb;
		buttons [currentButton].transform.position += new Vector3 (15f, 0f, 0f);
	}
	public void DeselectButton(){

		ColorBlock cb = buttons [currentButton].colors;
		cb.normalColor = Color.white;
		buttons [currentButton].colors = cb;
		buttons [currentButton].transform.position -= new Vector3 (15f, 0f, 0f);
	}

	public void ConfirmAttack(){
		
		Debug.Log (buttons[currentButton].GetComponentInChildren<Text>().text + " CLICKED");
	}
}
