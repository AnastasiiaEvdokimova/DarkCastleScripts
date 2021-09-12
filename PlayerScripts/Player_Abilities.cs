using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_Abilities : MonoBehaviour
{
    protected Player_Status playerStatus;
    public abstract void PrimaryAbility(Vector2 mousePos);
    public abstract void DefensiveAbility(Vector2 mousePos);
    public abstract void StopDefensiveAbility();
}
