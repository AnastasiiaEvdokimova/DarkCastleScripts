using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDash_Behaviour : MonoBehaviour
{
    float damage;
    Player_Movement playerMovement;
    public GameObject StaticEffect;
    Player_Status playerStatus;
    GameObject boss;
    private void Start()
    {
        damage = 2;
        GameObject player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<Player_Movement>();
        playerStatus = player.GetComponent<Player_Status>();
        boss = GameObject.FindWithTag("Boss");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "WaterShield(Clone)")
        {
            Destroy(gameObject);
            boss.GetComponent<Rio_Movement>().DashDestroyed();
        }
          if (collision.tag == "Player")
        {
            playerStatus.LoseHP(damage);
            Instantiate(StaticEffect).GetComponent<ElectrifiedAnimation_Behaviour>().Bind(GameObject.FindWithTag("Player"));
            playerMovement.AffectSpeed(0, 1f);
            Destroy(gameObject);
            boss.GetComponent<Rio_Status>().TriggerInvincibility();
            boss.GetComponent<Rio_Movement>().DashDestroyed();
        }
        if (collision.GetComponent<Projectile_Behaviour>() != null && collision.name!= "LightingBall(Clone)" && collision.name != "LightingBolt(Clone)")
        {
            collision.GetComponent<Projectile_Behaviour>().Destroy();
        }
    }
}
