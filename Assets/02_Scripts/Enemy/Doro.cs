using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doro : Enemy
{
    private void Awake()
    {
        atkCooltime = 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (targets.Count > 0)
        {
            // 현재 활성화 되어있는 플레이어와 몬스터의 거리
            distance = Vector2.Distance(targets[0].transform.position, gameObject.transform.position);

            // 잡몹 //
            // 일정 거리 내에 있다면 추적 (Move)
            if (distance > 1f)
            {
                enemyState = EnemyState.Move;
            }
            // 근거리에 있다면 공격
            else if (distance <= 1f)
            {
                enemyState = EnemyState.Attack;
            }
        }
    }

    protected override void Dead()
    {
        Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        Managers.Data.doros.Remove(this.gameObject);
        Managers.Data.doros.Add(this.gameObject);
        base.Dead();
    }
}
