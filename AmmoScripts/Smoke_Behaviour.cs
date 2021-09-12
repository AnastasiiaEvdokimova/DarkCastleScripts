using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_Behaviour : MonoBehaviour
{
    float finalScale, growthRate, dissapearanceRate, transparency, smokeDamage;
    SpriteRenderer sprite;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        sprite = gameObject.GetComponent<SpriteRenderer>();
        smokeDamage = 0.05f;
    }
    public void SetCloud(Vector2 position, float scale, float time)
    {
        gameObject.transform.position = position;
        finalScale = scale;
        growthRate = (finalScale - gameObject.transform.localScale.x) / time;
        dissapearanceRate = 50 * Time.deltaTime;
        transparency = 255;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Wind_Slash(Clone)")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (finalScale - gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale += new Vector3(growthRate * Time.deltaTime, growthRate * Time.deltaTime);
            }
            else
            {
                transparency -= dissapearanceRate;
                if (transparency <= 0) { Destroy(gameObject); }
                else { sprite.color = new Color(1, 1, 1, transparency / 255); }
            }
            if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < sprite.bounds.size.x * 0.3 && Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < sprite.bounds.size.y * 0.3)
            {
                player.GetComponent<Player_Status>().LoseHP(smokeDamage * Time.deltaTime * transparency / 255);
            }
        }
    }

}
