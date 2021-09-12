using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDragging : MonoBehaviour
{
    GameObject[] players;
    GameObject currentlyMoving;
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        }

    private Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return (Vector2)mousePos;
    }

    private void OnMouseDown()
    {
        Vector2 mousePos = GetMousePos();
            for (int i = 0; i < players.Length; i++)
            {
                if ((mousePos.x > players[i].transform.position.x - players[i].GetComponent<SpriteRenderer>().bounds.size.x / 2) && (mousePos.x < players[i].transform.position.x + players[i].GetComponent<SpriteRenderer>().bounds.size.x / 2) &&
               (mousePos.y > players[i].transform.position.y - players[i].GetComponent<SpriteRenderer>().bounds.size.y / 2) && (mousePos.y < players[i].transform.position.y + players[i].GetComponent<SpriteRenderer>().bounds.size.y / 2) &&  (!players[i].GetComponent<PlayerIcon_Behaviour>().IsLocked()))
                {
                    currentlyMoving = players[i];
                    currentlyMoving.transform.position = mousePos;
                    currentlyMoving.GetComponent<PlayerIcon_Behaviour>().Drag(true);
                    break;
                }
            }
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = GetMousePos();
        if (currentlyMoving != null)
        {
            currentlyMoving.transform.position = mousePos;
        }
    }

    private void OnMouseUp()
    {
        if (currentlyMoving != null)
        {
            currentlyMoving.GetComponent<PlayerIcon_Behaviour>().Drag(false);
            currentlyMoving = null;
        }
    }
}
