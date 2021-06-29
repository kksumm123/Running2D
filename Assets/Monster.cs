using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    [SerializeField] int hp = 10;
    internal void OnDamage(int damage)
    {
        hp -= damage;
        animator.Play("Hit");

        if (hp <= 0)
        {
            StartCoroutine(DieCo());
        }
    }

    [SerializeField] float dieDelay = 0.3f;
    [SerializeField] float destroyDelay = 0.7f;
    internal int damage = 1;

    private IEnumerator DieCo()
    {
        yield return new WaitForSeconds(dieDelay);
        animator.Play("Die");
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
