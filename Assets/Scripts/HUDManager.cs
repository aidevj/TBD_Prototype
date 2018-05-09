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
	float APBarMaxHeight;


	void Start () {
		UnitControllerScript = GameObject.Find ("ControllerManager").GetComponent<UnitController> ();

		// assign all text
		activeUnitText = activeUnitObj.GetComponent<Text> ();
		HPText = HPTextObj.GetComponent<Text> ();
		APText = APTextObj.GetComponent<Text> ();

		// Get transforms of bars
		APBarTransform = APBar.GetComponent<RectTransform> ();
		APBarMaxHeight = APBarTransform.sizeDelta.y;
	}

	void Update () {
        UpdateAPBar();
	}

	public void UpdateAPBar() {
		// Change height of bars accordingly
		APBarTransform.sizeDelta = new Vector2(
			APBarTransform.sizeDelta.x, // NEVER CHANGE
			APBarMaxHeight * ((float)UnitControllerScript.controlledUnit.currentAP / (float)UnitControllerScript.controlledUnit.maxAP)
		);

		APText.text = "AP " + UnitControllerScript.controlledUnit.currentAP + "/" + UnitControllerScript.controlledUnit.maxAP;
	}


	public void UpdateActiveUnitText(string newText) {
		activeUnitText.text = newText;
	}
}
