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

	void Update () {}

	public void UpdateActiveUnitText(string newText) {
		activeUnitText.text = newText;
	}
}
