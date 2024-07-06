using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Unitychan,
    Toko,
}

public class Player : MonoBehaviour
{
    // 캐릭터 기본 변수
    [SerializeField]
    protected float maxHP = 100;
    protected float CurrentHP { get; set; }

    // 이동 관련 변수
    protected float speed = 1f;     // 기본 이동속도
    protected float jumpForce = 4f; // 점프력
    protected float dashForce = 2f; // 대시 속도
    protected float moveInput;      // 좌우 이동 입력받는 변수

    // 공격 관련 변수
    protected float chargeTime = 0f;    // 차지하는 시간
    protected float chargeAtk = 0.5f;   // 기본 공격과 강공격의 경계
    protected float fullChargeAtk = 1f; // 강공격과 특수공격의 경계

    // 상태 관련 변수
    protected bool isGrounded = true;   // 땅에 닿았는지 여부 ( 점프가능 여부 )
    protected bool canDash = true;      // 대시할 수 있는지 여부 ( 대시 쿨타임 관련 )
    protected bool isDead = false;

    // 캐릭터 태그 관련 변수
    KeyCode[] numKey = {KeyCode.Alpha1, KeyCode.Alpha2 };
    public GameObject[] player;
    protected static Vector2 lastPos;

    // 컴포넌트
    protected Rigidbody2D rig2d;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected BoxCollider2D boxCol;

    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        isGrounded = true;
        canDash = true;
    }

    private void Update()
    {
        HandleInput();
        ChangeInputKey();
    }

    protected void ChangeInputKey()
    {
        lastPos = transform.position;

        for (int i = 0; i < numKey.Length; i++)
        {
            if (Input.GetKeyDown(numKey[i]))
            {
                // 인덱스 i를 열거형 타입으로 변환해주고 player 변수에 대입
                PlayerClass player = (PlayerClass)i;
                ChangePlayer(player);
            }
        }
    }

    void ChangePlayer(PlayerClass selectedPlayer)
    {
        // 모든 캐릭터를 비활성화하고 선택된 캐릭터만 활성화
        for(int i = 0;i < player.Length; i++)
        {
            player[i].gameObject.SetActive(i == (int) selectedPlayer);

            // 선택된 캐릭터는 마지막 위치로 이동
            if (i == (int)selectedPlayer)
            {
                player[i].transform.position = lastPos;
            }
        }
    }

    protected void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if ( !isDead )
        {
            // 우클릭을 눌렀고 대시가 가능하면 대시
            if (Input.GetMouseButtonDown(1) && canDash)
            {
                Dash();
            }

            // 좌우 이동 입력이 있다면 이동 / 없다면 대기
            if (moveInput != 0)
            {
                Move();
            }
            else
            {
                Idle();
            }

            // 땅에 있고 S키를 누르면 숙이기
            if (Input.GetKey(KeyCode.S) && isGrounded)
            {
                Crouch();
            }
            // S를 떼면 원래 상태로
            else if (Input.GetKeyUp(KeyCode.S) && isGrounded)
            {
                anim.SetBool("doCrouch", false);
                speed = 1f;
            }

            // 땅에있고 Space 누르면 점프
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            // 마우스 왼쪽을 누르고 있으면 차지 시작
            if (Input.GetMouseButton(0))
            {
                StartCharge();
            }
            // 마우스 왼쪽을 떼면 차지에 따른 공격
            if (Input.GetMouseButtonUp(0))
            {
                Attack();
            }

            anim.SetFloat("jumpVel", rig2d.velocity.y);
            anim.SetFloat("doJump", Mathf.Abs(rig2d.velocity.y));
        }

        if (CurrentHP > 0)
        {
            anim.SetBool("doDead", false);
        }
    }

    // 이동
    void Move()
    {
        anim.SetFloat("doRun", Mathf.Abs(moveInput));
        transform.Translate(Vector2.right * moveInput * speed * Time.deltaTime);
        sr.flipX = moveInput < 0;
    }

    // 대기
    void Idle()
    {
        anim.SetFloat("doRun", Mathf.Abs(moveInput));
        Debug.Log("대기");
        boxCol.enabled = true;
    }

    // 숙이기
    void Crouch()
    {
        anim.SetBool("doCrouch", true);
        speed = 0f;
        Debug.Log("앉기");
        boxCol.enabled = false;
    }

    // 점프
    void Jump()
    {
        isGrounded = false;
        rig2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    // 대시
    void Dash()
    {
        canDash = false;

        anim.SetTrigger("doDash");

        if (!sr.flipX)
        {
            //rig2d.AddForce(new Vector2(dashForce, 0), ForceMode2D.Impulse);
            rig2d.velocity = new Vector2(dashForce, rig2d.velocity.y);
        }
        else
        {
            //rig2d.AddForce(new Vector2(-dashForce, 0), ForceMode2D.Impulse);
            rig2d.velocity = new Vector2(-dashForce, rig2d.velocity.y);
        }
        StartCoroutine(DashCoolDownCo());
    }
    IEnumerator DashCoolDownCo()
    {
        yield return new WaitForSeconds(1f);
        canDash = true;
    }

    // 차지 시작, 차지 시간 증가
    void StartCharge()
    {
        chargeTime += Time.deltaTime;
    }

    // 차지 시간에 따른 공격
    protected virtual void Attack()
    {
        speed = 0;
        if (chargeTime >= fullChargeAtk)
        {
            FullCAttack();
        }
        else if (chargeTime >= chargeAtk)
        {
            CAttack();
        }
        else
        {
            WeekAttack();
        }

        // 공격 후 차지 시간 초기화
        chargeTime = 0f;
    }

    // 차지 없이 기본 공격
    protected void WeekAttack()
    {
        anim.SetTrigger("doWeekAttack");
        Debug.Log("기본 공격");
    }

    // 약간 차지한 공격
    protected void CAttack()
    {
        anim.SetTrigger("doChargeAttack");
        Debug.Log("차지 공격");
    }

    // 최대 차지 공격
    protected void FullCAttack()
    {
        anim.SetTrigger("doFullCAttack");
        Debug.Log("풀차지 공격");
    }

    // 자신의 현재 체력에서 적의 공격력 만큼 감소
    public void TakeDamage(float damage)
    {
        anim.SetTrigger("doHit");
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Dead();
        }
    }

    protected void Dead()
    {
        CurrentHP = 0f;
        isDead = true;
        anim.SetBool("doDead", isDead);
        anim.SetTrigger("isDead");
        Debug.Log("죽음");
    }

    // 공격 애니메이션 마지막에 이벤트로 넣어줄 함수 ( 속도를 1로 만들어 다시 움직이게 )
    public void EventSetMoveSpd()
    {
        speed = 1f;
    }
    public void EventSetAlive()
    {
        isDead = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Item"))
        {
            speed = 0;
            anim.SetTrigger("doGet");
            collision.gameObject.SetActive(false);
            Debug.Log("아이템 획득");
        }
    }
}
