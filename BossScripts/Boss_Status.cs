using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Status : MonoBehaviour
{
    protected float HP;

    public void DealDamage(float damage)
    {
        if (GeneralMethods.isStoryMode)
        {
            HP -= damage;
            if (HP <= 0)
            {
                GameObject.Find("Input_Area").GetComponent<Observer>().GameWon();
            }
            else if ((int)(HP + damage) > (int)HP)
            {
                GameObject.Find("Input_Area").GetComponent<Observer>().BossHit();
            }
        }
        else
        {
            GameObject.Find("Input_Area").GetComponent<Observer>().ScoreUp(damage);
        }
    }
}
