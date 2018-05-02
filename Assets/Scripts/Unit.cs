// Aiden
using UnityEngine;

/// <summary>
/// Base Class for any Unit (ally) in the Game
/// </summary>
public class Unit : MonoBehaviour{

	// 
	public HexCoordinates coordinates;							// Unit's location on the hexgrid
	public HexCoordinates initialCoord;							// to be used for pathfinding

	// Status Attributes
	[SerializeField]
    public string name;

    public int currentHP;
    public int maxHP;

    public int currentAP;
    public int maxAP;

    // Cost to move per normal grid hex
    public int moveCost;

    public string weaponType;
    public string weaponAff;

    // Base stats
    public int attack;
    public int defense;
    public int dexterity;
    public int endurance;
    public int intelligence;

	// List of available moves
	public Ability[] movelist;

	// Properties
	#region
	public string Name {
		get { return name; }
	}

	#endregion

}
