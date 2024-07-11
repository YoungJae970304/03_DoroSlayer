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
            // ���� Ȱ��ȭ �Ǿ��ִ� �÷��̾�� ������ �Ÿ�
            distance = Vector2.Distance(targets[0].transform.position, gameObject.transform.position);

            // ��� //
            // ���� �Ÿ� ���� �ִٸ� ���� (Move)
            if (distance < 1.5f)
            {
                enemyState = EnemyState.Move;
            }
            // �ٰŸ��� �ִٸ� ����
            if (distance <= 0.5f)
            {
                enemyState = EnemyState.Attack;
            }
        }
    }
}
