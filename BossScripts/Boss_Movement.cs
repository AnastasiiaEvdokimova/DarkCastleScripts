using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Movement : MonoBehaviour
{
    protected float spriteHeight, spriteWidth, minX, maxX, minY, maxY;
    protected float requiredX, requiredY;
    protected float runAwayTime, playerDetectionTime;
    protected bool playerIsClose, isPushed;
    protected float pushX, pushY;
    protected GameObject area, player;

    protected void  Starting()
    {
        runAwayTime = 2f;
        area = GameObject.Find("Input_Area");
        player = GameObject.FindWithTag("Player");
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
        spriteHeight = spriteRenderer.bounds.size.y;
        minX = area.GetComponent<Collider>().bounds.center.x - area.GetComponent<Collider>().bounds.size.x / 2 + spriteWidth / 2;
        maxX = area.GetComponent<Collider>().bounds.center.x + area.GetComponent<Collider>().bounds.size.x / 2 - spriteWidth / 2;
        minY = area.GetComponent<Collider>().bounds.center.y - area.GetComponent<Collider>().bounds.size.y / 2 + spriteHeight / 2;
        maxY = area.GetComponent<Collider>().bounds.center.y + area.GetComponent<Collider>().bounds.size.y / 2 - spriteHeight / 2;
        requiredX = area.GetComponent<Collider>().bounds.size.x / 4 + spriteWidth;
        requiredY = area.GetComponent<Collider>().bounds.size.y / 4 + spriteHeight;
    }


    protected Vector2 ChooseMovementPoint()
    {
        Vector2 newPosition = new Vector3(Random.Range(minX + spriteWidth / 2, maxX - spriteWidth / 2), Random.Range(minY + spriteHeight / 2, maxY - spriteWidth / 2));
        float distanceFromPlayer = player.transform.position.x - newPosition.x * player.transform.position.x - newPosition.x + player.transform.position.y - newPosition.y * player.transform.position.y - newPosition.y; //no sqrt for optimisation
        if (Mathf.Abs(player.transform.position.x - newPosition.x) < requiredX && Mathf.Abs(player.transform.position.y - newPosition.y) < requiredY)
        {
            //move the x
            if (Mathf.Abs(newPosition.x - player.transform.position.x) < requiredX)
            {
                if (newPosition.x - player.transform.position.x < 0)
                {
                    if (requiredX > player.transform.position.x - minX) //if the no-warp area extends past the borders
                    {
                        newPosition.x = Random.Range(player.transform.position.x + requiredX, maxX);
                    }
                    else
                    {
                        newPosition.x = Random.Range(minX, player.transform.position.x - requiredX);
                    }
                }
                if (newPosition.x - player.transform.position.x > 0)
                {
                    if (requiredX > maxX - player.transform.position.x) //if the no-warp area extends past the borders
                    {
                        newPosition.x = Random.Range(minX, player.transform.position.x - requiredX);
                    }
                    else
                    {
                        newPosition.x = Random.Range(player.transform.position.x + requiredX, maxX);
                    }
                }
            }
            //and the y
            if (Mathf.Abs(newPosition.y - player.transform.position.x) < area.GetComponent<Collider>().bounds.size.y / 4)
            {
                if (newPosition.y - player.transform.position.y < 0)
                {
                    if (area.GetComponent<Collider>().bounds.size.y / 4 > player.transform.position.y - minY)
                    {
                        newPosition.y = Random.Range(player.transform.position.y + area.GetComponent<Collider>().bounds.size.y / 4, maxY);
                    }
                    else
                    {
                        newPosition.y = Random.Range(minY, player.transform.position.y - area.GetComponent<Collider>().bounds.size.y / 4);
                    }
                }
                if (newPosition.y - player.transform.position.y > 0)
                {
                    if (area.GetComponent<Collider>().bounds.size.y / 4 > maxY - player.transform.position.x)
                    {
                        newPosition.y = Random.Range(minY, player.transform.position.y - area.GetComponent<Collider>().bounds.size.y / 4);
                    }
                    else
                    {
                        newPosition.y = Random.Range(player.transform.position.y + area.GetComponent<Collider>().bounds.size.y / 4, maxY);
                    }
                }
            }
        }
        if (newPosition.x > maxX) newPosition.x = maxX;
        if (newPosition.y > maxY) newPosition.y = maxY;
        if (newPosition.x < minX) newPosition.x = minX;
        if (newPosition.y < minY) newPosition.y = minY;
        return newPosition;
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (collision.name == "WaterWave(Clone)")
            {
                if (gameObject.transform.position.x + pushX <= maxX && gameObject.transform.position.x + pushX >= minX)
                {
                    gameObject.transform.position += new Vector3(pushX, 0);
                }
                if (gameObject.transform.position.y + pushY <= maxY && gameObject.transform.position.y + pushY >= minY)
                {
                    gameObject.transform.position += new Vector3(0, pushY);
                }
                gameObject.GetComponent<Boss_Status>().DealDamage(0.1f * Time.deltaTime);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "WaterWave(Clone)")
        {
            isPushed = false;
            pushX = 0;
            pushY = 0;
        }
     
    }

    public void Push(float xSpeed, float ySpeed, float speedMultiplyer)
    {
        isPushed = true;
        pushX = xSpeed * speedMultiplyer;
        pushY = ySpeed * speedMultiplyer;
    }
}
