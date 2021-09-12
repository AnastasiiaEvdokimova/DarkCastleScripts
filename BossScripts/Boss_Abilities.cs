using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Abilities : MonoBehaviour
{
    public float attackCooldown;
    protected bool isAttacking;
    protected GameObject player;
    protected float lastAttackTime;

    public void ControlBossAttack(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
}
