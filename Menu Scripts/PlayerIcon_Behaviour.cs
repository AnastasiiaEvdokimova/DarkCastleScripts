using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon_Behaviour : MonoBehaviour
{
    Vector2 defaultPosition, lastFixPosition;
    bool isDragged = false, isLocked = false;
    bool wasDragged = false;
    Vector4[] doorsXY;
    GameObject[] doors;
    void Awake()
    {
        defaultPosition = new Vector2(gameObject.transform.position.x, -3.07f);
        isDragged = false;
        doors = GameObject.FindGameObjectsWithTag("Boss");
        doorsXY = new Vector4[doors.Length];
        for (int i = 0; i<doors.Length; i++)
        {
            SpriteRenderer doorSprite = doors[i].GetComponent<SpriteRenderer>();
            float maxX = doorSprite.transform.position.x + doorSprite.bounds.size.x / 2;
            float minX = doorSprite.transform.position.x - doorSprite.bounds.size.x / 2;
            float maxY = doorSprite.transform.position.y + doorSprite.bounds.size.y / 2;
            float minY = doorSprite.transform.position.y - doorSprite.bounds.size.y / 2;
            doorsXY[i] = new Vector4(minX, maxX, minY, maxY);
        }
        lastFixPosition = defaultPosition;
    }

    private void Update()
    {
        if (!isDragged && wasDragged && gameObject.transform.position != (Vector3)lastFixPosition)
        {
            for (int i = 0; i<doorsXY.Length; i++)
            {
                if (gameObject.transform.position.x > doorsXY[i].x && gameObject.transform.position.x < doorsXY[i].y
                    && gameObject.transform.position.y > doorsXY[i].z && gameObject.transform.position.y < doorsXY[i].w && doors[i].GetComponent<DoorFix>().FixedCharacter() != gameObject.name)
                {
                    doors[i].GetComponent<DoorFix>().FixCharacter(gameObject);
                    lastFixPosition = gameObject.transform.position;
                }
            }
        }
    }


    public void ReturnToDefault()
    {
        gameObject.transform.position = defaultPosition;
        isLocked = false;
    }

    public void Drag(bool isDragged)
    {
        if (!isLocked)
        {
            this.isDragged = isDragged;
            wasDragged = true;
        }
    }

    public void Lock(bool isLocked)
    {
        this.isLocked = isLocked;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public bool IsDragged()
    {
        return isDragged;
    }


}
