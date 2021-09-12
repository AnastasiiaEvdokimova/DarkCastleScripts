using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFix : MonoBehaviour
{
    GameObject[] players;
    GameObject currentFix = null;
    CharacterMenuScript  menu;
    Vector2 fixedIconPosition;

    private void Start()
    {
        menu = GameObject.Find("CharacterMenu").GetComponent<CharacterMenuScript>();
        float playerHeight = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>().bounds.size.y;
        fixedIconPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - gameObject.GetComponent<CapsuleCollider2D>().bounds.size.y/2 + playerHeight/2);
        if (!GeneralMethods.isStoryMode)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }

    public void CallStart()
    {
        Start();
    }

    public string FixedCharacter()
    {
        if (currentFix != null)
            return currentFix.name;
        else
            return null;
    }

    public void FixCharacter(GameObject character)
    {
         if (currentFix != null && currentFix!=character)
        {
            currentFix.GetComponent<PlayerIcon_Behaviour>().ReturnToDefault();
            menu.CharacterFixed(false);
        }
        if (character != null)
        {
            currentFix = character;
            currentFix.transform.position = fixedIconPosition;
            menu.CharacterFixed(true, gameObject.name);
            if (!GeneralMethods.isStoryMode)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] != currentFix)
                    {
                        players[i].GetComponent<PlayerIcon_Behaviour>().ReturnToDefault();
                    }
                }
            }
        }
        else
        {
            currentFix = null;
        }
    }

private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentFix && collision.GetComponent<PlayerIcon_Behaviour>().IsDragged())
        {
            currentFix = null;
            menu.CharacterFixed(false);
        }
    } 
}
