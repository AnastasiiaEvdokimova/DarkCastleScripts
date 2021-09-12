using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ringo_Movement : Boss_Movement
{
    private float movementCooldown, lastMovementTime, createWispsCooldown, lastWispTime, wispAmount;
    private float xSpeed, ySpeed, basicSpeed;
    bool isMoving, isRunAway;
    Vector2 destinationPoint;
    Ringo_Abilities ringo;
    private void Start()
    {
        Starting();
        basicSpeed = 4f;
        movementCooldown = 10f; //he rarely moves
        ringo = GetComponent<Ringo_Abilities>();
        createWispsCooldown = 0.5f;
        lastMovementTime = Time.time;
    }

    private void Moving()
    {
       if (Time.time - lastWispTime > createWispsCooldown && !isRunAway && wispAmount < 4)
        {
            float radius = Random.Range(gameObject.GetComponent<SpriteRenderer>().bounds.size.y, gameObject.GetComponent<SpriteRenderer>().bounds.size.y * 1.5f);
            ringo.CastTravelFire((destinationPoint.x - gameObject.transform.position.x)/xSpeed, radius);
            lastWispTime = Time.time;
            wispAmount++;
        }
        if ((Mathf.Abs(destinationPoint.x - gameObject.transform.position.x) <= Mathf.Abs(xSpeed)) && (Mathf.Abs(destinationPoint.y - gameObject.transform.position.y) <= Mathf.Abs(ySpeed)))
        {
            gameObject.transform.position = new Vector2(destinationPoint.x, destinationPoint.y);
            xSpeed = 0;
            ySpeed = 0;
            isMoving = false;
            lastMovementTime = Time.time;
        }
        else
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed, gameObject.transform.position.y + ySpeed);
        }
    }

    public void StartMoving(bool isRunAway)
    {
        isMoving = true;
        this.isRunAway = isRunAway;
        destinationPoint = ChooseMovementPoint();
        GeneralMethods.CalculateSpeed(gameObject.transform.position, destinationPoint, basicSpeed, out xSpeed, out ySpeed);
        if (!isRunAway) ringo.CastTravelFire(3, gameObject.GetComponent<SpriteRenderer>().bounds.size.y);
        lastWispTime = Time.time;
        wispAmount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Iceberg(Clone)" && !collision.GetComponent<Iceberg_Behaviour>().isPassingAllowed())
        {
            Stop();
        }
    }

    private void Stop()
    {
        xSpeed = 0;
        ySpeed = 0;
        isMoving = false;
        lastMovementTime = Time.time;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isPushed && isMoving) //to prevent movement error of not reaching destination point
            {
                xSpeed = 0;
                ySpeed = 0;
                isMoving = false;
                lastMovementTime = Time.time;
            }
            if (isMoving)
            {
                Moving();
            }
            else if (Time.time - lastMovementTime > movementCooldown)
            {
                StartMoving(false);
            }
            if (!playerIsClose)
            {
                if (!isMoving && (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < requiredX) && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < requiredY))
                {
                    playerIsClose = true;
                    playerDetectionTime = Time.time;
                }
            }
            else if (Time.time - playerDetectionTime > runAwayTime)
            {
                playerIsClose = false;
                StartMoving(false);
            }
        }
    }
}
