﻿using System;
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
        rayStart = transform;
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    string animationName;
    void Update()
    {
        Move();
        Jump();
        Animation();
    }


    void Move()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rigid.AddForce(jumpForce);
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
}
