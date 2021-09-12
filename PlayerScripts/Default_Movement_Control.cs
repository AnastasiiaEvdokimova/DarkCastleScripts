using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Movement_Control : MonoBehaviour
{
    private GameObject player;
    float spriteHeight, spriteWidth, minX, maxX, minY, maxY;
    float doubleTapTime = 0.18f, lastTapTime, dragDetectionTime = 0.15f, mouseDownTime;
    bool isDoubleTap = false, isSingleTap = false, isMouseDrag = false, commandInputted = false;
    Vector2 mousePos;

    private void Start()
    {
        //getting the field borders to prevent going off-stage (depends on input area)
        player = GameObject.FindWithTag("Player");
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
        spriteHeight = spriteRenderer.bounds.size.y;
        minX = gameObject.GetComponent<Collider>().bounds.center.x - gameObject.GetComponent<Collider>().bounds.size.x / 2;
        maxX = gameObject.GetComponent<Collider>().bounds.center.x + gameObject.GetComponent<Collider>().bounds.size.x / 2;
        minY = gameObject.GetComponent<Collider>().bounds.center.y - gameObject.GetComponent<Collider>().bounds.size.y / 2;
        maxY = gameObject.GetComponent<Collider>().bounds.center.y + gameObject.GetComponent<Collider>().bounds.size.y / 2;
    }


    public void WallRestriction(byte type, float newBoundary)
    {
        switch (type)
        {
            case 0: minX = newBoundary + spriteWidth / 2; break;
            case 1: maxX = newBoundary - spriteWidth / 2; break;
            case 2: maxY = newBoundary - spriteHeight / 2; break;
            case 3: minY = newBoundary + spriteHeight / 2; break;
        }
    }


    private void OnMouseDown() {
        mouseDownTime = Time.time;
        commandInputted = true;
        mousePos = GetMousePos();
    }

    private void OnMouseUp()
    {
        if (Time.time - mouseDownTime <= dragDetectionTime)
        {
            if (Time.time - lastTapTime < doubleTapTime)
            {
                isSingleTap = false;
                isDoubleTap = true;
            }
            else
            {
                lastTapTime = Time.time;
                isSingleTap = true;
            }
        }
       if (isMouseDrag)
        {
            isMouseDrag = false;
            player.GetComponent<Player_Abilities>().StopDefensiveAbility();
        }
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if ((Time.time - mouseDownTime > dragDetectionTime) && (commandInputted || isMouseDrag))
            {
                if (isSingleTap)
                {
                    SingleTap(mousePos);
                    isSingleTap = false;
                }
                else if (isDoubleTap)
                {
                    DoubleTap(mousePos);
                    isDoubleTap = false;
                }
                else
                {
                    isMouseDrag = true;
                    player.GetComponent<Player_Abilities>().DefensiveAbility(GetMousePos());
                }
                commandInputted = false;
            }
        }
    }

    private Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //check the boundaries with sprite measures
        if (mousePos.y > maxY - spriteHeight / 2)
        {
            mousePos.y = maxY - spriteHeight / 2;
        }
        else if (mousePos.y < minY + spriteHeight / 2)
        {
            mousePos.y = minY + spriteHeight / 2;
        }
        if (mousePos.x > maxX - spriteWidth / 2)
        {
            mousePos.x = maxX - spriteWidth / 2;
        }
        else if (mousePos.x < minX + spriteWidth / 2)
        {
            mousePos.x = minX + spriteWidth / 2;
        }
        return (Vector2)mousePos;
    }

    private void SingleTap(Vector2 mousePos)
    {
        player.GetComponent<Player_Movement>().SetPoint(mousePos.x, mousePos.y);
    }

    private void DoubleTap (Vector2 mousePos)
    {
            player.GetComponent<Player_Abilities>().PrimaryAbility(mousePos);
    }

  

}
