using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Toko : Player
{
    private void Start()
    {
        maxHP = 200;
        CurrentHP = maxHP;
    }

    protected override void Attack()
    {
        speed = 0;
        if (chargeTime >= chargeAtk)
        {
            CAttack();
        }
        else
        {
            WeekAttack();
        }

        // ���� �� ���� �ð� �ʱ�ȭ
        chargeTime = 0f;
    }
}
