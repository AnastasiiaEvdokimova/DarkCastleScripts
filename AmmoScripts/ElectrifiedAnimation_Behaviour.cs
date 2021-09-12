using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrifiedAnimation_Behaviour : MonoBehaviour
{
    bool isBinded;
    GameObject boundedObject;
    public void SetPosition(Vector2 position)
    {
        gameObject.transform.position = position;
    }

    public void Scale(float sizeX, float sizeY)
    {
gameObject.transform.localScale = new Vector3(sizeX/3, sizeY/3);      
    }

    public void Bind(GameObject boundedObject)
    {
        this.boundedObject = boundedObject;
        isBinded = true;
    }

    private void Update()
    {
        if (isBinded)
        {
            gameObject.transform.position = boundedObject.transform.position;
            if (!boundedObject.GetComponent<Player_Movement>().IsSpeedAltered())
            {
                Destroy(gameObject);
            }
        }
    }
}
