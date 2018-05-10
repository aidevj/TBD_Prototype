using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	// Other script references
	[HideInInspector]
	public UnitController UnitControllerScript;

	// HUD Objects
	public GameObject activeUnitObj;
	public GameObject HPTextObj;
	public GameObject APTextObj;
	public GameObject HPBar;
	public GameObject APBar;

	// Text components of corresponding HUD objs
	Text activeUnitText;
	Text HPText;
	Text APText;

	RectTransform APBarTransform;
	RectTransform HPBarTransform;
	float APBarMaxHeight;
	float HPBarMaxHeight;


	void Start () {
		UnitControllerScript = GameObject.Find ("ControllerManager").GetComponent<UnitController> ();

		// assign all text
		activeUnitText = activeUnitObj.GetComponent<Text> ();
		HPText = HPTextObj.GetComponent<Text> ();
		APText = APTextObj.GetComponent<Text> ();

		// Get transforms of bars
		APBarTransform = APBar.GetComponent<RectTransform> ();
		APBarMaxHeight = APBarTransform.sizeDelta.y;
		HPBarTransform = HPBar.GetComponent<RectTransform> ();
		HPBarMaxHeight = HPBarTransform.sizeDelta.y;
	}

	void Update () {
        UpdateAPBar();
	}

	public void UpdateAPBar() {
		APBarTransform.sizeDelta = new Vector2(
			APBarTransform.sizeDelta.x, // NEVER CHANGE
			APBarMaxHeight * ((float)UnitControllerScript.controlledUnit.currentAP / (float)UnitControllerScript.controlledUnit.maxAP)
		);

		APText.text = "AP " + UnitControllerScript.controlledUnit.currentAP + "/" + UnitControllerScript.controlledUnit.maxAP;
	}

	public void UpdateHPBar() {
		HPBarTransform.sizeDelta = new Vector2(
			HPBarTransform.sizeDelta.x, // NEVER CHANGE
			HPBarMaxHeight * ((float)UnitControllerScript.controlledUnit.currentHP / (float)UnitControllerScript.controlledUnit.maxHP)
		);

		HPText.text = "HP " + UnitControllerScript.controlledUnit.currentHP + "/" + UnitControllerScript.controlledUnit.maxHP;

	}


	public void UpdateActiveUnitText(string newText) {
		activeUnitText.text = newText;
	}
}
