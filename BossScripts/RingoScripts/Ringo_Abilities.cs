using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ringo_Abilities : Boss_Abilities
{
    public float attackSpeed = 6f;
    public GameObject fire, wisp, heat;
    private float fireAppearingRadius;
    private float attackChoise;
    private float recoilDamage;
    Ringo_Status ringo;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        lastAttackTime = Time.time;
        attackChoise = 50;
        recoilDamage = 0.01f;
        ringo = gameObject.GetComponent<Ringo_Status>();
        isAttacking = true;
        Instantiate(heat).GetComponent<Heat_Behaviour>().Set(gameObject, true, true);
        fireAppearingRadius = gameObject.GetComponent<SpriteRenderer>().bounds.size.y*1.3f;
    }

    private void CastFire()
    {
        GameObject firstFire = Instantiate(fire);
        float xSpeed, ySpeed;
        int fireMight = (int)Random.Range(5, 10);
        GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, firstFire.GetComponent<BoxCollider2D>().bounds.size.x, out xSpeed, out ySpeed);
        xSpeed /= Time.deltaTime;
        ySpeed /= Time.deltaTime;
        if (ySpeed < 0)
        {
            firstFire.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
        else
        {
            firstFire.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        firstFire.GetComponent<Fire_Behaviour>().SetFlame(gameObject.transform.position + new Vector3((Mathf.Abs(xSpeed) < Mathf.Abs(gameObject.transform.position.x - gameObject.GetComponent<SpriteRenderer>().bounds.size.y/3))? xSpeed:  gameObject.GetComponent<SpriteRenderer>().bounds.size.y/3*xSpeed/Mathf.Abs(xSpeed), (Mathf.Abs(ySpeed) < Mathf.Abs(gameObject.transform.position.y - gameObject.GetComponent<SpriteRenderer>().bounds.size.y/3))? ySpeed : gameObject.GetComponent<SpriteRenderer>().bounds.size.y/3 * ySpeed/Mathf.Abs(ySpeed)), 0.5f, fireMight, xSpeed, ySpeed);

    }

    private void CastWillOWisp(bool traveling = false, float time = 0, float radius = 0)
    {
        float wispSpeed = Random.Range(2, 3);
        if (!traveling)
        {
            float wispFollowingTime = Random.Range(3, 5);
            Instantiate(wisp).GetComponent<WillOWisp_Behaviour>().Cast(gameObject.transform.position, wispSpeed, wispFollowingTime);
        }
        else
        {
            Instantiate(wisp).GetComponent<WillOWisp_Behaviour>().Cast(gameObject.transform.position, wispSpeed, time, true, radius);
        }
    }

    public void LeaveFireBehind()
    {
        CastFire();
    }

    public void CastTravelFire(float time, float radius)
    {
        CastWillOWisp(true, time, radius);
    }

    void Update()
    {
        if (isAttacking)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                int rnd = (int)Random.Range(0, 100);
                if (rnd >= attackChoise)
                {
                    if (Fire_Behaviour.fireAmount < 20)
                    CastFire();
                    else
                        CastWillOWisp();
                }
                else
                {
                    CastWillOWisp();
                }
                ringo.DealDamage(recoilDamage);
                lastAttackTime = Time.time;
            }
        }
    }
}
