using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Toko : Player
{
    public BoxCollider2D punchRange, ShotRange;

    protected override void Attack()
    {
        speed = 0;
        if (chargeTime >= chargeAtk)
        {
            ShotRange.enabled = true;
            punchRange.enabled = false;
            CAttack();
        }
        else
        {
            ShotRange.enabled = false;
            punchRange.enabled = true;
            WeekAttack();
        }

        // 공격 후 차지 시간 초기화
        chargeTime = 0f;
    }

    public void EventTokoAtkEnemy(int damage)
    {
        if (targets.Count > 0)
        {
            targets[0].GetComponent<Enemy>().Hit(damage + atk);
            //나중에 targets[0].transform.position에 터지는 이펙트 생성
        }
    }
}
