using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Behaviour : MonoBehaviour
{
    private float baseSpeed = 15f, activationTime = 1f;
    private float speed = 0;
    private bool isMoved, isPlayerPushed = false;
    private byte type; //0 = left, 1 = right, 2 = up, 3 = down
    private float playerWidth, playerHeight;
    private GameObject area;
    private GameObject corner1, corner2, blackCover; //they will move for cosmetic purposes
    float stageCenter, defaultPosition, movementStart, blackCoverWidth, blackCoverHeight, wallMeasure, baseDamage = 1;
    Miuna_Abilities miuna; 
    GameObject player;
    Vector2 distance;

    private void Start()
    {
        area = GameObject.Find("Input_Area");
        switch (gameObject.name)
        {
            case "LeftWall":
                type = 0; corner1 = GameObject.Find("UpperLeftCorner"); corner2 = GameObject.Find("DownLeftCorner"); wallMeasure = GetComponent<SpriteRenderer>().bounds.size.x / 2;
                stageCenter = area.GetComponent<Collider>().bounds.center.x - wallMeasure; defaultPosition = gameObject.transform.position.x; break;
            case "RightWall":
                type = 1; corner1 = GameObject.Find("UpperRightCorner"); corner2 = GameObject.Find("DownRightCorner"); wallMeasure = GetComponent<SpriteRenderer>().bounds.size.x / 2;
                stageCenter = area.GetComponent<Collider>().bounds.center.x + wallMeasure; defaultPosition = gameObject.transform.position.x; baseSpeed = -baseSpeed; break;
            case "UpperWall":
                type = 2; corner1 = GameObject.Find("UpperRightCorner"); corner2 = GameObject.Find("UpperLeftCorner"); wallMeasure = GetComponent<SpriteRenderer>().bounds.size.y / 2;
                stageCenter = area.GetComponent<Collider>().bounds.center.y + wallMeasure; defaultPosition = gameObject.transform.position.y; baseSpeed = -baseSpeed; break;
            case "DownWall":
                type = 3; corner1 = GameObject.Find("DownRightCorner"); corner2 = GameObject.Find("DownLeftCorner"); wallMeasure = GetComponent<SpriteRenderer>().bounds.size.y / 2;
                stageCenter = area.GetComponent<Collider>().bounds.center.y - wallMeasure; defaultPosition = gameObject.transform.position.y; break;
        }
        if (GameObject.FindWithTag("Boss") != null)
        if (GameObject.FindWithTag("Boss").name == "Boss_Miuna")
        {
            baseSpeed *= Time.fixedDeltaTime;
            blackCover = GameObject.Find("blackCover");
            blackCoverWidth = blackCover.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            blackCoverHeight = blackCover.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            miuna = GameObject.Find("Boss_Miuna").GetComponent<Miuna_Abilities>();
            player = GameObject.FindWithTag("Player");
            playerWidth = player.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            playerHeight = player.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        }
    }

    public void Push_Wall()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        movementStart = Time.time + activationTime;
        isMoved = true;
        isPlayerPushed = false;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (isMoved && Time.time >= movementStart && speed == 0)
            {
                if (gameObject.transform.position.x == stageCenter || gameObject.transform.position.y == stageCenter)
                {
                    speed = -baseSpeed;
                }
                else
                {
                    speed = baseSpeed;
                }
                if (blackCover.GetComponent<SpriteRenderer>().sortingLayerName == "Default")
                {

                    blackCover.GetComponent<SpriteRenderer>().sortingLayerName = "Cover";
                }
            }
            if (speed == baseSpeed)
            {
                switch (type)
                {
                    case 0:
                        if (gameObject.transform.position.x + speed < stageCenter)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed, gameObject.transform.position.y);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.x + wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(stageCenter, gameObject.transform.position.y);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x - blackCoverWidth - wallMeasure, gameObject.transform.position.y);
                        break;
                    case 1:
                        if (gameObject.transform.position.x + speed > stageCenter)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed, gameObject.transform.position.y);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.x - wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(stageCenter, gameObject.transform.position.y);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x + blackCoverWidth + wallMeasure, gameObject.transform.position.y);
                        break;
                    case 2:
                        if (gameObject.transform.position.y + speed > stageCenter)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + speed);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.y - wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, stageCenter);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + blackCoverHeight + wallMeasure);
                        break;
                    case 3:
                        if (gameObject.transform.position.y + speed < stageCenter)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + speed);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.y + wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, stageCenter);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - blackCoverHeight - wallMeasure);
                        break;
                }
                if (type == 0 || type == 1)
                {
                    corner1.transform.position = new Vector2(gameObject.transform.position.x, corner1.transform.position.y);
                    corner2.transform.position = new Vector2(gameObject.transform.position.x, corner2.transform.position.y);
                    if (isPlayerPushed)
                    {
                        player.transform.position = new Vector2(player.transform.position.x + speed, player.transform.position.y);
                    }
                }
                else
                {
                    corner1.transform.position = new Vector2(corner1.transform.position.x, gameObject.transform.position.y);
                    corner2.transform.position = new Vector2(corner2.transform.position.x, gameObject.transform.position.y);
                    if (isPlayerPushed)
                    {
                        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + speed);

                    }
                }
                if (speed == 0)
                {
                    movementStart = Time.time + activationTime * 2; //setting the cooldown for reverse movement
                    if (isPlayerPushed)
                    {
                        switch (type)
                        {
                            case 0: player.transform.position = new Vector2(gameObject.transform.position.x + wallMeasure + playerWidth, player.transform.position.y); break;
                            case 1: player.transform.position = new Vector2(gameObject.transform.position.x - wallMeasure - playerWidth, player.transform.position.y); break;
                            case 2: player.transform.position = new Vector2(player.transform.position.x, gameObject.transform.position.y - wallMeasure - playerHeight); break;
                            case 3: player.transform.position = new Vector2(player.transform.position.x, gameObject.transform.position.y + wallMeasure + playerHeight); break;
                        }
                        player.GetComponent<Player_Movement>().Push(Mathf.Abs(baseSpeed) / Time.deltaTime, distance);
                    }
                }
            }
            //reverse movement
            if (speed == -baseSpeed)
            {
                switch (type)
                {
                    case 0:
                        if (gameObject.transform.position.x + speed > defaultPosition)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed, gameObject.transform.position.y);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.x + wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(defaultPosition, gameObject.transform.position.y);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x - blackCoverWidth - wallMeasure, gameObject.transform.position.y);
                        break;
                    case 1:
                        if (gameObject.transform.position.x + speed < defaultPosition)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed, gameObject.transform.position.y);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.x - wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(defaultPosition, gameObject.transform.position.y);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x + blackCoverWidth + wallMeasure, gameObject.transform.position.y);
                        break;
                    case 2:
                        if (gameObject.transform.position.y + speed < defaultPosition)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + speed);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.y - wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, defaultPosition);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + blackCoverHeight + wallMeasure);
                        break;
                    case 3:
                        if (gameObject.transform.position.y + speed > defaultPosition)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + speed);
                            area.GetComponent<Default_Movement_Control>().WallRestriction(type, gameObject.transform.position.y + wallMeasure);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x, defaultPosition);
                            speed = 0;
                        }
                        blackCover.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - blackCoverHeight - wallMeasure);
                        break;
                }
                if (type == 0 || type == 1)
                {
                    corner1.transform.position = new Vector2(gameObject.transform.position.x, corner1.transform.position.y);
                    corner2.transform.position = new Vector2(gameObject.transform.position.x, corner2.transform.position.y);
                }
                else
                {
                    corner1.transform.position = new Vector2(corner1.transform.position.x, gameObject.transform.position.y);
                    corner2.transform.position = new Vector2(corner2.transform.position.x, gameObject.transform.position.y);
                }
                if (speed == 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    isPlayerPushed = false;
                    isMoved = false;
                    miuna.WallStopped(type);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (speed != 0)
        {
            if (collision.tag == "Player")
            {
                float damage;
                if (!isPlayerPushed)
                {
                    distance = new Vector2(0, 0);
                    if (type == 0 || type == 1)
                    {
                        distance.x = (stageCenter - gameObject.transform.position.x) * 1.1f;
                        damage = baseDamage * distance.x;
                    }
                    else
                    {
                        distance.y = (stageCenter - gameObject.transform.position.y) * 1.1f;
                        damage = baseDamage * distance.y;
                    }
                    player.GetComponent<Player_Status>().LoseHP(baseDamage);
                    player.GetComponent<Player_Movement>().WallContact(speed, type);
                    isPlayerPushed = true;
                }
            }
        }
        else
        {
            if (collision.tag == "Player" && !isPlayerPushed)
            {
                if (collision.GetComponent<Player_Movement>().IsPushed())
                {
                    collision.GetComponent<Player_Status>().LoseHP(0.5f * baseDamage);
                }
                collision.GetComponent<Player_Movement>().StopPlayer();
                Debug.Log("fuuu" + type);
                switch (type)
                {
                    case 0: collision.transform.position = new Vector2(gameObject.transform.position.x + wallMeasure*1.1f + playerWidth, collision.transform.position.y); break;
                    case 1: collision.transform.position = new Vector2(gameObject.transform.position.x - wallMeasure * 1.1f - playerWidth, collision.transform.position.y); break;
                    case 2: collision.transform.position = new Vector2(collision.transform.position.x, gameObject.transform.position.y - wallMeasure * 1.1f - playerHeight); break;
                    case 3: collision.transform.position = new Vector2(collision.transform.position.x, gameObject.transform.position.y + wallMeasure * 1.1f + playerHeight); break;
                }
            }
        }
    }


    public void Vanish()
    {
        speed = 0;
        if (type == 0 || type == 1) {
            gameObject.transform.position = new Vector2(defaultPosition, gameObject.transform.position.y);
            corner1.transform.position = new Vector2(gameObject.transform.position.x, corner1.transform.position.y);
            corner2.transform.position = new Vector2(gameObject.transform.position.x, corner2.transform.position.y);
        }
        else
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, defaultPosition);
            corner1.transform.position = new Vector2(corner1.transform.position.x, gameObject.transform.position.y);
            corner2.transform.position = new Vector2(corner2.transform.position.x, gameObject.transform.position.y);
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        isMoved = false;
        miuna.WallStopped(type);
        float restrict = defaultPosition;
        switch (type)
        {
            case 0:
                restrict = restrict + playerWidth; break;

            case 1:
                restrict = restrict - playerWidth; break;

            case 2:
                restrict = restrict - playerHeight; break;

            case 3:
                restrict = restrict + playerHeight; break;

        }
        area.GetComponent<Default_Movement_Control>().WallRestriction(type, restrict);
        blackCover.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }

    public float GetDefaultPosition() //for selecting the wall that's closest to the shot player
    {
        return defaultPosition;
    }

    public bool IsMoved() //for projectiles that can make illusion vanish
    {
       if (type == 0 || type == 1)
        {
            return gameObject.transform.position.x != defaultPosition;
         
        }
        else
        {
            return gameObject.transform.position.y != defaultPosition;
        }
    }

    public bool isVertical()
    {
        if (type == 0 || type == 1)
        {
            return true;
        }
        else { 
        return false; }
    }
}
