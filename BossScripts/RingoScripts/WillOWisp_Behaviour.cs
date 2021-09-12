using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWisp_Behaviour : Projectile_Behaviour
{
    float creationTime, followingTime, speed;
    private float radians, velocity, radius, dryingRate = 0.2f;
    Vector2 centerPoint;
    bool animationChangeRequired, isTravelFire;
    public AnimationClip regularFireAnimation;
    AnimatorOverrideController aoc;
    GameObject player;
    
    private void Follow()
    {
        GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, speed, out xSpeed, out ySpeed);
    }

    public void Cast(Vector2 position, float speed, float followingTime, bool isMovingHelper = false, float radius = 0)
    {
        basicDamage = 1f;
        SetBoundaries();
        dryingRate *= Time.deltaTime;
        Animator wispAnimation = gameObject.GetComponent<Animator>();
        aoc = new AnimatorOverrideController(wispAnimation.runtimeAnimatorController);
        wispAnimation.runtimeAnimatorController = aoc;
        creationTime = Time.time;
        animationChangeRequired = true;
        this.speed = speed;
        this.followingTime = followingTime;
        isTravelFire = isMovingHelper;
        if (!isMovingHelper)
        {
            player = GameObject.FindWithTag("Player");
            GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, speed, out xSpeed, out ySpeed);
            gameObject.transform.position = position;
            //SpriteAdjustment(position);
        }
        else
        {
            radians = 0;
            this.radius = radius;
            player = GameObject.FindWithTag("Boss");
            centerPoint = position;
            velocity = 2*Mathf.PI /4 *speed * Time.deltaTime;
            int rnd = (int)Random.Range(0, 2);
            if (rnd == 0) velocity = -velocity;
            Rotate();
        }
    }

    private void Rotate()
    {
        float x, y;
        centerPoint = player.transform.position;
        radians += velocity;
        x = Mathf.Cos(radians) * radius;
        y = Mathf.Sin(radians) * radius;
        xSpeed = (centerPoint.x + x - gameObject.transform.position.x)/2;
        ySpeed = (centerPoint.y + y - gameObject.transform.position.y)/2;
        gameObject.transform.position = new Vector2(centerPoint.x + x, centerPoint.y + y);
        CheckBoundaries();
    }

    private void ChangeAnimation()
    {
        aoc["WillOWispAlt"] = regularFireAnimation;
        animationChangeRequired = false;
    }
    
    void Update()
    {
        if (xSpeed == 0 && ySpeed == 0)
        {
            Destroy(gameObject);
        }
        if (Time.timeScale != 0)
        {
            if (animationChangeRequired)
            {
                if (Time.time - creationTime > followingTime)
                {
                    ChangeAnimation();
                }
                else
                {
                    if (!isTravelFire)
                    {
                        Follow();
                        MoveForward();
                    }
                    else
                    {
                        if (centerPoint == (Vector2)player.transform.position)
                        {
                            ChangeAnimation();
                        }
                        Rotate();
                    }
                }
            }
            else
            {
                MoveForward();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (collision.tag == "Trail" && !collision.GetComponent<WaterTrail_Behaviour>().IsIceWater())
            {
                collision.GetComponent<WaterTrail_Behaviour>().SetSize(new Vector3(collision.transform.localScale.x - dryingRate, collision.transform.localScale.y - dryingRate));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player_Status>().LoseHP(basicDamage);
            Destroy(gameObject);
        }
        if (collision.name == "IceShard(Clone)")
        {
            gameObject.transform.localScale -= new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.x / 2);
            if (gameObject.transform.localScale.x <=0)
            {
                Destroy(gameObject);
            }
        }

    }
}
