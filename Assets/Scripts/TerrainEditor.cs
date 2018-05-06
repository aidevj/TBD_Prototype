using UnityEngine;

public class TerrainEditor : MonoBehaviour {

	public Color[] colors;		// array of colors to be used
	public GameObject hexGridObj;
	HexGrid hexGrid;		// reference to our hex grid

	private Color activeColor;	// currently selected color

	void Awake () {
		SelectColor (0);
	}

	void Start() {
		hexGrid = hexGridObj.GetComponent<HexGrid> ();
	}

	void Update () {
		if (Input.GetMouseButton (0)) {
			HandleInput ();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			hexGrid.ColorCell (hit.point, activeColor);
			// TO DO: HANDLE CHANGING THE TERRAIN VALUE OF THE CLICKED CELL AS WELL
		}
	}

	public void SelectColor(int index) {
		activeColor = colors [index];
	}
}
