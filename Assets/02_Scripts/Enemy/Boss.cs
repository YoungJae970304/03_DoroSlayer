using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : Enemy
{
    public GameObject metis;
    float shotAtkCooltime;

    private void Awake()
    {
        atk = 2;
        life = 30;
        currentTime = 2.8f;
        atkCooltime = 3f;
        shotAtkCooltime = 4f;
        speed = 0.9f;
    }

    protected override void Update()
    {
        base.Update();

        if (targets.Count > 0)
        {
            // 현재 활성화 되어있는 플레이어와 몬스터의 거리
            distance = Vector2.Distance(targets[0].transform.position, gameObject.transform.position);

            // 보스 //
            // 거리가 멀면 추적
            if ( distance > 3f )
            {
                enemyState = EnemyState.Move;
            }

            // 더 가까운 거리에 있다면 근거리 공격
            if (distance <= 1f)
            {
                enemyState = EnemyState.Attack;
            }
            // 일정 거리 내에 있다면 원거리 공격
            else if ( distance > 1f && distance <= 3f )
            {
                enemyState = EnemyState.FarAttack;
            }
        }
    }

    protected override void FarAttack()
    {
        // 보스의 원거리 공격 정의

        currentTime += Time.deltaTime;

        if (currentTime >= shotAtkCooltime)
        {
            anim.SetTrigger("doFarAttack");
            metis.transform.position = transform.position;
            metis.SetActive(true);
            currentTime = 0;
        }
    }
}
