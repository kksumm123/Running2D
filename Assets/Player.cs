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
    void Start()
    {
        speed = RunGameManager.instance.speed;
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravityScale;
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(jumpForce);
        }
    }
}
