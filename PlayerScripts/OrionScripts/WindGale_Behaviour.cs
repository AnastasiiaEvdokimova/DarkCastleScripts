using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGale_Behaviour : MonoBehaviour
{
    public GameObject fire;
    private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Fire")
            {
                collision.GetComponent<Fire_Behaviour>().SetFlame(collision.transform.position, collision.transform.localScale.x + gameObject.transform.localScale.x / 2, 1, gameObject.transform.position.x - collision.transform.position.x, collision.transform.position.y - gameObject.transform.position.y);
                Destroy(gameObject);
            }
            if (collision.name == "WillOWisp(Clone)")
            {
                Instantiate(fire).GetComponent<Fire_Behaviour>().SetFlame(collision.transform.position, gameObject.transform.localScale.x, 0, 1, 1);
                Destroy(gameObject);
            }
    }

    public void Set(Transform player)
    {
        this.player = player;
        gameObject.transform.position = player.transform.position;
    }

    private void Update()
    {
        gameObject.transform.position = player.transform.position;
    }
}
