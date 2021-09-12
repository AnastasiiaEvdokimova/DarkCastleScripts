using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lydia_Abilities : Player_Abilities
{
    public GameObject iceberg, iceShard;
    private Player_Movement lydia;
    private float maxIcebergsAmount, iceCreationCost, iceShardCost, iceShardSpeed;
    void Start()
    {
        maxIcebergsAmount = 20;
        iceCreationCost = 40;
        iceShardCost = 5;
        iceShardSpeed = 8f;
        playerStatus = gameObject.GetComponent<Player_Status>();
        lydia = gameObject.GetComponent<Player_Movement>();
    }

    public override void DefensiveAbility(Vector2 mousePos)
    {
        if (lydia.IsMoving())
        {
            lydia.StopPlayer();
        }
        if (playerStatus.GetMP() - iceCreationCost *Time.deltaTime >= 0)
        {
            if (Iceberg_Behaviour.icebergCounter <= maxIcebergsAmount)
            {
                Instantiate(iceberg).GetComponent<Iceberg_Behaviour>().Create(mousePos);
                playerStatus.ExpendMP(iceCreationCost * Time.deltaTime);
            }
        }
        else
        {
            StopDefensiveAbility();
        }

    }

    public override void StopDefensiveAbility()
    {
        EventManager.StopIceSupport();
    }

    public override void PrimaryAbility(Vector2 mousePos)
    {
       if (playerStatus.GetMP() - iceShardCost >= 0)
        {
            float speedX, speedY;
            GeneralMethods.CalculateSpeed(gameObject.transform.position, mousePos, iceShardSpeed, out speedX, out speedY);
            Instantiate(iceShard).GetComponent<IceShard_Behaviour>().Shoot(gameObject.transform.position, speedX, speedY);
            playerStatus.ExpendMP(iceShardCost);
        }
    }
}
