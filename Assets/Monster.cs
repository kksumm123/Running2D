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
            if (state == value)
                return;
            state = value;
            switch (state)
            {
                case StateType.Idle:
                    animator.Play("Idle");
                    break;
                case StateType.Attack:
                    break;
                case StateType.Patrol:
                    animator.Play("Run");
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
        State = StateType.Patrol;
        while (isPatrol)
        {
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
        State = StateType.Hit;

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
        State = StateType.Die;
        StopCoroutine(patrolHandle);
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
