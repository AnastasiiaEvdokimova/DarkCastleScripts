using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceberg_Behaviour : MonoBehaviour
{
    public static int icebergCounter = 0;
    public GameObject puddle;
    private GameObject currentPuddle;
    SpriteRenderer sprite;
    private float maxX, maxY, minX, minY;
    private float scaleX, scaleY;
    private float meltingRate;
    bool canMerge;
    bool isMelting;

    void Start()
    {
        SetBoundaries();
    }

    private void SetPuddle()
    {
        if (currentPuddle == null) { 
            currentPuddle = Instantiate(puddle);
            if (currentPuddle != null)
            {   try
                {  currentPuddle.GetComponent<WaterTrail_Behaviour>().CreatePuddle(gameObject.transform.position, 1, 1, gameObject.transform.localScale.x / 2, true);
                } catch {  }
            }
        }
    }

    private void StartMelting()
    {
        meltingRate = 0.05f*Time.deltaTime;
        SetPuddle();
        isMelting = true;
        EventManager.CreationFinished -= StartMelting;
    }

    private void SetBoundaries()
    {
        GameObject area = GameObject.Find("Input_Area");
        minX = area.GetComponent<Collider>().bounds.center.x - area.GetComponent<Collider>().bounds.size.x / 2;
        maxX = area.GetComponent<Collider>().bounds.center.x + area.GetComponent<Collider>().bounds.size.x / 2;
        minY = area.GetComponent<Collider>().bounds.center.y - area.GetComponent<Collider>().bounds.size.y / 2;
        maxY = area.GetComponent<Collider>().bounds.center.y + area.GetComponent<Collider>().bounds.size.y / 2;
    }

    public void Create(Vector2 position)
    {
        float height, width;
        gameObject.transform.position = position;
        canMerge = true;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        height = sprite.bounds.size.y;
        width = sprite.bounds.size.x;
        scaleX = width / gameObject.transform.localScale.x;
        scaleY = height / gameObject.transform.localScale.y;
        isMelting = false;
        meltingRate = 0; //even if heat is presented, supported Iceberg won't melt
        EventManager.CreationFinished += StartMelting;
        Iceberg_Behaviour.icebergCounter += 1;
    }

    private void DestroyIce()
    {
        if (currentPuddle != null) currentPuddle.GetComponent<WaterTrail_Behaviour>().IceMelt();
        EventManager.CreationFinished -= StartMelting;
        Iceberg_Behaviour.icebergCounter -= 1;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if (canMerge)
        {
            if (collision.name == "Iceberg(Clone)")
            {
                collision.GetComponent<Iceberg_Behaviour>().Grow(scaleX * Time.deltaTime);
                DestroyIce();
            }
            if (collision.tag == "Boss" || collision.tag == "Player")
            {
                DestroyIce();
            }
            if (collision.tag == "Fire")
            {
                DestroyIce();
            }
        }
        else
        {
            if (collision.tag == "Boss" || collision.tag == "Player")
            {
                maxX = minX; //this stops iceberg from further growth
            }
            if (collision.tag == "Wall" && (gameObject.transform.localScale.x > 0.5))
            {
                collision.GetComponent<Wall_Behaviour>().Vanish();
            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (gameObject.transform.localScale.x > 1)
            {
                if (collision.GetComponent<Projectile_Behaviour>() != null)
                {
                    collision.GetComponent<Projectile_Behaviour>().Destroy();
                    if (collision.name == "Light Arrow(Clone)") { if (!isMelting) StartMelting(); Melt(); }
                    if (collision.name == "WillOWisp(Clone)") { if (!isMelting) StartMelting(); meltingRate = 0.1f * Time.deltaTime; Melt(); }
                }
            }
        }
    }

    public bool Grow(float growthRate)
    { if (canMerge) { canMerge = false; }
        if (gameObject.transform.position.x + (sprite.bounds.size.x + scaleX * growthRate)/2 > maxX || gameObject.transform.position.x - (sprite.bounds.size.x + scaleX * growthRate) / 2 < minX
            || gameObject.transform.position.y + (sprite.bounds.size.y + scaleY * growthRate) / 2 > maxY || gameObject.transform.position.y - (sprite.bounds.size.y + scaleY * growthRate) / 2 < minY)
        {
            return false;
        }
        else
        {
            gameObject.transform.localScale += new Vector3(growthRate, growthRate);
            return true;
        }
    }
    public void Melt(bool destruction = false)
    {
  if (currentPuddle == null)
        {
            SetPuddle();
        }
        else
        {
            if (destruction || gameObject.transform.localScale.x - meltingRate < 0) meltingRate = gameObject.transform.localScale.x;
            currentPuddle.transform.localScale += new Vector3(meltingRate, meltingRate);
            gameObject.transform.localScale -= new Vector3(meltingRate, meltingRate);
            if (gameObject.transform.localScale.x <= 0)
            {
                DestroyIce();
            }
        }
    }

   public bool isPassingAllowed()
    {
        if (gameObject.transform.localScale.x < 0.5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isMelting)
            {
                Melt();
            }
        }
    }
}
