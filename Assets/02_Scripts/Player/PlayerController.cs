using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 1f;
    float jumpForce = 4f;
    float dashForce = 2f;
    float moveInput;
    private float chargeTime = 0f;  // 차지하는 시간
    private float chargeAtk = 0.5f; // 기본 공격과 강공격의 경계
    private float fullChargeAtk = 1f; // 강공격과 특수공격의 경계

    bool isGrounded = true;  // 땅에 닿았는지 여부 ( 점프가능 여부 )
    bool canDash = true;

    private Rigidbody2D rig2d;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    private void Start()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleInput();
        Debug.Log("대시가능여부 : " + canDash);
    }

    void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(1) && canDash)
        {
            Dash();
        }

        if (moveInput != 0)
        {
            Move();
        }
        else
        {
            Idle();
        }

        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.S) && isGrounded)
        {
            anim.SetBool("doCrouch", false);
            speed = 1f;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButton(0))
        {
            StartCharge();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Attack();
        }

        anim.SetFloat("jumpVel", rig2d.velocity.y);
        anim.SetFloat("doJump", Mathf.Abs(rig2d.velocity.y));
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
    void WeekAttack()
    {
        anim.SetTrigger("doWeekAttack");
        Debug.Log("기본 공격");
    }

    // 약간 차지한 공격
    void CAttack()
    {
        anim.SetTrigger("doChargeAttack");
        Debug.Log("차지 공격");
    }

    // 최대 차지 공격
    void FullCAttack()
    {
        anim.SetTrigger("doFullCAttack");
        Debug.Log("풀차지 공격");
    }

    void EventSetMoveSpd()
    {
        speed = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //canDash = true;
        }
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Crouch,
    Jump,
    // 공격도 애니메이션 블렌드로 gauge값에 따라? 그러면 Attack 하나만 있어도 됨
    Attack,
    WeekAttack,
    MiddleAttack,
    StrongAttack,
    Hitted,
    Dead,
    Positive
}

public class PlayerController : MonoBehaviour
{
    PlayerState playerState;

    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rig2d;
    BoxCollider2D boxCol;

    float speed = 1f;
    float jumpForce = 5f;
    float lastMoveDir;

    bool isGrounded = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rig2d = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        playerState = PlayerState.Idle;
        rig2d.velocity = Vector2.zero;
    }

    
    void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                // 애니메이터에서 Run이랑 블렌드해서 따로 Idle 사용X, x속도가 0이면 자동으로 Idle
                IdleState();
                break;
            case PlayerState.Run:
                RunState();
                break;
            case PlayerState.Crouch:
                CrouchState();
                break;
            case PlayerState.Jump:
                JumpState();
                break;
            case PlayerState.Attack:
                Attack();
                break;
            case PlayerState.Hitted:
                
                break;
            case PlayerState.Dead:
                
                break;
            case PlayerState.Positive:
                
                break;
        }
    }

    private void Update()
    {
        NowStateCheck();
    }

    void NowStateCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerState = PlayerState.Attack;
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            playerState = PlayerState.Run;
        }

        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.S))
            {
                playerState = PlayerState.Crouch;
            }
            if (Input.GetKey(KeyCode.W))
            {
                playerState = PlayerState.Jump;
            }
        }
        

        if (!Input.anyKey)
        {
            playerState = PlayerState.Idle;
        }

    }

    void IdleState()
    {
        Debug.Log("Idle");
        boxCol.enabled = true;
    }

    void RunState()
    {
        Debug.Log("달리기");
        float moveInput = Input.GetAxisRaw("Horizontal");

        rig2d.velocity = new Vector2(moveInput * speed, rig2d.velocity.y);

        if (moveInput != 0)
        {
            lastMoveDir = moveInput;
        }
        sr.flipX = lastMoveDir < 0;
    }

    void CrouchState()
    {
        Debug.Log("숙이기");
        rig2d.velocity = Vector2.zero;
        boxCol.enabled = false;
    }

    void JumpState()
    {
        Debug.Log("점프");
        isGrounded = false;

        rig2d.AddForce(new Vector2(rig2d.velocity.x, jumpForce), ForceMode2D.Impulse);
        playerState = PlayerState.Run;
    }

    void Attack()
    {
        Debug.Log("공격");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.contacts[0].normal.y >= 0.7 || collision.contacts[0].normal.x >= 1 || collision.contacts[0].normal.x >= -1)
            {
                Debug.Log("경사면 초기화");
                isGrounded = true;
            }
        }
    }
}
*/