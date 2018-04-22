using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.UI;

/// <summary>
/// Script for the HexGrid object. Handles creating and trangulating the meshes of the cells of the grid.
/// Also sets coordinates and writes them on grid.
/// </summary>
public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	Canvas gridCanvas;
	HexMesh hexMesh;

	HexCell[] cells;

	public Color defaultColor = Color.white;
	public Color activeColor = Color.cyan; // "touched"

	void Awake () {
		cells = new HexCell[height * width];

		gridCanvas = GetComponentInChildren<Canvas> ();
		hexMesh = GetComponentInChildren<HexMesh> ();

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start(){
		// after the grid has awoken
		hexMesh.Triangulate (cells);
	}

	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			TouchCell(hit.point);
		}
	}

	void TouchCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);

		// to know where we are touching, we have to convert the touch position to hex coordinates
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		Debug.Log("touched at " + coordinates.ToString());

		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = activeColor;
		hexMesh.Triangulate(cells);
	}

	/// <summary>
	/// Creates cells at the given positions.
	/// </summary>
	/// <param name="x">The x position.</param>
	/// <param name="z">The z position.</param>
	/// <param name="i">Index of the cell out of all.</param>
	void CreateCell (int x, int z, int i) {
		Vector3 position;
		// space them out according to radius and offset
		position.x = ( x + z * 0.5f  - z / 2 ) * (HexMetrics.innerRadius * 2f); //position.x = x * 10f; // <-- non shifted
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f); //position.z = z * 10f;

		// assign cell to the current index and assign its values
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		// instantiate the labels and show cell coordinates
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}

}

// reference: http://catlikecoding.com/unity/tutorials/hex-map/part-1/