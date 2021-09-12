using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArrow_Behaviour : Projectile_Behaviour
{
    private byte shootingType, MPdamage = 10;
    private Miuna_Abilities boss;

    private void Start()
    {
        basicDamage = 0.1f;
        boss = GameObject.Find("Boss_Miuna").GetComponent<Miuna_Abilities>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player_Status playerStatus = collision.gameObject.GetComponent<Player_Status>();
            playerStatus.LoseHP(basicDamage);
            playerStatus.StopMPRecover(1.5f);
            playerStatus.ExpendMP(MPdamage);
            boss.TargetShot(shootingType);
            if (collision.name == "Player_Orion")
            {
                collision.gameObject.GetComponent<Player_Movement>().AffectSpeed(0.5f, 1.5f);
            }
            Destroy(gameObject);
        }
        if (collision.name == "WaterWave(clone)" || collision.name == "Wind_Slash(Clone)")
        {
            Destroy(gameObject);
        }
    }

    public void SetType (byte shootingIndex)
    {
        this.shootingType = shootingIndex;
    }
    
        void Update()
     {
        if (Time.timeScale != 0)
            MoveForward();
    }
}
