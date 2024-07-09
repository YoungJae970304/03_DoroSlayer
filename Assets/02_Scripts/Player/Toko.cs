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

        // ���� �� ���� �ð� �ʱ�ȭ
        chargeTime = 0f;
    }

    public void EventTokoAtkEnemy(int damage)
    {
        if (targets.Count > 0)
        {
            targets[0].GetComponent<Enemy>().Hit(damage + atk);
            //���߿� targets[0].transform.position�� ������ ����Ʈ ����
        }
    }
}
