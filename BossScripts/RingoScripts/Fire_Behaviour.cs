using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Behaviour : Projectile_Behaviour
{
    public GameObject firePrefab, heat, smoke;
    private GameObject newFire, previousFire;
    public static int fireAmount = 0;
    private float distributionCooldown, creationTime, burningTime;
    bool canDistribute, waterContacted;
    private int cloningCapability;
    private float size, extinguishingRate, burningRate;
    void Start()
    {
        SetBoundaries();
        distributionCooldown = 0.9f;
        extinguishingRate = 0.5f * Time.deltaTime;
        basicDamage = 1f;
        waterContacted = false;
        Fire_Behaviour.fireAmount += 1;
    }

    private void BurnUp()
    {
        gameObject.transform.localScale += new Vector3(burningRate, burningRate);
    }

    public void SetFlame(Vector2 position, float size, int cloningCapability, float xSpeed, float ySpeed, float burningTime = 3)
    {
        canDistribute = true;
        gameObject.transform.position = position;
       this.size = size;
        this.cloningCapability = cloningCapability;
        this.xSpeed = xSpeed;
        this.ySpeed = ySpeed;
        creationTime = Time.time;
        this.burningRate = size/burningTime * Time.deltaTime;
        this.burningTime = burningTime;
        Instantiate(heat).GetComponent<Heat_Behaviour>().Set(gameObject);
        if (ySpeed < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
        }
    }

    private void Extinguish()
    {
        gameObject.transform.localScale -= new Vector3(extinguishingRate, extinguishingRate);
        if (gameObject.transform.localScale.x <= 0)
        {
                extinguishingRate /= Time.deltaTime;
            if (extinguishingRate > size) extinguishingRate = size*2;
                Instantiate(smoke).GetComponent<Smoke_Behaviour>().SetCloud(gameObject.transform.position, extinguishingRate, 3);
            Fire_Behaviour.fireAmount--;
            Destroy(gameObject);
        }
    }

    private bool CheckNextLocation()
    {
        float currentWidth = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
        float currentHeight = gameObject.GetComponent<BoxCollider2D>().bounds.size.y;
        if (Mathf.Abs(xSpeed) > currentWidth)
        {
            xSpeed = currentWidth * xSpeed/Mathf.Abs(xSpeed) * 2;
        }
        if (Mathf.Abs(ySpeed) > currentHeight)
        {
            ySpeed = currentHeight / Mathf.Abs(ySpeed) * 2;
        }
        if ((gameObject.transform.position.x + xSpeed > maxX || gameObject.transform.position.x + xSpeed < minX) || (gameObject.transform.position.y + ySpeed > maxY || gameObject.transform.position.y + ySpeed < minY)) //if stuck in the corner
        {
            cloningCapability = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void WaterContact()
    { if (!waterContacted)
        {
            extinguishingRate *= 10;
            canDistribute = false;
            creationTime = Time.time - burningTime; //start the process of extinguishing
            waterContacted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Trail")
        {
            WaterContact();
        }
       if (collision.name == "Wind_Slash(Clone)")
        {
            float scale = collision.GetComponent<WindSlash_Behaviour>().GetSize();
            if (2.7 * scale < gameObject.transform.localScale.x)
            {
                Vector2 windSpeed = collision.GetComponent<WindSlash_Behaviour>().GetSpeed();
                SetFlame(gameObject.transform.position, size + scale, (int)scale*2, windSpeed.x, windSpeed.y);
                collision.GetComponent<WindSlash_Behaviour>().Destroy();
            }
            else
            {
                WaterContact();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<Player_Status>().LoseHP(basicDamage * Time.deltaTime);
            }
        }
    }

    public float GetSize()
    {
        return gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
    }

    private void Distribute()
    {
        if (CheckNextLocation() && Fire_Behaviour.fireAmount < 20)
        {
           newFire = Instantiate(firePrefab);
            if (ySpeed < 0) {
                newFire.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
                    }
            else
            {
                newFire.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
            newFire.GetComponent<Fire_Behaviour>().SetFlame(new Vector2(gameObject.transform.position.x + xSpeed/2, gameObject.transform.position.y + ySpeed/2), size + extinguishingRate/3, cloningCapability - 1, xSpeed, ySpeed);
        }
    }
    
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (canDistribute == true && cloningCapability > 0 && Time.time - creationTime > distributionCooldown)
            {
                Distribute();
                canDistribute = false;
            }
            if (Time.time - creationTime < burningTime)
            {
                BurnUp();
            }
            else
            {
                Extinguish();
            }
        }
    }
}
