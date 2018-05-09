using UnityEngine;

public class HexCell : MonoBehaviour {

	// Physical Attributes
	public HexCoordinates coordinates;

	public Color color;

	// Status Attributes
	public TerrainType terrainType;
	public Unit occupant = null;				// unit occupying this hex

	public bool IsOccupied() {
		if (occupant != null)
			return true;
		return false;
	}
}