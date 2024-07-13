using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerClass
{
    Unitychan,
    Toko,
}

public class Player : MonoBehaviour
{
    // 이동 관련 변수
    protected float speed = 2f;     // 기본 이동속도
    protected float jumpForceY = 5f; // 점프력
    protected float jumpForceX = 0;
    protected float dashForce = 3f; // 대시 속도
    protected float moveInput;      // 좌우 이동 입력받는 변수

    // 공격 관련 변수
    protected int atk = 1;
    protected float chargeTime = 0f;    // 차지하는 시간
    protected float chargeAtk = 0.5f;   // 기본 공격과 강공격의 경계
    protected float fullChargeAtk = 1f; // 강공격과 특수공격의 경계
    protected float backForce = 3f;

    // 상태 관련 변수
    protected bool isGrounded = true;   // 땅에 닿았는지 여부 ( 점프가능 여부 )
    protected bool canDash = true;      // 대시할 수 있는지 여부 ( 대시 쿨타임 관련 )
    protected bool isDead = false;      // 현재 생존 상태

    // 캐릭터 태그 관련 변수
    List<KeyCode> numKey = new List<KeyCode>();
    public List<GameObject> player = new List<GameObject>();
    protected static Vector2 lastPos;

    // 적 관련 변수
    protected List<GameObject> targets = new List<GameObject>();

    // 컴포넌트
    protected Rigidbody2D rig2d;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected BoxCollider2D boxCol;

    protected void Start()
    {
        numKey.Add(KeyCode.Alpha1);
        numKey.Add(KeyCode.Alpha2);
    }

    // 오브젝트 활성화 시 초기화 해주는 작업
    private void OnEnable()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

