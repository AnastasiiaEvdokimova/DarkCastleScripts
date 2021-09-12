using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miuna_Status : Boss_Status
{
    void Start()
    {
        HP = 10;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<Warp_Movement>().Warp();
           if (!collision.GetComponent<Player_Movement>().IsPushed()) DealDamage(1);
        }
    }
}
