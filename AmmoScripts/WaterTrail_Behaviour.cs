using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrail_Behaviour : MonoBehaviour
{
    bool isElectrified;
    float dryingRate, startingSize;
    private bool iceWater;
    public GameObject electrifiedAnimation;
    private GameObject currentAnimation, nearbyFire;
    float basicDamage = 0.5f;

    private void Start()
    {
        dryingRate = 0.05F * Time.deltaTime;
    }

    private void RotateSprite(float xSpeed, float ySpeed)
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

    public void CreatePuddle(Vector2 position, float xSpeed, float ySpeed, float size = 0, bool isIceWater = false)
    {
        gameObject.transform.position = position;
        isElectrified = false;
        RotateSprite(xSpeed, ySpeed);
        if (size != 0)
        {
            SetSize(new Vector3(size, size));
        }
        startingSize = gameObject.transform.localScale.x;
        this.iceWater = isIceWater;
    }

    public void SetSize(Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }
    
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (!iceWater)
            {
                gameObject.transform.localScale -= new Vector3(dryingRate, dryingRate);
                if (isElectrified) currentAnimation.GetComponent<ElectrifiedAnimation_Behaviour>().Scale(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                if (gameObject.transform.localScale.x <= 0)
                {
                    Destroy(gameObject);
                    if (isElectrified) Destroy(currentAnimation);
                }
            }
        }
    }

    public bool IsIceWater()
    {
        return iceWater;
    }

    public void IceMelt()
    {
        iceWater = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isElectrified)
        {
            if ((collision.name == "LightingBolt(Clone)") || (collision.tag == "Trail" && collision.GetComponent<WaterTrail_Behaviour>().IsElectrified()))
            {
                isElectrified = true;
                currentAnimation = Instantiate(electrifiedAnimation);
                currentAnimation.GetComponent<ElectrifiedAnimation_Behaviour>().SetPosition(gameObject.transform.position);
                currentAnimation.GetComponent<ElectrifiedAnimation_Behaviour>().Scale(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (collision.tag == "Player" && isElectrified)
            {
                bool isWaterShielded = false;
                if (collision.name == "Player_Mari")
                {
                    isWaterShielded = collision.GetComponent<Mari_Abilities>().IsShieldActivated();
                }
                if (!isWaterShielded)
                {
                    collision.GetComponent<Player_Status>().LoseHP(basicDamage * gameObject.transform.localScale.x / startingSize);
                    isElectrified = false;
                    Destroy(currentAnimation);
                }
                else
                {
                    isElectrified = false;
                    Destroy(currentAnimation);
                }
            }
        }
    }

    public bool IsElectrified()
    {
        return isElectrified;
    }
}
