using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Animator animator;
    [SerializeField] float speed = 2;

    [SerializeField] StateType state;
    StateType State
    {
        get { return state; }
        set
        {
            state = value;
            switch (state)
            {
                case StateType.Idle:
                    animator.Play("Idle");
                    break;
                case StateType.Attack:
                    break;
                case StateType.Patrol:
                    animator.Play("Run", 0);
                    break;
                case StateType.Hit:
                    animator.Play("Hit");
                    break;
                case StateType.Die:
                    animator.Play("Die");
                    break;
            }
        }
    }
    public enum StateType
    {
        Idle,
        Attack,
        Patrol,
        Hit,
        Die,
    }
    Coroutine patrolHandle;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        patrolHandle = StartCoroutine(PatrolCo());
    }

    bool isPatrol = true;
    [SerializeField] float range = 1;
    float minWorldX;
    float maxWorldX;
    private IEnumerator PatrolCo()
    {
        minWorldX = transform.position.x - range;
        maxWorldX = transform.position.x + range;
        while (isPatrol)
        {
            if (State == StateType.Hit)
                yield return null;
            else
                State = StateType.Patrol;

            var pos = transform.position;

            if (minWorldX > pos.x || maxWorldX < pos.x)
                transform.Rotate(0, 180, 0);
            if (State == StateType.Patrol)
                transform.Translate(speed * Time.deltaTime, 0, 0);
            yield return null;
        }
    }

    [SerializeField] int hp = 10;
    internal void OnDamage(int damage)
    {
        hp -= damage;
        StartCoroutine(HitCo());

        if (hp <= 0)
        {
            StartCoroutine(DieCo());
        }
    }

    [SerializeField] float hitDelay = 0.3f;
    private IEnumerator HitCo()
    {
        State = StateType.Hit;
        yield return new WaitForSeconds(hitDelay);
        State = StateType.Idle;
    }
    [SerializeField] float destroyDelay = 0.7f;
    internal int damage = 1;

    private IEnumerator DieCo()
    {
        yield return new WaitForSeconds(hitDelay);
        State = StateType.Die;
        StopCoroutine(patrolHandle);
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