        isGrounded = true;
        canDash = true;
        speed = 2f;
    }

    private void Update()
    {
        Debug.Log(Managers.Data.PlayerLife);
        Debug.Log(Managers.Data.PlayerGage);
        HandleInput();      // 키 입력에 대한 부분을 담당하는 함수
        ChangeInputKey();   // 캐릭터 태그 함수

        // 재시작 코드
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }

    protected void ChangeInputKey()
    {
        // 게이지가 일정 이상일때만 동작되도록 설정
        if ( !isDead && Managers.Data.PlayerGage >= 30f )
        {
            lastPos = transform.position + new Vector3(0, 0.3f);

            for (int i = 0; i < numKey.Count; i++)
            {
                if (Input.GetKeyDown(numKey[i]))
                {
                    // 인덱스 i를 열거형 타입으로 변환해주고 player 변수에 대입
                    PlayerClass playerClass = (PlayerClass)i;
                    ChangePlayer(playerClass);

                    Managers.Data.PlayerGage -= 30f;
                }
            }
        }
    }

    void ChangePlayer(PlayerClass selectedPlayer)
    {
        // 모든 캐릭터를 비활성화하고 선택된 캐릭터만 활성화
        for(int i = 0;i < player.Count; i++)
        {
            // 선택된 캐릭터는 마지막 위치로 이동
            if (i == (int)selectedPlayer)
            {
                player[i].gameObject.SetActive(true);
                player[i].transform.position = lastPos;
            }
            else
            {
                player[i].gameObject.SetActive(false);
            }
        }
    }

    // 키 입력 함수
    protected void HandleInput()
    {
        // 좌우 키 입력을 moveInput에 대입
        moveInput = Input.GetAxisRaw("Horizontal");

        // 캐릭터가 죽지 않은 상태라면 즉, 살아있을 때만 키 입력 발생
        if ( !isDead )
        {
            // 우클릭을 눌렀고 대쉬가 가능하면 대쉬
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
                speed = 2f;
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

            // R키를 누르면 라이프 회복
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(LifeUp());
            }

            // 속도y의 절대값에 따라 점프 모션 재생 ( 0.001 이상이면 점프중, 0이면 점프X )
            anim.SetFloat("doJump", Mathf.Abs(rig2d.velocity.y));

            // 애니메이션 블렌드 설정
            // 점프 시 상승(+)일 때 상승 애니메이션 재생, 하강(-)일 때 하강 애니메이션 재생
            anim.SetFloat("jumpVel", rig2d.velocity.y); 
        }

        if (Managers.Data.PlayerLife > 0)
        {
            anim.SetBool("doDead", false);
        }
    }

    // 이동
    void Move()
    {
        // 좌우 입력키의 절대값에 따라 이동하는 애니메이션 재생
        anim.SetFloat("doRun", Mathf.Abs(moveInput));

        // 실제 이동에 관련된 코드
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // 좌우 입력값에 따라 캐릭터 좌,우 방향 설정
        if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0);
        }
    }

    // 대기
    void Idle()
    {
        speed = 2;
        anim.SetFloat("doRun", Mathf.Abs(moveInput));
        boxCol.enabled = true;
    }

    // 숙이기
    void Crouch()
    {
        // 숙이는 애니메이션 재생
        anim.SetBool("doCrouch", true);

        // 숙일때는 속도를 0으로 해서 이동불가
        speed = 0f;

        // 피격에 관련된 콜라이더를 비활성화
        boxCol.enabled = false;
    }

    // 점프
    void Jump()
    {
        // 2단 점프를 방지하기 위한 변수 설정
        isGrounded = false;

        // 실제 점프, jumpForce만큼 y축으로 힘을 가함
        rig2d.AddForce(new Vector2(jumpForceX, jumpForceY), ForceMode2D.Impulse);
    }

    // 대쉬
    void Dash()
    {
        // 연속 대쉬를 방지하기 위한 변수 설정
        canDash = false;

        // 대쉬 애니메이션 재생
        anim.SetTrigger("doDash");

        // 좌우 방향에 따른 대쉬 설정
        if (moveInput >= 0)
        {
            rig2d.velocity = new Vector2(dashForce, rig2d.velocity.y);
        }
        else
        {
            rig2d.velocity = new Vector2(-dashForce, rig2d.velocity.y);
        }

        // 대쉬 쿨타임
        StartCoroutine(DashCoolDownCo());
    }
    IEnumerator DashCoolDownCo()
    {
        // 1초로 설정 후 1초가 지나면 대쉬 가능하게 해주는 변수 설정
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
    protected virtual void WeekAttack()
    {
        anim.SetTrigger("doWeekAttack");
    }

    // 약간 차지한 공격
    protected virtual void CAttack()
    {
        anim.SetTrigger("doChargeAttack");
        Debug.Log("차지 공격");
    }

    // 최대 차지 공격
    protected virtual void FullCAttack()
    {
        backForce = 0;
        anim.SetTrigger("doFullCAttack");
        Debug.Log("풀차지 공격");
    }

    // 사망 관련 함수
    void Dead()
    {
        // 사망 상태 변수 설정
        isDead = true;

        // 현재 사망상태에 따른 애니메이션 실행
        anim.SetBool("doDead", isDead);
        anim.SetTrigger("isDead");

        Debug.Log("죽음");
    }

    IEnumerator LifeUp()
    {
        if (Managers.Data.PlayerGage >= 70)
        {
            speed = 0;
            anim.SetTrigger("doGet");
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            Managers.Data.PlayerLife++;
            Managers.Data.PlayerGage -= 70;
        }
    }

    // 애니메이션 마지막에 이벤트로 넣어줄 함수들
    // 자신의 현재 체력에서 적의 공격력 만큼 감소
    public void TakeDamage(int damage)
    {
        // 피격 애니메이션 재생
        anim.SetTrigger("doHit");

        // 플레이어의 HP에서 데미지만큼 감소
        Managers.Data.PlayerLife -= damage;

        // HP가 0 이하면 사망
        if (Managers.Data.PlayerLife <= 0)
        {
            Dead();
        }
    }

    // 애니메이션 특정 부분에서 이벤트로 사용될 함수
    public void EventSetMoveSpd()
    {
        speed = 2f;
        backForce = 3f;
    }
    public void EventSetAlive()
    {
        isDead = false;
    }
    public void EventSetDeadTimeScale()
    {
        Time.timeScale = 0;
    }
    public void EventSetAliveTimeScale()
    {
        Time.timeScale = 1;
    }

    // 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground 태그를 가진 오브젝트와 충돌하면
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 점프가 가능하게 하는 변수 설정
            jumpForceX = 0;

            if (collision.contacts[0].normal.y >= 0.7f)
            {
                isGrounded = true;
            }

            if (collision.contacts[0].normal.x >= 0.7f)
            {
                isGrounded = true;
                jumpForceX = 3f;
            }
            else if (collision.contacts[0].normal.x <= -0.7f)
            {
                isGrounded = true;
                jumpForceX = -3f;
            }
        }

        // Item 태그를 가진 오브젝트와 충돌 시
        else if (collision.gameObject.CompareTag("Item"))
        {
            // 속도를 0으로 하고 획득 애니메이션 실행
            speed = 0;
            anim.SetTrigger("doGet");
            Managers.Data._onElevator = true;
            // 충돌한 오브젝트는 비활성화
            collision.gameObject.SetActive(false);
            Debug.Log("아이템 획득");
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Add(collision.gameObject);
        }
        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
        }
    }
}
