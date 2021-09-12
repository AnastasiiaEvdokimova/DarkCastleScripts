using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ringo_Status : Boss_Status
{
    private float invincibilityTime, touchTime;
    void Start()
    {
        HP = 6;
        invincibilityTime = 2f;
        touchTime = Time.time;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Player"))
        {
            if ((Time.time - touchTime > invincibilityTime))
            {
                touchTime = Time.time;
                DealDamage(1);
            }
            gameObject.GetComponent<Ringo_Movement>().StartMoving(true);
        }
    }
}