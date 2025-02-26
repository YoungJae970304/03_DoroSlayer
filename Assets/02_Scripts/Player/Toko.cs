using UnityEngine;

public class Toko : Player
{
    public BoxCollider2D punchRange, ShootRange;

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
        isAttack = true;

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
            Managers.Data.PlayerGage += Managers.Data.PlayerUpGauge;

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
}
