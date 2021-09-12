using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rio_Abilities : Boss_Abilities
{
    private float attackChoise;
    public float attackSpeed = 6f;
    private float recoilDamage;
    Rio_Status rio;
    public GameObject bolt, ball;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        lastAttackTime = Time.time;
        attackChoise = 50;
        recoilDamage = 0.01f;
        rio = gameObject.GetComponent<Rio_Status>();
        isAttacking = true;
    }

    public void TakeAction()
    {
        isAttacking = true;
        lastAttackTime -= attackCooldown;
    }

    private void CastBolt()
    {
        lastAttackTime = Time.time;
        float xSpeed, ySpeed;
        float step = attackSpeed * Time.deltaTime;
        int rnd = (int)Random.Range(1, 4); //determine the amount of bolts
        GameObject[] castedBolts = new GameObject[rnd];
        GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, attackSpeed, out xSpeed, out ySpeed);
        castedBolts[0] = Instantiate(bolt);
        castedBolts[0].GetComponent<LightingBolt_Behaviour>().Shoot(gameObject.transform.position, xSpeed, ySpeed); //the first one is always aimed at player
        for (int i = 1; i < rnd; i++)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            xSpeed = step * Mathf.Cos(angle);
            ySpeed = step * Mathf.Sin(angle);
            castedBolts[i] = Instantiate(bolt);
            castedBolts[i].GetComponent<LightingBolt_Behaviour>().Shoot(gameObject.transform.position, xSpeed, ySpeed);
        }
    }

    public void BallDissapeared()
    {
        attackChoise += 15;
    }

    private void CastBall(bool isMovingClockwise)
    {   float xSpeed, ySpeed;
        lastAttackTime = Time.time;
        GameObject lightingBall = Instantiate(ball);
        float rotationSpeed = 4f;
        if (isMovingClockwise)
        {
            rotationSpeed = -rotationSpeed;
        }
        GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, attackSpeed, out xSpeed, out ySpeed);
        float radiusGrowthSpeed = Random.Range(0.2f, 0.6f);
        lightingBall.GetComponent<LightingBall_Behaviour>().Cast(gameObject.transform.position, radiusGrowthSpeed, xSpeed, ySpeed, rotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                int rnd = (int)Random.Range(0, 100);
                if (rnd >= attackChoise)
                {
                    CastBolt();
                }
                else
                {
                    if (rnd <= attackChoise / 2)
                    {
                        CastBall(true);
                    }
                    else
                    {
                        CastBall(false);
                    }
                    attackChoise -= 15;
                }
                rio.DealDamage(recoilDamage);
            }
        }
    }
}
