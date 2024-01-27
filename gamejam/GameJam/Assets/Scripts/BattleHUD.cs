using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{   
    public Text EnemyName;
    public Text PlayerName;
    public Slider enemyHealth;
    public Slider enemyMood;
    public Slider playerHealth;

    public void SetHUD(Unit enemyUnit, Unit playerUnit)
    {
        enemyHealth.maxValue = enemyUnit.maxHP;
        enemyMood.maxValue = enemyUnit.maxMood;
        playerHealth.maxValue = playerUnit.maxHP;
        EnemyName.text = enemyUnit.unitName;
        PlayerName.text = playerUnit.unitName;



    }
    public void HP(int enemyhp, int playerhp, int enemymood)
    {
        enemyHealth.value = enemyhp;
        enemyMood.value = enemymood;
        playerHealth.value = playerhp;
    }
}
