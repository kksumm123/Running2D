using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackCollider : MonoBehaviour
{
    [SerializeField] int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Monster>()?.OnDamage(damage);

    }
}
