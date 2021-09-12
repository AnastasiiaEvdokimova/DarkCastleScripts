using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlash_Behaviour : Projectile_Behaviour
{
    Boss_Status bossStatus;
    float dissapearanceRate;
    float originalScale;
    private void Start()
    {
        if (GameObject.FindWithTag("Boss")!=null)
        bossStatus = GameObject.FindWithTag("Boss").GetComponent<Boss_Status>();
        basicDamage = 0.5f;
        dissapearanceRate = 0.3F*Time.deltaTime;
        originalScale = gameObject.transform.localScale.x;
    }

    public float GetSize()
    {
        return gameObject.transform.localScale.x;
    }

    public Vector2 GetSpeed()
    {
        return new Vector2(xSpeed, ySpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            bossStatus.DealDamage(basicDamage * gameObject.transform.localScale.x / originalScale);
            Destroy(gameObject);
        }
        else if (collision.tag == "Wall")
        {
            if (collision.GetComponent<Wall_Behaviour>().IsMoved())
            {
                collision.GetComponent<Wall_Behaviour>().Vanish();
            }
        }
        else if (collision.name == "Light Arrow(Clone)")
        {
            Destroy(gameObject);
        }
        else if (collision.name == "LightingBall(Clone)")
        {
            collision.GetComponent<LightingBall_Behaviour>().Push(this.xSpeed, this.ySpeed);
        }
        else if (collision.name == "WillOWisp(Clone)")
        {
            gameObject.transform.localScale -= new Vector3(dissapearanceRate*2, dissapearanceRate*2);
            collision.GetComponent<WillOWisp_Behaviour>().Destroy();
        }
    }

    protected override void SpriteAdjustment(Vector2 startingPoint)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
        spriteHeight = spriteRenderer.bounds.size.y;
        gameObject.transform.position = new Vector2(startingPoint.x + spriteWidth * (xSpeed / Mathf.Abs(xSpeed))/3, startingPoint.y + spriteHeight * (ySpeed / Mathf.Abs(ySpeed)/3));
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            MoveForward();
            gameObject.transform.localScale -= new Vector3(dissapearanceRate, dissapearanceRate);
            if (gameObject.transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
