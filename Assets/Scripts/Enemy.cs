using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Ability[] movelist;

    private int currentAP;
    private int maxAP;

    private int currentHP;
    private int maxHP;

	// Use this for initialization
	void Start () {
        currentAP = 15;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //beginning template for attack behavior. Does not currently distinguish between attacks and non-attack abilities
    private void Attack()
    {
        int currentMoveNum = -1;
        int highestMoveCost = 0;

        //search array of abilities, prioritize high cost moves
        for(int i = 0; i < movelist.Length; i++)
        {
            //when it has a higher move cost, set it as current move to use. But only if they can afford it with their current AP
            //does not currently account for moves of the same cost
            if(movelist[i].cost > highestMoveCost && movelist[i].cost < currentAP)
            {
                highestMoveCost = movelist[i].cost;
                currentMoveNum = i;
            }
        }

        //if no move was chosen, the enemy does not have enough AP to use an attack
        if(currentMoveNum == -1)
        {
            //end attack, no effect
        }
        //proceed with movelist(currentMoveNum)
    }
}
