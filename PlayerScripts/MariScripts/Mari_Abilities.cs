using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mari_Abilities : Player_Abilities
{
    Player_Status Mari_Stats;
    float waveCreationCost, waveSpeed;
    float shieldCreationCost, shieldSupportCost, shieldMovementSpeed;
    bool isShieldActivated, isWaveCreated;
    public GameObject waterWave, waterShield, waterSplash;
    GameObject shield;

    private void Start()
    {
        Mari_Stats = gameObject.GetComponent<Player_Status>();
        waveCreationCost = 50;
        shieldCreationCost = 20;
        shieldSupportCost = 20;
        shieldMovementSpeed = 1f;
        waveSpeed = 3f;
        isShieldActivated = false;
        isWaveCreated = false;
    }

    public bool IsShieldActivated()
    {
        return isShieldActivated;
    }

    public void waveDestroyed() {
        isWaveCreated = false;
    }

    public override void PrimaryAbility(Vector2 mousePos)
    {
        if (Mari_Stats.GetMP() >= waveCreationCost && !isWaveCreated)
        {
            float xSpeed, ySpeed;
            Mari_Stats.ExpendMP(waveCreationCost);
            GeneralMethods.CalculateSpeed(gameObject.transform.position, mousePos, waveSpeed, out xSpeed, out ySpeed);
            GameObject wave = Instantiate(waterWave);
            wave.GetComponent<WaterWave_Behaviour>().Shoot(gameObject.transform.position, xSpeed, ySpeed);
            isWaveCreated = true;
        }
    }


    public override void DefensiveAbility(Vector2 mousePos)
    {

        if (!isShieldActivated && Mari_Stats.GetMP() >= shieldCreationCost)
        {
            isShieldActivated = true;
            shield = Instantiate(waterShield);
            shield.GetComponent<WaterShield_Behaviour>().Cast(gameObject);
            Mari_Stats.ExpendMP(shieldCreationCost);
            if (gameObject.GetComponent<Player_Movement>().IsMoving())
            {
                gameObject.GetComponent<Player_Movement>().StopPlayer();
            }
        }
        if (isShieldActivated)
        {
            if (Mari_Stats.GetMP() > shieldSupportCost * Time.deltaTime)
            {
                float xSpeed, ySpeed;
                GeneralMethods.CalculateSpeed(gameObject.transform.position, mousePos, shieldMovementSpeed, out xSpeed, out ySpeed);
                Mari_Stats.ExpendMP(shieldSupportCost * Time.deltaTime);
                if (Mathf.Abs(mousePos.x - gameObject.transform.position.x) <= Mathf.Abs(xSpeed) && (Mathf.Abs(mousePos.y - gameObject.transform.position.y) <= Mathf.Abs(ySpeed)))
                {
                    gameObject.transform.position = new Vector2(mousePos.x, mousePos.y);
                }
                else
                {
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x + xSpeed, gameObject.transform.position.y + ySpeed);
                }
            }
            else
            {
                StopDefensiveAbility();
            }
        }

    }

    public override void StopDefensiveAbility()
    {
        Instantiate(waterSplash).GetComponent<Splash_Behaviour>().Splash(gameObject.transform.position);
        Destroy(shield);
        isShieldActivated = false;
    }

}
