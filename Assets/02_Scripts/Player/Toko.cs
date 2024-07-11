using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Toko : Player
{
    public BoxCollider2D punchRange, ShootRange;

    protected override void Attack()
    {
        speed = 0;
        if (chargeTime >= chargeAtk)
        {
            ShootRange.enabled = true;
            punchRange.enabled = false;
            CAttack();
        }
        else
        {
            ShootRange.enabled = false;
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
            Managers.Data.PlayerGage += 10f;

            targets[0].GetComponent<Enemy>().Hit(damage + atk);
            //���߿� targets[0].transform.position�� ������ ����Ʈ ����

            float dir = targets[0].transform.position.x - transform.position.x;
            targets[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.5f) * backForce, ForceMode2D.Impulse);
        }
    }
}
