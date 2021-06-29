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
        Attacked, //피격
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
        if (State == StateType.IdleOrRunOrJump)
        {
            Move();
            Jump();
        }
        Attack();
        Animation();
    }

    #region Move
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
    #endregion Move

    #region Jump
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
    #endregion Jump

    #region Attack
    [Serializable]
    class Attackinfo
    {
        public string clipName;
        public float animationTime; //0.6f;
        public float dashSpeed;
        public float dashTime;
        public GameObject collider;
    }
    [SerializeField] List<Attackinfo> attacks;
    Coroutine attackHandle;
    Attackinfo curAttack;
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (attackHandle != null)
            {
                curAttack?.collider.SetActive(false);
                StopCoroutine(attackHandle);
            }

            attackHandle = StartCoroutine(AttackCo());
        }
    }
    [SerializeField] int curATtackIdx = 0;
    private IEnumerator AttackCo()
    {
        State = StateType.Attack;
        curAttack = attacks[curATtackIdx];

        curATtackIdx++;
        if (curATtackIdx == attacks.Count)
            curATtackIdx = 0;

        animator.Play(curAttack.clipName);
        curAttack.collider.SetActive(true);
        // curAttack.dashTime 동안 curAttack.dashSpeed로 이동

        // 플레이 후 현재까지 지난 시간
        float dashEndTime = Time.time + curAttack.dashTime;
        float WaitEndTime = Time.time + curAttack.animationTime;
        while (WaitEndTime > Time.time)
        {
            if (dashEndTime > Time.time)
                transform.Translate(
                    curAttack.dashSpeed * Time.deltaTime, 0, 0, Space.Self);

            yield return null;
        }

        //연속공격 끝나고 실행되는 곳
        State = StateType.IdleOrRunOrJump;
        curAttack.collider.SetActive(false);
        curATtackIdx = 0;
    }
    #endregion Attack

    #region Animation
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
    #endregion Animation

    #region Methods
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
    #endregion Methods

    #region OnEvents
    [SerializeField] int hp = 5;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();
        if (monster == null)
            return;
        this.hp -= monster.damage;
        StartCoroutine(Hit());
    }
    [SerializeField] float hitDelay = 0.3f;
    private IEnumerator Hit()
    {
        State = StateType.Attacked;
        animator.Play("Hit");
        yield return new WaitForSeconds(hitDelay);
        State = StateType.IdleOrRunOrJump;
    }
    #endregion OnEvents
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