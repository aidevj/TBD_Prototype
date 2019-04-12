using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBehavior : MonoBehaviour
{
    public List<Unit> playerParty;
    public List<Enemy> enemies;
    
    void Start()
    {
        // populate player party and enemies list
    }
    
    void Update()
    {
        
    }

    /// <summary>
    /// Checks the status of a given list of units, such as the player's party or the enemy party.
    /// Returns true for remaining living units, or false for wiped parties.
    /// </summary>
    public bool CheckPartyStatusAlive(List<Unit> unitGroup)
    {
        foreach (Unit u in unitGroup)
        {
            if (u.status == Status.Alive) // this means at least one party member is still alive
                return true;
        }
        // this means all have died
        // TO DO: lose game

        return false;
    }
}
