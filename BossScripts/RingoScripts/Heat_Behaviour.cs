using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat_Behaviour : MonoBehaviour
{
    public bool isPaused = false;
    private bool isMoving, isPlayer, wasDried;
    private float dryingRate = 0.4f;
    private GameObject creator;

    public void Set(GameObject heatingObject, bool isMoving = false, bool isPlayer = false)
    {
        dryingRate *= Time.deltaTime;
        gameObject.transform.position = heatingObject.transform.position;
        creator = heatingObject;
        if (isPlayer) {
            gameObject.GetComponent<CircleCollider2D>().radius = creator.GetComponent<CapsuleCollider2D>().bounds.size.y;
        }
        else
        {
            gameObject.GetComponent<CircleCollider2D>().radius = creator.GetComponent<BoxCollider2D>().bounds.size.y;
        }
        this.isMoving = isMoving;
        this.isPlayer = isPlayer;
    }

    private void Update()
    {
        if (creator == null)
        { Destroy(gameObject); }
        else
        {
            if (!isPlayer) { gameObject.GetComponent<CircleCollider2D>().radius = creator.GetComponent<BoxCollider2D>().bounds.size.y; }
            if (isMoving) gameObject.transform.position = creator.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale != 0)
        {
            if (collision.tag == "Trail" && !collision.GetComponent<WaterTrail_Behaviour>().IsIceWater())
            {
                collision.GetComponent<WaterTrail_Behaviour>().SetSize(new Vector3(collision.transform.localScale.x - dryingRate, collision.transform.localScale.y - dryingRate));
            }
            if (collision.name == "Iceberg(Clone)")
            {
                collision.GetComponent<Iceberg_Behaviour>().Melt();
            }
            if (collision.name == "IceShard(Clone)")
            {
                collision.GetComponent<IceShard_Behaviour>().Melt();
            }
            if (collision.name == "WaterWave(Clone)")
            {
                collision.GetComponent<WaterWave_Behaviour>().Dry();
            }
        }
    }
}
