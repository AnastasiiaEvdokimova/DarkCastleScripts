using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rio_Status : Boss_Status
{
    private float invincibilityTime, touchTime;
    void Start()
    {
        HP = 6;
        invincibilityTime = 2f;
        touchTime = Time.time;
    }

    public void TriggerInvincibility()
    {
        touchTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
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
                gameObject.GetComponent<Rio_Movement>().Stop();
                gameObject.GetComponent<Rio_Movement>().StartMoving(false);
        }
    }
}
