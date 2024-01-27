using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int feeling;
    public int maxHP;
    public int currentHP;
    public int maxMood;
    public int currentMood;

    public float dodgePossibility;
    public float listenPossibility;
    public float ticklePossibility;
    public float smilePossibility;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if(currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool TakeMood(int feeling)
    {
        currentMood += feeling;
        if(currentMood >= 100) {
            return true;
        }
        else
        {
            return false;
        }
    }
}
