using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShield_Behaviour : MonoBehaviour
{
    GameObject player;
    
    public void Cast(GameObject player)
    {
        gameObject.transform.position = player.transform.position;
        this.player = player;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile_Behaviour>() != null)
        {
            collision.GetComponent<Projectile_Behaviour>().Destroy();
        }
        if (collision.tag == "Wall" && collision.GetComponent<Wall_Behaviour>().IsMoved())
        {
            collision.GetComponent<Wall_Behaviour>().Vanish();
        }
    }
    private void Update()
    {
        gameObject.transform.position = player.transform.position;
    }
}
