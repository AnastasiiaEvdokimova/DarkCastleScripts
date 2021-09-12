using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Behaviour : MonoBehaviour
{
    public float basicDamage;
    protected float xSpeed, ySpeed;
    protected float minX, maxX, minY, maxY, spriteHeight, spriteWidth;
    protected float originalDeltaTime;

    public void Shoot(Vector2 startingPoint, float xSpeed, float ySpeed)
    {
        this.xSpeed = xSpeed;
        this.ySpeed = ySpeed;
        originalDeltaTime = Time.deltaTime;
        RotateSprite();
        SetBoundaries();
        SpriteAdjustment(startingPoint);
    }

    protected void RotateSprite()
    {
        float hypoth = Mathf.Sqrt(xSpeed * xSpeed + ySpeed * ySpeed);
        float rotation = 0;
        if (xSpeed >= 0 && ySpeed >= 0)
        {
            rotation = -Mathf.Asin(xSpeed / hypoth) * 180 / Mathf.PI;
        }
        else if (xSpeed < 0 && ySpeed >= 0)
        {
            rotation = Mathf.Acos(ySpeed / hypoth) * 180 / Mathf.PI;
        }
        else if (xSpeed < 0 && ySpeed < 0)
        {
            rotation = Mathf.Asin(Mathf.Abs(ySpeed) / hypoth) * 180 / Mathf.PI + 90;
        }
        else if (xSpeed >= 0 && ySpeed < 0)
        {
            rotation = -Mathf.Asin(Mathf.Abs(ySpeed) / hypoth) * 180 / Mathf.PI - 90;
        }
        gameObject.transform.Rotate(new Vector3(0, 0, rotation));
    }

    public void Destroy()
    {
        if (gameObject.name == "WaterWave(Clone)") { gameObject.GetComponent<WaterWave_Behaviour>().DestroyWave(); }
        else if (gameObject.name == "IceShard(Clone)") { gameObject.GetComponent<IceShard_Behaviour>().Break(); }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void SetBoundaries()
    {
        GameObject area = GameObject.Find("Input_Area");
        minX = area.GetComponent<Collider>().bounds.center.x - area.GetComponent<Collider>().bounds.size.x / 2;
        maxX = area.GetComponent<Collider>().bounds.center.x + area.GetComponent<Collider>().bounds.size.x / 2;
        minY = area.GetComponent<Collider>().bounds.center.y - area.GetComponent<Collider>().bounds.size.y / 2;
        maxY = area.GetComponent<Collider>().bounds.center.y + area.GetComponent<Collider>().bounds.size.y / 2;
    }

    protected void CheckBoundaries()
    {
        if (gameObject.transform.position.x >= maxX - spriteWidth/2 || gameObject.transform.position.y >= maxY - spriteHeight/2 || gameObject.transform.position.x <= minX + spriteWidth/2 || gameObject.transform.position.y <= minY - spriteHeight / 2)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void SpriteAdjustment(Vector2 startingPoint)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
        spriteHeight = spriteRenderer.bounds.size.y;
        gameObject.transform.position = new Vector2(startingPoint.x + spriteWidth * (xSpeed / Mathf.Abs(xSpeed)), startingPoint.y + spriteHeight * (ySpeed / Mathf.Abs(ySpeed)));
    }

    protected void MoveForward()
    {
        if (Time.timeScale != 0) {
            if (xSpeed == 0 && ySpeed == 0) //preventing some errors
            {
                Destroy(gameObject);
            }
            if (originalDeltaTime!=default)
            if (Mathf.Abs(Time.fixedDeltaTime - originalDeltaTime) != 0)
            {
                xSpeed = xSpeed / originalDeltaTime * Time.fixedDeltaTime;
                ySpeed = ySpeed / originalDeltaTime * Time.fixedDeltaTime;
                originalDeltaTime = Time.fixedDeltaTime;
            }
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed, gameObject.transform.position.y + ySpeed);
            //boundaries check
            if (gameObject.transform.position.y + ySpeed * Time.deltaTime > maxY - spriteHeight / 2 || gameObject.transform.position.y < minY + spriteHeight / 2 || gameObject.transform.position.x > maxX - spriteWidth / 2 || gameObject.transform.position.x < minX + spriteWidth / 2)
            {
                Destroy(gameObject);
            }
        }
    }


}
