using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    bool ishit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponentInChildren<Animator>().Play("Hide", 1);
            RunGameManager.instance.AddCoin(100);
            MagneticAbility.instance.RemoveItem(transform);
            Destroy(gameObject, 1f);
        }
    }
}
