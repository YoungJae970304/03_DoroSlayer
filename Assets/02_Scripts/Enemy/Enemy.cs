using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    FarAttack,
    Dead
}

public class Enemy : MonoBehaviour
{
    // 몬스터 스탯 변수
    protected int atk = 1;
    protected int life = 5;
    protected float currentTime = 1.8f;
    protected float atkCooltime = 2f;
    protected float speed = 0.75f;

    // 플레이어 정보
    protected float distance;
    protected Vector2 dir;
    protected List<GameObject> targets = new List<GameObject>();

    // 몬스터 상태
    protected EnemyState enemyState;

    // 컴포넌트
    protected Rigidbody2D rig2d;
    protected Animator anim;
    protected SpriteRenderer sr;

    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        enemyState = EnemyState.Idle;
    }

    protected virtual void Update()
    {
        if (targets.Count > 0)
        {
            dir = (targets[0].transform.position - transform.position).normalized;
            sr.flipX = dir.x > 0;
        }
        
        // 기본 상태
        enemyState = EnemyState.Idle;
        
        if (targets.Count > 0)
        {
            // 현재 활성화 되어있는 플레이어와 몬스터의 거리
            distance = Vector2.Distance(targets[0].transform.position, gameObject.transform.position);

            // 잡몹 //
            // 일정 거리 내에 있다면 추적 (Move)
            if (distance < 1.5f)
            {
                enemyState = EnemyState.Move;
            }
            // 근거리에 있다면 공격
            if (distance <= 0.5f)
            {
                enemyState = EnemyState.Attack;
            }
        }
    }

    void LateUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.FarAttack:
                FarAttack();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    void Idle()
    {
        Debug.Log("Idle");
        anim.SetTrigger("doIdle");
    }

    void Move()
    {
        Debug.Log("Move");

        anim.SetTrigger("doRun");

        rig2d.velocity = new Vector2(dir.x * speed, 0);
    }

    // 데미지 적용은 공격 모션에 이벤트로 처리
    protected virtual void Attack()
    {
        rig2d.velocity = Vector2.zero;

        currentTime += Time.deltaTime;
        if (currentTime >= atkCooltime)
        {
            anim.SetTrigger("doAttack");
            currentTime = 0;
        }
    }

    protected virtual void FarAttack()
    {
        // boss에서 재정의
    }

    public void Hit(int damage)
    {
        anim.SetTrigger("doHit");
        life -= damage;

        if ( life <= 0)
        {
            enemyState = EnemyState.Dead;
        }
    }

    void Dead()
    {
        anim.SetTrigger("doDead");
        rig2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    // 애니메이션 이벤트로 넣어줄 함수들
    public void EventSetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void EventAtkPlayer()
    {
        if (targets.Count > 0)
        {
            targets[0].GetComponent<Player>().TakeDamage(atk);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            targets.Add( collision.gameObject );
        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targets.Remove( collision.gameObject );
        }
    }
    
}
