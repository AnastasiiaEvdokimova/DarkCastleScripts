using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard_Behaviour : Projectile_Behaviour
{
    public AnimationClip IceBreaking;
    public GameObject puddle;
    GameObject currentPuddle;
    AnimatorOverrideController aoc;
    float meltingRate;
    bool isBreaking, isMelting;
    
    void Start()
    {
        Animator shardAnimation = gameObject.GetComponent<Animator>();
        aoc = new AnimatorOverrideController(shardAnimation.runtimeAnimatorController);
        shardAnimation.runtimeAnimatorController = aoc;
        isBreaking = false;
        basicDamage = 0.3f;
        meltingRate = 0.05f;
    }

    public void Melt()
    {
        if (currentPuddle == null)
        {
            SetPuddle();
        }
        else
        {
            currentPuddle.transform.localScale += new Vector3(meltingRate, meltingRate);
            gameObject.transform.localScale -= new Vector3(meltingRate, meltingRate);
            if (gameObject.transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void SetPuddle()
    {
        if (currentPuddle == null)
        {
            currentPuddle = Instantiate(puddle);
            if (currentPuddle != null)
            {
                currentPuddle.GetComponent<WaterTrail_Behaviour>().CreatePuddle(gameObject.transform.position, 1, 1, gameObject.transform.localScale.x / 4);
            }
        }
    }

    public void Break()
    {
        aoc["IceShard"] = IceBreaking;
        isBreaking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            collision.GetComponent<Boss_Status>().DealDamage(basicDamage);
            Break();
        }
        if (collision.name == "Light Arrow(Clone)") { Break(); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "WillOWisp(Clone)")
        {
            Melt();
        }
    }

    private bool WallCrush()
    {
        float height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        float width = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        if (gameObject.transform.position.x + width >= maxX || gameObject.transform.position.x - width <= minX || gameObject.transform.position.y + height >= maxY || gameObject.transform.position.y - height <= minY)
        {
            Break();
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
            if (isBreaking && gameObject.GetComponent<SpriteRenderer>().sprite.name == "IceShardBreaking_3")
            {
                Destroy(gameObject);
            }
            if (!WallCrush())
            {
                if (!isBreaking) MoveForward();
            }
        }
    }
}
