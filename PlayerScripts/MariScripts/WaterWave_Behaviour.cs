using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave_Behaviour : Projectile_Behaviour
{
    bool isTrailLeft, isFirstPrefab;
    GameObject lastTrail;
    Mari_Abilities mari;
    public GameObject trailPrefab1, trailPrefab2, waterSplash;
    float trailSize;
    float dissapearanceRate;

    private void Start()
    {
        SetBoundaries();
        isTrailLeft = false;
        dissapearanceRate = 0.1f *Time.deltaTime;
        trailSize = trailPrefab1.GetComponent<CircleCollider2D>().radius * 0.2f;
        mari = GameObject.FindWithTag("Player").GetComponent<Mari_Abilities>();
        isFirstPrefab = true;
    }

    private bool WallCrush()
    {
        if (gameObject.transform.position.x >= maxX || gameObject.transform.position.x <= minX || gameObject.transform.position.y >= maxY || gameObject.transform.position.y <= minY)
        {
            DestroyWave();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyWave()
    {
        GameObject splash = Instantiate(waterSplash);
        splash.GetComponent<Splash_Behaviour>().Splash(gameObject.transform.position);
        mari.waveDestroyed();
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (collision.tag == "Trail")
            {
                collision.GetComponent<WaterTrail_Behaviour>().SetSize(collision.transform.localScale + new Vector3(dissapearanceRate, dissapearanceRate));
                isTrailLeft = true;
            }
            if (collision.tag == "Boss")
            {
                collision.GetComponent<Boss_Movement>().Push(xSpeed, ySpeed, 0.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" && collision.GetComponent<Wall_Behaviour>().IsMoved()) {
                collision.GetComponent<Wall_Behaviour>().Vanish();
            }

        if (collision.tag == "Fire") { collision.GetComponent<Fire_Behaviour>().WaterContact(); }
        else
        if (collision.GetComponent<Projectile_Behaviour>()!= null)
        {
            collision.GetComponent<Projectile_Behaviour>().Destroy();
        }
        if (collision.name == "Light Arrow(Clone)")
        {
            DestroyWave();
        }
      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrailLeft = false;
    }



    private void LeaveTrail()
    {
            if (isFirstPrefab)
                lastTrail = Instantiate(trailPrefab1);
            else
                lastTrail = Instantiate(trailPrefab2);
        isFirstPrefab = !isFirstPrefab;
            lastTrail.GetComponent<WaterTrail_Behaviour>().CreatePuddle(gameObject.transform.position, xSpeed, ySpeed, gameObject.transform.localScale.x*2);
        isTrailLeft = true;
    }

    protected override void SpriteAdjustment(Vector2 startingPoint)
    {
        gameObject.transform.position = startingPoint + new Vector2(xSpeed/Mathf.Abs(xSpeed)*spriteWidth/2, ySpeed / Mathf.Abs(ySpeed)*spriteHeight / 2);
        lastTrail = Instantiate(trailPrefab2);
        lastTrail.GetComponent<WaterTrail_Behaviour>().CreatePuddle(gameObject.transform.position, xSpeed, ySpeed);
    }

    public void Dry()
    {
            gameObject.transform.localScale -= new Vector3(dissapearanceRate, dissapearanceRate);
            if (gameObject.transform.localScale.x <= 0)
            {
                mari.waveDestroyed();
                Destroy(gameObject);
            }
        
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            MoveForward();
            if (!WallCrush())
            {
                if (lastTrail == null || !isTrailLeft && (((Mathf.Abs(gameObject.transform.position.x - lastTrail.transform.position.x) > trailSize) || (Mathf.Abs(gameObject.transform.position.y - lastTrail.transform.position.y) > trailSize))))
                    LeaveTrail();
                gameObject.transform.localScale -= new Vector3(dissapearanceRate, dissapearanceRate);
                if (gameObject.transform.localScale.x <= 0)
                {
                    mari.waveDestroyed();
                    Destroy(gameObject);
                }
            }
        }
    }
}
