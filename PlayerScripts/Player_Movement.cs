using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private float baseSpeed = 5f;
    private float speedMultiplier = 1, normalSpeedCooldown;
    private bool isMoving = false, speedIsAltered = false, isPushed = false, isPullingBack = false;
    private float xSpeed, xPercent;
    private float ySpeed, yPercent;
    private byte weightType;
    private Vector2 destinationPoint;
    void Start()
    {
        xSpeed = 0;
        ySpeed = 0;
        if (gameObject.name == "Player_Orion") //Orion wears heavy armor
        {
            weightType = 3;
        }
        else if (gameObject.name == "Player_Hyun" || gameObject.name == "Player_Lydia")
        {
            weightType = 1;
        }
        else
        {
            weightType = 2;
        }

    }

    public void AffectSpeed(float speedMultiplier, float effectTime)
    {
        if (!speedIsAltered)
        {
            speedIsAltered = true;
            normalSpeedCooldown = Time.time + effectTime;
            this.speedMultiplier = speedMultiplier;
        }
        else
        {
            normalSpeedCooldown = Time.time + effectTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPullingBack){
            isPullingBack = false;
            StopPlayer();
        }
    }
    
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isPullingBack)
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed, gameObject.transform.position.y + ySpeed);
            }
            else
        if (isMoving)
            {
                if (speedIsAltered)
                {
                    if (Time.time >= normalSpeedCooldown)
                    {
                        speedIsAltered = false;
                        speedMultiplier = 1;
                    }
                }
                if ((Mathf.Abs(destinationPoint.x - gameObject.transform.position.x) <= Mathf.Abs(xSpeed * speedMultiplier)) && (Mathf.Abs(destinationPoint.y - gameObject.transform.position.y) <= Mathf.Abs(ySpeed * speedMultiplier))) //|| Mathf.Abs(destinationPoint.y - gameObject.transform.position.y) <= ySpeed)
                {
                    gameObject.transform.position = new Vector2(destinationPoint.x, destinationPoint.y);
                    xSpeed = 0;
                    ySpeed = 0;
                    isMoving = false;
                }
                else
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed * speedMultiplier, gameObject.transform.position.y + ySpeed * speedMultiplier);
                }
            }
            else
        if (isPushed)
            {
                if ((Mathf.Abs(destinationPoint.x - gameObject.transform.position.x) <= Mathf.Abs(xSpeed)) && (Mathf.Abs(destinationPoint.y - gameObject.transform.position.y) <= Mathf.Abs(ySpeed))) //|| Mathf.Abs(destinationPoint.y - gameObject.transform.position.y) <= ySpeed)
                {
                    gameObject.transform.position = new Vector2(destinationPoint.x, destinationPoint.y);
                    xSpeed = 0;
                    ySpeed = 0;
                    isPushed = false;
                }
                else
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed * speedMultiplier, gameObject.transform.position.y + ySpeed * speedMultiplier);
                }
            }
        }
    }

    public void Push(float speed, Vector2 distance)
    {
        if (weightType == 1) {
            distance.x *= 1.3f; distance.y *= 1.3f;
        }
        else if (weightType == 3)
        {
            distance.x *= 0.7f; distance.y *= 0.7f;
        }
        destinationPoint = (Vector2)gameObject.transform.position + distance;
        isPushed = true;
        isMoving = false;
        GeneralMethods.CalculateSpeed(gameObject.transform.position, destinationPoint, speed, out xSpeed, out ySpeed);
    }

    public void StopPlayer()
    {
        if (isPushed)
        {
            isPushed = false;
        }
        isMoving = false;
        xSpeed = 0;
        ySpeed = 0;
    }

    public void WallContact(float speed, float wallType)
    {
        if (wallType == 0 || wallType == 1) { xSpeed = speed; ySpeed = 0; }
        if (wallType == 3 || wallType == 3) { ySpeed = speed; xSpeed = 0; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Iceberg(Clone)" && !collision.GetComponent<Iceberg_Behaviour>().isPassingAllowed())
        {
            float collisionHeight = collision.GetComponent<BoxCollider2D>().size.y / 2;
            float collisionWidth = collision.GetComponent<BoxCollider2D>().size.x / 2;
            float playerHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
            float playerWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
           
                float currentSpeedVector = xSpeed * xSpeed + ySpeed * ySpeed;
                float icebergSizeVector = collisionHeight * collisionHeight + collisionWidth * collisionWidth;
            if (xSpeed!=0) { 
            if (currentSpeedVector != 0)
            {
                if (xSpeed > baseSpeed) xSpeed = baseSpeed;
                xSpeed = -xSpeed * icebergSizeVector * Time.deltaTime / currentSpeedVector;
                ySpeed = -ySpeed * icebergSizeVector * Time.deltaTime / currentSpeedVector;
            }
                destinationPoint = collision.transform.position;
                if (isPushed) isPushed = false;
                isMoving = false;
                isPullingBack = true;
            }
            else
            {
                gameObject.transform.position = new Vector2(gameObject.transform.position.x, collision.transform.position.y - (playerHeight / 2 + collisionHeight) * ySpeed / Mathf.Abs(ySpeed));
                isPushed = false;
                isMoving = false;
            }
                
        }
    }
    
    public void SetPoint(float mouseX, float mouseY)
    {
        if (!isPushed)
        {
            destinationPoint = new Vector2(mouseX, mouseY);
            GeneralMethods.CalculateSpeed(gameObject.transform.position, destinationPoint, baseSpeed, out this.xSpeed, out this.ySpeed);
            isMoving = true;
        }
    }

    public void GetSpeed(out float xSpeed, out float ySpeed)
    {
        xSpeed = this.xSpeed;
        ySpeed = this.ySpeed;
    }

    public bool IsSpeedAltered()
    {
        return speedIsAltered;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsPushed()
    {
        return isPushed;
    }
}
