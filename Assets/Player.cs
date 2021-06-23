using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigid;
    [SerializeField] float speed;
    [SerializeField] Vector2 jumpForce = new Vector2(0, 1000f);
    [SerializeField] float gravityScale = 7f;
    [SerializeField] float valueMidAirVeloY = 10f;
    void Start()
    {
        speed = RunGameManager.instance.speed;
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
        animator.Play("Run");
    }

    string animationName;
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            rigid.AddForce(jumpForce);

        // 애니메이션

        // 0보다 작으면 추락
        // -10 ~ 10 = MidAir
        // 0보다 크면 상승
        float veloY = rigid.velocity.y;
        if (Mathf.Abs(veloY) < valueMidAirVeloY) // MidAir
            animationName = "MidAir";
        else if (veloY > 0)
            animationName = "JumpUp";
        else if (veloY == 0)
            animationName = "Run";
        else
            animationName = "JumpFall";

        animator.Play(animationName); 
    }
}
