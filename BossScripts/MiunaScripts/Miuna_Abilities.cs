using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Miuna_Abilities : Boss_Abilities
{
    public float arrowSpeed = 6f;
    private bool isShooting;
    public GameObject arrow;
    Warp_Movement bossMovement;
    int shootModeRng;

    public Wall_Behaviour[] wall = new Wall_Behaviour[4];
    private bool isWallMoved;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        isShooting = true;
        shootModeRng = 50;
        wall[0] = GameObject.Find("LeftWall").GetComponent<Wall_Behaviour>();
        wall[1] = GameObject.Find("RightWall").GetComponent<Wall_Behaviour>();
        wall[2] = GameObject.Find("UpperWall").GetComponent<Wall_Behaviour>();
        wall[3] = GameObject.Find("DownWall").GetComponent<Wall_Behaviour>();
        bossMovement = gameObject.GetComponent<Warp_Movement>();
        isWallMoved = false;
    }
    
    void Update()
    {
        if (isShooting)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                ShootArrow();
            }
        }
    }

    public void TargetShot(int type)
    {
        if (type==0 && shootModeRng < 100)
        {
            shootModeRng+=5;
        }
        else if (shootModeRng > 1)
        {
            shootModeRng-=5;
        }
        if (!isWallMoved)
        {
            float minPlayerDistance;
            byte closestWall;
            if (player.transform.position.x - wall[0].GetDefaultPosition() < wall[1].GetDefaultPosition() - player.transform.position.x)
            {
                minPlayerDistance = player.transform.position.x - wall[0].GetDefaultPosition();
                closestWall = 0;
            }
            else
            {
                minPlayerDistance = wall[1].GetDefaultPosition() - player.transform.position.x;
                closestWall = 1;
            }
            for (byte i = 2; i < 4; i++)
            {
             if (Mathf.Abs(player.transform.position.y - wall[i].GetDefaultPosition()) < minPlayerDistance)
                    {
                    minPlayerDistance = Mathf.Abs(player.transform.position.y - wall[i].GetDefaultPosition());
                    closestWall = i;
                   }
            }
            Debug.Log("Closest Wall = " + closestWall);
            isWallMoved = true;
            wall[closestWall].Push_Wall();
            bossMovement.WarpFromWall(closestWall);
        }
    }

    public void WallStopped(int wallType)
    {
        isWallMoved = false;
        bossMovement.WallsBackToNormal(wallType);
    }

    public void ImmediateShot()
    {
        lastAttackTime -= attackCooldown;
    }

    private void ShootArrow()
    {
        lastAttackTime = Time.time;
        GameObject newArrow = Instantiate(arrow);
        float xSpeed, ySpeed;
        //switch between the 2 shooting patterns
        int rnd = (int) Random.Range(1, 100);
        if (rnd <= shootModeRng) //direct shot
        {
            GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, arrowSpeed, out xSpeed, out ySpeed);
            newArrow.GetComponent<LightArrow_Behaviour>().SetType(0);
        }
        else
        {
            float currentPlayerSpeedX, currentPlayerSpeedY;
            player.GetComponent<Player_Movement>().GetSpeed(out currentPlayerSpeedX, out currentPlayerSpeedY);
            if (currentPlayerSpeedX != 0 && currentPlayerSpeedY != 0)
            {
                GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, arrowSpeed, out xSpeed, out ySpeed);
                float arrowTime = (player.transform.position.x + currentPlayerSpeedX - gameObject.transform.position.x) / xSpeed;
                GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position + new Vector3(currentPlayerSpeedX * Random.Range(arrowTime / 5, arrowTime / 3), currentPlayerSpeedY * Random.Range(arrowTime / 5, arrowTime / 3)), arrowSpeed, out xSpeed, out ySpeed);
                newArrow.GetComponent<LightArrow_Behaviour>().SetType(1);
            }
            else
            {
                GeneralMethods.CalculateSpeed(gameObject.transform.position, player.transform.position, arrowSpeed, out xSpeed, out ySpeed);
                newArrow.GetComponent<LightArrow_Behaviour>().SetType(0);
            }
        }
        newArrow.GetComponent<LightArrow_Behaviour>().Shoot(gameObject.transform.position, xSpeed, ySpeed);
    }
}
