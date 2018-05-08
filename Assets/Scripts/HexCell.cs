using UnityEngine;

public class HexCell : MonoBehaviour {

	// Physical Attributes
	public HexCoordinates coordinates;

	public Color color;

	// Status Attributes
	public TerrainType terrainType;
	public bool isOccupied;	// is there a unit already on this hex
}