using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] StateType state;
    StateType State
    {
        get { return state; }
        set { state = value; }
    }
    public enum StateType
    {
        IdleOrRunOrJump,
        Attack,
    }

    Animator animator;
    Rigidbody2D rigid;
    [SerializeField] float speed = 6;


    [SerializeField] Vector2 jumpForce = new Vector2(0, 1000f);
    [SerializeField] float gravityScale = 7f;
    [SerializeField] float valueMidAirVeloY = 10f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
        rayStart = transform;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    string animationName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            transform.Find("MagneticAbility").gameObject.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            transform.Find("MagneticAbility").gameObject.SetActive(true);
        Move();
        Jump();
        Attack();
        Animation();
    }


    float move;
    void Move()
    {
        // A, D 좌우이동
        move = 0;
        if (Input.GetKey(KeyCode.A))
            move = -1;
        if (Input.GetKey(KeyCode.D))
            move = 1;

        if (move != 0)
        {
            transform.Translate(move * speed * Time.deltaTime, 0, 0, Space.World);
            transform.rotation = Quaternion.Euler(0, move == 1 ? 0 : 180, 0);
        }
    }
    int jumpCount = 0;
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (jumpCount < 1)
            {
                rigid.velocity = Vector2.zero;
                rigid.AddForce(jumpForce);
                jumpCount++;
            }
        }
    }
    [SerializeField] string attackClipName = "Attack1";
    [SerializeField] float animationTime = 0.6f;
    private void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        State = StateType.Attack;
        animator.Play(attackClipName);
        yield return new WaitForSeconds(animationTime);
        State = StateType.IdleOrRunOrJump;
    }

    float veloY;
    void Animation()
    {
        if (State == StateType.Attack)
            return;
        // 애니메이션
        if (ChkGround())
        {
            jumpCount = 0;
            if (move == 0)
                animationName = "Idle";
            else
                animationName = "Run";
        }
        else
        {
            // 0보다 작으면 추락
            // -10 ~ 10 = MidAir
            // 0보다 크면 상승
            veloY = rigid.velocity.y;
            if (Mathf.Abs(veloY) < valueMidAirVeloY) // MidAir
                animationName = "MidAir";
            else if (veloY > 0)
                animationName = "JumpUp";
            else
                animationName = "JumpFall";
        }
        animator.Play(animationName);
    }

    [SerializeField] Transform rayStart;
    [SerializeField] float rayCheckDistance = 0.1f;
    [SerializeField] LayerMask groundLayer;
    bool ChkGround()
    {
        Debug.Assert(groundLayer != 0, "레이어 지정안됨");
        var hit = Physics2D.Raycast(
            rayStart.position + new Vector3(0, -1, 0), Vector2.down, rayCheckDistance, groundLayer);
        return hit.transform;
    }
    public void OnEndStage()
    {
        animator.Play("Idle");
    }
}
namespace Run
{
    public class Player : MonoBehaviour
    {
        public static Player instance;
        private void Awake()
        {
            instance = this;
        }

        Animator animator;
        Rigidbody2D rigid;
        [SerializeField] float speed;


        [SerializeField] Vector2 jumpForce = new Vector2(0, 1000f);
        [SerializeField] float gravityScale = 7f;
        [SerializeField] float valueMidAirVeloY = 10f;

        [SerializeField] Transform camTr;
        [SerializeField] float offsetXcampos;
        void Start()
        {
            speed = RunGameManager.instance.speed;
            animator = GetComponentInChildren<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = gravityScale;
            rayStart = transform;
            groundLayer = 1 << LayerMask.NameToLayer("Ground");
            camTr = Camera.main.transform;
            offsetXcampos = camTr.position.x - transform.position.x;
        }

        string animationName;
        void Update()
        {
            if (RunGameManager.IsPlaying() == false)
                return;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                transform.Find("MagneticAbility").gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                transform.Find("MagneticAbility").gameObject.SetActive(true);
            Move();
            Jump();
            Animation();

            ResotreXPosition();
        }

        float curOffestX;
        private void ResotreXPosition()
        {
            curOffestX = camTr.position.x - transform.position.x;
            transform.Translate(new Vector3(curOffestX - offsetXcampos, 0, 0) * Time.deltaTime);
        }

        void Move()
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        int jumpCount = 0;
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpCount < 1)
                {
                    rigid.velocity = Vector2.zero;
                    rigid.AddForce(jumpForce);
                    jumpCount++;
                }
            }
        }
        IEnumerator CoFn()
        {
            yield return null;
        }
        void Animation()
        {
            var coHandle = StartCoroutine(CoFn());
            StopCoroutine(coHandle);
            StopAllCoroutines();
            // 애니메이션
            if (ChkGround())
            {
                animationName = "Run";
                jumpCount = 0;
            }
            else
            {
                // 0보다 작으면 추락
                // -10 ~ 10 = MidAir
                // 0보다 크면 상승
                float veloY = rigid.velocity.y;
                if (Mathf.Abs(veloY) < valueMidAirVeloY) // MidAir
                    animationName = "MidAir";
                else if (veloY > 0)
                    animationName = "JumpUp";
                else
                    animationName = "JumpFall";
            }
            animator.Play(animationName);
        }

        [SerializeField] Transform rayStart;
        [SerializeField] float rayCheckDistance = 0.1f;
        [SerializeField] LayerMask groundLayer;
        bool ChkGround()
        {
            Debug.Assert(groundLayer != 0, "레이어 지정안됨");
            var hit = Physics2D.Raycast(
                rayStart.position + new Vector3(0, -1, 0), Vector2.down, rayCheckDistance, groundLayer);
            return hit.transform;
        }
        public void OnEndStage()
        {
            animator.Play("Idle");
        }
    }
}