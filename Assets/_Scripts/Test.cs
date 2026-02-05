using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Test : MonoBehaviour
{
    public float h;
    public float s;
    public float d;
    public float[] statLevel;

    public float playerLevel;
    public float xp;
    private float levelCost;
    public float totalSpent;

    void Start()
    {
        statLevel = new float[] { 1,1,1};
        levelCost = 10;
        playerLevel = 1;
        totalSpent = 0;
    }

   void Update()
    {

        h = 180 + (20 * statLevel[0]);
        s = 135 + (15 * statLevel[1]);
        d = 3 + (2 * statLevel[2]);
    }

    public void LevelUp(int stat)
    {
        if (xp >= levelCost)
        {
            playerLevel++;
            statLevel[stat]++;
            /*switch (stat)
                {
                case 0:
                    h += 21 - statLevel[stat];
                    break;
                case 1:
                    s += 16 - statLevel[stat];
                    break;
                case 2:
                    d += 6 - statLevel[stat]/2;
                    break;
            }*/
          //  Debug.Log(10 + (Mathf.Pow(1.1f, playerData.level)));
           // playerData.maxHealth += playerData.maxHealth * 0.2f;
           // playerData.damage += playerData.damage * 0.5f;
            xp -= levelCost;
            totalSpent += levelCost;
            levelCost = 10 + (playerLevel * playerLevel);
        }
    }
}
