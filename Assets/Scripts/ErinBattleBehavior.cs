using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErinBattleBehavior : MonoBehaviour {

    public Unit P1;
    public Unit P2;
    public Unit P3;
    public Unit P4;

    public Enemy E1;
    public Enemy E2;
    public Enemy E3;
    public Enemy E4;

    public Text ReeseHP;
    public Text ReeseAP;
    public Text AndrewHP;
    public Text AndrewAP;
    public Text AliceaHP;
    public Text AliceaAP;
    public Text StellaHP;
    public Text StellaAP;

    public Text Enemy1HP;
    public Text Enemy1AP;
    public Text Enemy2HP;
    public Text Enemy2AP;
    public Text Enemy3HP;
    public Text Enemy3AP;
    public Text Enemy4HP;
    public Text Enemy4AP;

    public Unit ActiveUnit;
    public Text ActiveUnitText;

    private bool isPlayerTurn;
    public Unit[] players;
    public Unit[] enemies;
    private int unitIndex;

    public Image PlayerBackground;
    public Image EnemyBackground;

    public Text PMove1;
    public Text PMove2;
    public Text PMove3;
    public Text PMove4;

    public Text EMove1;
    public Text EMove2;
    public Text EMove3;
    public Text EMove4;

    // Use this for initialization
    void Start () {
        isPlayerTurn = true;
        PlayerBackground.enabled = true;
        EnemyBackground.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        ReeseHP.text = "Reese HP: " + P1.currentHP;
        ReeseAP.text = "Reese AP: " + P1.currentAP;

        AndrewHP.text = "Andrew HP: " + P2.currentHP;
        AndrewAP.text = "Andrew AP: " + P2.currentAP;

        AliceaHP.text = "Alicea HP: " + P3.currentHP;
        AliceaAP.text = "Alicea AP: " + P3.currentAP;

        StellaHP.text = "Stella HP: " + P4.currentHP;
        StellaAP.text = "Stella AP: " + P4.currentAP;

        Enemy1HP.text = "Engineer HP: " + E1.currentHP;
        Enemy1AP.text = "Engineer AP: " + E1.currentAP;

        Enemy2HP.text = "Wolf HP: " + E2.currentHP;
        Enemy2AP.text = "Wolf AP: " + E2.currentAP;

        Enemy3HP.text = "FM 1 HP: " + E3.currentHP;
        Enemy3AP.text = "FM 1 AP: " + E3.currentAP;

        Enemy4HP.text = "FM 2 HP: " + E4.currentHP;
        Enemy4AP.text = "FM 2 AP: " + E4.currentAP;

        ActiveUnitText.text = "Active: " + ActiveUnit.name;

        if(isPlayerTurn)
        {
            PMove1.text = ActiveUnit.movelist[0].name + ": ";
            PMove2.text = ActiveUnit.movelist[1].name + ": ";
            PMove3.text = ActiveUnit.movelist[2].name + ": ";
            PMove4.text = ActiveUnit.movelist[3].name + ": ";
        }
        if(!isPlayerTurn)
        {
            EMove1.text = ActiveUnit.movelist[0].name + ": ";
            EMove2.text = ActiveUnit.movelist[1].name + ": ";
            EMove3.text = ActiveUnit.movelist[2].name + ": ";
            EMove4.text = ActiveUnit.movelist[3].name + ": ";
        }
        
    }

    public void SwitchUnit()
    {
        if(isPlayerTurn)
        {
            unitIndex += 1;
            if(unitIndex >= 4)
            {
                unitIndex = 0;
            }
            ActiveUnit = players[unitIndex];
        }
        if (!isPlayerTurn)
        {
            unitIndex += 1;
            if (unitIndex >= 4)
            {
                unitIndex = 0;
            }
            ActiveUnit = enemies[unitIndex];
        }
    }

    public void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        unitIndex = 0;

        if(isPlayerTurn)
        {
            PlayerBackground.enabled = true;
            EnemyBackground.enabled = false;
            ActiveUnit = P1;
            foreach(Unit p in players)
            {
                p.currentAP = p.maxAP;
            }
        }
        if (!isPlayerTurn)
        {
            PlayerBackground.enabled = false;
            EnemyBackground.enabled = true;
            ActiveUnit = E1;
            foreach (Unit e in enemies)
            {
                e.currentAP = e.maxAP;
            }
        }
    }

    public void UseSelectedMove(int moveNum)
    {
        if(ActiveUnit.currentAP >= ActiveUnit.movelist[moveNum].cost)
        {
            ActiveUnit.currentAP -= ActiveUnit.movelist[moveNum].cost;
        }
        
    }


}
