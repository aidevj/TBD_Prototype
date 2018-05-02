// Aiden
using UnityEngine;

/// <summary>
/// Base Class for any Unit (ally) in the Game
/// </summary>
public class Unit : MonoBehaviour{

	// Attributes
	[SerializeField]
	private string name;

	private int currentHP;
	private int maxHP;

	private int currentAP;
	private int maxAP;

	// Cost to move per normal grid hex
	private int moveCost;

	private string weaponType;
	private string weaponAff;

	// Base stats
	private int attack;
	private int defense;
	private int dexterity;
	private int endurance;
	private int intelligence;

	// List of available moves
	public Ability[] movelist;

	// Properties
	#region
	public string Name {
		get { return name; }
	}

	#endregion

}
