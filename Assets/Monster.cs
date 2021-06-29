using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Animator animator;
    [SerializeField] float speed = 2;
    [SerializeField] float range = 1;
    float minWorldX;
    float maxWorldX;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(PatrolCo());
    }

    bool isPatrol = true;
    private IEnumerator PatrolCo()
    {
        minWorldX = transform.position.x - range;
        maxWorldX = transform.position.x + range;
        while (isPatrol)
        {
            var pos = transform.position;

            if (minWorldX > pos.x || maxWorldX < pos.x)
                transform.Rotate(0, 180, 0);

            transform.Translate(speed * Time.deltaTime, 0, 0);
            yield return null;
        }
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
