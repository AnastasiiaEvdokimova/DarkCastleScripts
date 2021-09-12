using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orion_Abilities : Player_Abilities
{
    private float dashingSpeed = 10f;
    private float startDashCost = 30;
    private float dashCost = 60;
    private float slashCost = 30;
    private float cooldownTime;
    private float slashSpeed = 7f;
    private bool isDashActivated;
    public GameObject windSlash, windGale;
    private GameObject galeAnimation;
    private void Start()
    {
        playerStatus = gameObject.GetComponent<Player_Status>();
        isDashActivated = false;

    }

    public override void StopDefensiveAbility()
    {
        isDashActivated = false;

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
        Destroy(galeAnimation);
    }

    public override void PrimaryAbility(Vector2 mousePos)
    {
        if (playerStatus.GetMP() >= slashCost)
        {
            float xSpeed, ySpeed;
            playerStatus.ExpendMP(slashCost);
            GeneralMethods.CalculateSpeed(gameObject.transform.position, mousePos, slashSpeed, out xSpeed, out ySpeed);
            GameObject slash = Instantiate(windSlash);
            slash.GetComponent<WindSlash_Behaviour>().Shoot(gameObject.transform.position, xSpeed, ySpeed);
        }
    }

    public override void DefensiveAbility(Vector2 mousePos) //gale dash
    {
        if (!isDashActivated && playerStatus.GetMP() >= startDashCost)
        {
            isDashActivated = true;
            playerStatus.ExpendMP(startDashCost);
            if (gameObject.GetComponent<Player_Movement>().IsMoving())
            {
                gameObject.GetComponent<Player_Movement>().StopPlayer();
            }
            galeAnimation = Instantiate(windGale);
            galeAnimation.GetComponent<WindGale_Behaviour>().Set(gameObject.transform);
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }
        if (isDashActivated)
        {
            if (playerStatus.GetMP() > dashCost * Time.deltaTime)
            {
                float xSpeed, ySpeed;
                GeneralMethods.CalculateSpeed(gameObject.transform.position, mousePos, dashingSpeed, out xSpeed, out ySpeed);
                playerStatus.ExpendMP(dashCost * Time.deltaTime);
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

    public bool isDashing(){
        return isDashActivated;
        }
}
