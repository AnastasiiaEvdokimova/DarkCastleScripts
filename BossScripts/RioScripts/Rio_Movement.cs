using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rio_Movement : Boss_Movement
{
    private float movementCooldown, lastMovementTime, activationTime, startTime;
    private float xSpeed, ySpeed, basicSpeed;
    Vector2 destinationPoint;
    public GameObject dash;
    GameObject lightingDash;
    bool isMoving, startedCharging, dashStopping, isDashing;
    void Start()
    {
        Starting();
        isMoving = false;
        dashStopping = false;
        startedCharging = false;
        playerIsClose = false;
        basicSpeed = 8f;
        lastMovementTime = Time.time;
        movementCooldown = 5;
        activationTime = 0.7f;
    }

    public void StartMoving(bool isDashing)
    {
        startTime = Time.time;
        destinationPoint = ChooseMovementPoint();
        GeneralMethods.CalculateSpeed(gameObject.transform.position, destinationPoint, basicSpeed, out xSpeed, out ySpeed);
        startedCharging = true;
        if (isDashing)
        {
            if (lightingDash != null)
            {
                Destroy(lightingDash);
            }
            lightingDash = Instantiate(dash);
            lightingDash.transform.position = gameObject.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            this.isDashing = isDashing;
        }
        else
        {
            startTime -= activationTime;
        }
    }

    private void Moving()
    {
        if ((Mathf.Abs(destinationPoint.x - gameObject.transform.position.x) <= Mathf.Abs(xSpeed)) && (Mathf.Abs(destinationPoint.y - gameObject.transform.position.y) <= Mathf.Abs(ySpeed)))
        {
            gameObject.transform.position = new Vector2(destinationPoint.x, destinationPoint.y);
            xSpeed = 0;
            ySpeed = 0;
            isMoving = false;
            lastMovementTime = Time.time;
            startTime = Time.time;
            dashStopping = true;
        }
        else
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed, gameObject.transform.position.y + ySpeed);
            if (isDashing && lightingDash!=null)
            {
                lightingDash.transform.position = gameObject.transform.position;
            }
        }
    }

    public void Stop()
    {
        xSpeed = 0;
        ySpeed = 0;
        startedCharging = false;
        isMoving = false;
        lastMovementTime = Time.time;
        startTime = Time.time;
        dashStopping = true;
    }
    
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isPushed && (startedCharging || isMoving)) //to prevent movement error of not reaching destination point
            {
                Stop();
            }
            if (startedCharging)
            {
                if (Time.time - activationTime > startTime)
                {
                    startedCharging = false;
                    isMoving = true;
                }
            }
            else
            if (isMoving)
            {
                Moving();
            }
            else if (dashStopping)
            {
                if (Time.time - activationTime > startTime)
                {
                    dashStopping = false;
                    if (isDashing)
                    {

                        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
                        Destroy(lightingDash);
                        isDashing = false;
                    }
                }
            }
            else if (Time.time - lastMovementTime > movementCooldown)
            {
                StartMoving(true);
            }
            if (playerIsClose)
            {
                if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) >= requiredX) && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) >= requiredY))
                {
                    playerIsClose = false;
                }
                else if (isMoving || startedCharging)
                {
                    playerIsClose = true;
                }
                else if (Time.time - playerDetectionTime >= runAwayTime)
                {
                    StartMoving(true);
                    playerIsClose = false;
                }
            }
            else
              if (!isMoving && !startedCharging && (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < requiredX) && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < requiredY))
            {
                playerIsClose = true;
                playerDetectionTime = Time.time;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Iceberg(Clone)" && !collision.GetComponent<Iceberg_Behaviour>().isPassingAllowed())
        {
            Stop();
        }
    }

    public void DashDestroyed()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
        StartMoving(false);

    }
    public bool IsMoving()
    {
        return isMoving;
    }
}
