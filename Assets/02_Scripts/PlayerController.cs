using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float jumpForce = 5f;
    private float chargeTime = 0f;
    private float chargeAtk = 1f; // 기본 공격과 강공격의 경계
    private float fullChargeAtk = 2f; // 강공격과 특수공격의 경계

    public bool isGrounded = true;
    private bool isCharging = false;

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
        UpdateCharge();
    }

    void HandleInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            Move(moveInput);
        }
        else
        {
            Idle();
        }

        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            Crouch();
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCharge();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChargeAttack();
        }
    }

    void Move(float moveInput)
    {
        rig2d.velocity = new Vector2(moveInput * speed, rig2d.velocity.y);

        sr.flipX = moveInput < 0;
    }

    void Idle()
    {
        Debug.Log("대기");
        boxCol.enabled = true;
    }

    void Crouch()
    {
        Debug.Log("앉기");
        rig2d.velocity = Vector2.zero;
        boxCol.enabled = false;
    }

    void Jump()
    {
        isGrounded = false;
        rig2d.AddForce(new Vector2(rig2d.velocity.x, jumpForce), ForceMode2D.Impulse);
    }

    void StartCharge()
    {
        isCharging = true;
        chargeTime = 0f;
    }

    void ChargeAttack()
    {
        isCharging = false;

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
            BasicAttack();
        }

        chargeTime = 0f;
    }

    void UpdateCharge()
    {
        if (isCharging)
        {
            chargeTime += Time.deltaTime;
        }
    }

    void BasicAttack()
    {
        Debug.Log("기본 공격");
    }

    void CAttack()
    {
        Debug.Log("차지 공격");
    }

    void FullCAttack()
    {
        Debug.Log("풀차지 공격");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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