using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBolt_Behaviour : Projectile_Behaviour
{
    private byte hitCount = 3;
    public static byte boltHitCounter;
    public GameObject StaticEffect;
    
    protected override void SetBoundaries()
    {
        basicDamage = 0.3f;
        GameObject area = GameObject.Find("Input_Area");
        minX = area.GetComponent<Collider>().bounds.center.x - area.GetComponent<Collider>().bounds.size.x;
        maxX = area.GetComponent<Collider>().bounds.center.x + area.GetComponent<Collider>().bounds.size.x;
        minY = area.GetComponent<Collider>().bounds.center.y - area.GetComponent<Collider>().bounds.size.y;
        maxY = area.GetComponent<Collider>().bounds.center.y + area.GetComponent<Collider>().bounds.size.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall") //bounce from the wall, then dissapear
        {
            Wall_Behaviour wall = collision.GetComponent<Wall_Behaviour>();
            if (wall.isVertical())
            {
                xSpeed = -xSpeed;
            }
            else
            {
                ySpeed = -ySpeed;
            }
            gameObject.transform.rotation =  Quaternion.Euler(new Vector3(0, 0, 90));
            RotateSprite();
            hitCount--;
            if (hitCount == 0)
            {
                Destroy(gameObject);
            }
        }
        if (collision.name == "Iceberg(Clone)")
        {
            xSpeed = -xSpeed;
            ySpeed = -ySpeed;
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            RotateSprite();
            hitCount--;
            if (hitCount == 0)
            {
                Destroy(gameObject);
            }
        }
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player_Status>().LoseHP(basicDamage);
            LightingBolt_Behaviour.boltHitCounter++;
            if (LightingBolt_Behaviour.boltHitCounter == 3)
            {
                LightingBolt_Behaviour.boltHitCounter = 0;
                collision.GetComponent<Player_Movement>().AffectSpeed(0.3f, 3);
                Instantiate(StaticEffect).GetComponent<ElectrifiedAnimation_Behaviour>().Bind(GameObject.FindWithTag("Player"));
            }
            Destroy(gameObject);
        }
        if (collision.tag == "Trail")
        {
            if (!collision.GetComponent<WaterTrail_Behaviour>().IsElectrified()) Destroy(gameObject);
        }
    }
    
    void Update()
    {if (Time.timeScale!=0)
        MoveForward();
    }
}
