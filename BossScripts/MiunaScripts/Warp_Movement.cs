using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp_Movement : Boss_Movement
{
    public int warpCooldown;
    private float warpTime;
  
    void Start()
    {
        Starting();
        warpTime = Time.time; //Boss won't warp at the start of the game
    }

    public void WarpFromWall(int wallType)
    {
      switch (wallType)
        {
            case 0: minX = area.GetComponent<Collider>().bounds.center.x + spriteWidth / 2; if (gameObject.transform.position.x < minX) Warp(); break;
            case 1: maxX = area.GetComponent<Collider>().bounds.center.x - spriteWidth / 2; if (gameObject.transform.position.x > maxX) Warp(); break;
            case 2: maxY = area.GetComponent<Collider>().bounds.center.y - spriteHeight / 2; if (gameObject.transform.position.y > maxY) Warp(); break;
            case 3: minY = area.GetComponent<Collider>().bounds.center.y + spriteHeight / 2; if (gameObject.transform.position.y < minY) Warp(); break;
        }
    }

    public void WallsBackToNormal(int wallType)
    {
        switch (wallType)
        {
            case 0: minX = area.GetComponent<Collider>().bounds.center.x - area.GetComponent<Collider>().bounds.size.x / 2 + spriteWidth / 2; break;
            case 1: maxX = area.GetComponent<Collider>().bounds.center.x + area.GetComponent<Collider>().bounds.size.x / 2 - spriteWidth / 2; break;
            case 2: maxY = area.GetComponent<Collider>().bounds.center.y + area.GetComponent<Collider>().bounds.size.y / 2 - spriteHeight / 2; break;
            case 3: minY = area.GetComponent<Collider>().bounds.center.y - area.GetComponent<Collider>().bounds.size.y / 2 + spriteHeight / 2; break;
        }
    }

     public void Warp() {
        gameObject.transform.position = ChooseMovementPoint();
        warpTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Iceberg(Clone)")
        {
            collision.GetComponent<Iceberg_Behaviour>().Melt(true);
        }
    }

    void Update()
    {
        if (Time.time - warpTime > warpCooldown)
            this.Warp();
        if (playerIsClose)
        {
            if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) >= requiredX) && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) >= requiredY))
            {
                playerIsClose = false;
            }
            else if ((Time.time - playerDetectionTime >= runAwayTime)&& ((Time.time - warpTime) >= warpCooldown / 4))
            {
                gameObject.GetComponent<Miuna_Abilities>().ImmediateShot();
                Warp();
                playerIsClose = false;
            }
        }
        else
       if ((Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < requiredX) && (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < requiredY))
        {
            if (player.GetComponent<Player_Movement>().IsPushed())
            {
                Warp();
            }
            else
            {
                playerIsClose = true;
                playerDetectionTime = Time.time;
            }
        }


    }
}
