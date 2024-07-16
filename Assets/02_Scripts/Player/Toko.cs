using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Toko : Player
{
    public BoxCollider2D punchRange, ShootRange;
    //List<GameObject> breakableOb = new List<GameObject>();

    public ParticleSystem boomP;

    protected override void StartCharge()
    {
        if (chargeAtkCheck)
        {
            chargeTime += Time.deltaTime;

            if (chargeTime >= chargeAtk && chargeAtkCheck)
            {
                Instantiate(chargeP, transform);
                chargeAtkCheck = false;
            }
        }
    }

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

            float dir = targets[0].transform.position.x - transform.position.x;
            targets[0].GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.5f) * backForce, ForceMode2D.Impulse);

            targets[0].GetComponent<InteractiveOb>().Hit(damage + atk);
        }
    }

    public void EventTokoRangeAtkEnemy(int damage)
    {
        if (targets.Count > 0)
        {
            Instantiate(boomP, targets[0].transform.position, Quaternion.identity);
        }
        EventTokoAtkEnemy(damage);
    }

    /*
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Breakable"))
        {
            targets.Add(collision.gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.CompareTag("Breakable"))
        {
            targets.Remove(collision.gameObject);
        }
    }
    */
}
