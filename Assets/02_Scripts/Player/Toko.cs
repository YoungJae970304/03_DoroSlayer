using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Toko : Player
{
    public BoxCollider2D punchRange, ShootRange;
    List<GameObject> breakableOb = new List<GameObject>();

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

        // 공격 후 차지 시간 초기화
        chargeTime = 0f;
    }

    public void EventTokoAtkEnemy(int damage)
    {
        if (targets.Count > 0)
        {
            Managers.Data.PlayerGage += 10f;

            targets[0].GetComponent<Enemy>().Hit(damage + atk);
            //나중에 targets[0].transform.position에 터지는 이펙트 생성

            float dir = targets[0].transform.position.x - transform.position.x;
            targets[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.5f) * backForce, ForceMode2D.Impulse);
        }

        if (breakableOb.Count > 0)
        {
            Managers.Data.PlayerGage += 10f;
            breakableOb[0].SetActive(false);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.CompareTag("Breakable"))
        {
            breakableOb.Add(collision.gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag("Breakable"))
        {
            breakableOb.Remove(collision.gameObject);
        }
    }
}
