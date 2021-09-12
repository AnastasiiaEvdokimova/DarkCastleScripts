using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBall_Behaviour : Projectile_Behaviour
{
    private float radians, velocity, radiusX, radiusY, radiusGrowth, x, y, lifeTime, creationTime, followingSpeed, maxFollowingDistance;
    public GameObject StaticEffect;
    Vector2 centerPoint;
    Rio_Abilities rio;
    GameObject player;
    bool isPushed, isFollowing, isFollowingDash;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        lifeTime = 15;
        radians = 0;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
        spriteHeight = spriteRenderer.bounds.size.y;
        rio = GameObject.Find("Boss_Rio").GetComponent<Rio_Abilities>();
        isPushed = false;
        isFollowingDash = false;
        basicDamage = 2;
        followingSpeed = 3f;
        maxFollowingDistance = spriteWidth * 2;
    }

    public void Push(float xSpeed, float ySpeed)
    {
        isPushed = true;
        this.xSpeed = xSpeed;
        this.ySpeed = ySpeed;
    }

    public void Cast(Vector2 startingPosition, float radiusGrowthSpeed, float SpiralCenterMovementSpeedX, float SpiralCenterMovementSpeedY, float rotationSpeed)
    {
        SetBoundaries();
        if (startingPosition.x > maxX)
        {
            startingPosition.x -= GameObject.Find("Boss_Rio").GetComponent<SpriteRenderer>().bounds.size.x * 1.5f;
        }
        gameObject.transform.position = startingPosition;
        velocity = rotationSpeed * Time.deltaTime;
        radiusX = 0.2f;
        radiusY = radiusX;
        xSpeed = SpiralCenterMovementSpeedX*Time.deltaTime;
        ySpeed = SpiralCenterMovementSpeedY * Time.deltaTime;
        creationTime = Time.time;
         centerPoint = new Vector2(startingPosition.x, startingPosition.y);
        radiusGrowth = radiusGrowthSpeed * Time.deltaTime;
    }

    private void SpiralMove()
    {
        centerPoint = centerPoint + new Vector2(xSpeed, ySpeed);
        radians += velocity;
       x =  Mathf.Cos(radians) * radiusX;
        y =  Mathf.Sin(radians) * radiusY;
        gameObject.transform.position = new Vector2(centerPoint.x + x, centerPoint.y + y);
        if (centerPoint.x + radiusX + radiusGrowth < maxX - spriteWidth/2 && centerPoint.x - radiusX - radiusGrowth > minX + spriteWidth/2) radiusX += radiusGrowth;
        if (centerPoint.y + radiusY + radiusGrowth < maxY - spriteHeight/2 && centerPoint.y - radiusY - radiusGrowth > minY + spriteHeight/2) radiusY += radiusGrowth;
    }

    private void CheckPlayerMovement()
    {      
        if ((player.GetComponent<Player_Movement>().IsMoving())&&(Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) <= maxFollowingDistance) && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) <= maxFollowingDistance))
        {
            isFollowing = true;
        } else
        if (player.name == "Player_Orion")
           {
          if ((player.GetComponent<Orion_Abilities>().isDashing()))
            {
                isFollowingDash = true;
            }
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            rio.BallDissapeared();
            Destroy(gameObject);
        }
        if (collision.tag == "Player")
        {
            player.GetComponent<Player_Status>().LoseHP(basicDamage);
            player.GetComponent<Player_Movement>().StopPlayer();
            player.GetComponent<Player_Movement>().AffectSpeed(0, 0.5f);
            Instantiate(StaticEffect).GetComponent<ElectrifiedAnimation_Behaviour>().Bind(GameObject.FindWithTag("Player"));
            rio.BallDissapeared();
            Destroy(gameObject);
        }
        if (collision.name == "IceShard(Clone)")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isFollowing)
            {
                if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) >= maxFollowingDistance) || (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) >= maxFollowingDistance))
                {
                    isFollowing = false;
                    isPushed = true;
                }
                GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, followingSpeed, out xSpeed, out ySpeed);
            }
            else
            if (isFollowingDash)
            {
                if (!(player.GetComponent<Orion_Abilities>().isDashing()))
                {
                    isFollowingDash = false;
                    isPushed = true;
                }
                GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, followingSpeed * 1.6f, out xSpeed, out ySpeed);
            }

            if (!isPushed && !isFollowing && !isFollowingDash)
            {
                SpiralMove();
                CheckPlayerMovement();
            }
            else
            {
                MoveForward();
            }

            if (Time.time - creationTime > lifeTime)
            {
                rio.BallDissapeared();
                Destroy(gameObject);
            }
        }
    }
}
