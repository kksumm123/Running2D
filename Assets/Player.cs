using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;

    [SerializeField] float speed = 20f;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
