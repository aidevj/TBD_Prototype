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

	void Start () {
		UnitControllerScript = GameObject.Find ("ControllerManager").GetComponent<UnitController> ();

		// assign all text
		activeUnitText = activeUnitObj.GetComponent<Text> ();
		HPText = HPTextObj.GetComponent<Text> ();
		APText = APTextObj.GetComponent<Text> ();

	}

	void Update () {
		// Attach currentUnit HP and AP to 

	}

	public void UpdateActiveUnitText(string newText) {
		activeUnitText.text = newText;
	}
}
