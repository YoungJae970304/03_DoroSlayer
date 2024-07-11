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
            // ���� Ȱ��ȭ �Ǿ��ִ� �÷��̾�� ������ �Ÿ�
            distance = Vector2.Distance(targets[0].transform.position, gameObject.transform.position);

            // ���� //
            // �Ÿ��� �ָ� ����
            if ( distance > 3f )
            {
                enemyState = EnemyState.Move;
            }

            // �� ����� �Ÿ��� �ִٸ� �ٰŸ� ����
            if (distance <= 1f)
            {
                enemyState = EnemyState.Attack;
            }
            // ���� �Ÿ� ���� �ִٸ� ���Ÿ� ����
            else if ( distance > 1f && distance <= 3f )
            {
                enemyState = EnemyState.FarAttack;
            }
        }
    }

    protected override void FarAttack()
    {
        // ������ ���Ÿ� ���� ����

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
