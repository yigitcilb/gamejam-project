using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab0;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public BattleHUD battlehud;

    Unit playerUnit;
    Unit enemyUnit;


    public TextMeshProUGUI fightText;
    public string enemy_name;

    Dictionary<string, float[]> enemyProbabilities = new Dictionary<string, float[]>
    {
        {"minion", new float[] {0.1f, 0.5f, 0.7f, 0.8f}},
        {"little_girl", new float[] {0.8f, 0.3f, 0.7f, 0.8f}},
        {"fat_man", new float[] {0.3f, 0.6f, 0.6f, 0.2f}},
        {"final_boss", new float[] {0.9f, 0.4f, 0.2f, 0.2f}}

    };
    string[] array_important = { "little_girl", "fat_man", "final_boss" };



    void Start()
    {
        int bossTypeValue = GameManager.instance.boss_type;
        enemy_name = array_important[bossTypeValue];
        state = BattleState.START;
        
        StartCoroutine(setUpBattle());
    }
    IEnumerator setUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab0, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        

        fightText.text = enemy_name + ":";
        battlehud.SetHUD(enemyUnit, playerUnit);
        enemyUnit.maxHP = 100;
        playerUnit.maxHP = 120;
        enemyUnit.maxMood = 100;
        enemyUnit.currentHP = enemyUnit.maxHP;
        playerUnit.currentHP = playerUnit.maxHP;
        enemyUnit.currentMood = 0;
        battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();


    }
    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
        fightText.text = "Attack is succesfull.";
        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn("attack"));
        }
    }
    void PlayerTurn()
    {
        fightText.text = "Choose an action...";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }
    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            fightText.text = "You won the battle.";
        }
        else if ( state == BattleState.LOST)
        {
            fightText.text = "You were defeated.";
        }
    }
    IEnumerator EnemyTurn(string yourAction)
    {
        bool isDead = false;
        if (yourAction == "attack")
        {
            fightText.text = enemyUnit.unitName + "attacks.";
            yield return new WaitForSeconds(1f);
            isDead = playerUnit.TakeDamage(enemyUnit.damage);
        }
        else if (yourAction == "listen")
        {
            fightText.text = "Your enemy does not feel like attacking you, since you listened him/her";
            yield return new WaitForSeconds(1f);
        }
        else if (yourAction == "tickle")
        {
            fightText.text = "Your enemy does not feel like attacking you, since you tickled him/her";
            yield return new WaitForSeconds(1f);
        }
        else if (yourAction == "smile")
        {
            fightText.text = "Your enemy does not feel like attacking you, since you smiled at him/her";
            yield return new WaitForSeconds(1f);
        }
        battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void onListenButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerListen());
    }
    IEnumerator PlayerListen()
    {
        bool isDead = enemyUnit.TakeMood(playerUnit.feeling);

        battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn("listen"));
        }
        yield return new WaitForSeconds(0f);
    }
    
    public void onTickleButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerTickle());
    }
    public IEnumerator PlayerTickle()
    {
        System.Random tickleRandom = new System.Random();
        Debug.Log(enemy_name);
        Debug.Log($"{tickleRandom.NextDouble()}");
        float tickleFloat = (float)tickleRandom.NextDouble();
        bool isDead = false;
        if (tickleFloat < (float)enemyProbabilities[enemy_name][2])
        {
            isDead = enemyUnit.TakeMood(playerUnit.feeling);
            battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
            fightText.text = "You tickled the enemy successfully";
            yield return new WaitForSeconds(1f);
        }
        else
        {
            fightText.text = "Unfortunately, you failed to tickle the enemy. Try another time";
            enemyUnit.currentMood -= playerUnit.feeling / 2;
            battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
            yield return new WaitForSeconds(1f);
        }

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn("tickle"));
        }
        yield return new WaitForSeconds(0f);
    }
    public void onSmileButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerSmile());
    }
    IEnumerator PlayerSmile()
    {
        bool isDead = enemyUnit.TakeMood(playerUnit.feeling);

        battlehud.HP(enemyUnit.currentHP, playerUnit.currentHP, enemyUnit.currentMood);
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {   state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn("smile"));
        }
        yield return new WaitForSeconds(0f);
    }
}
