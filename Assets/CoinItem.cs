using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    bool ishit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ishit == false)
        {
            GetComponentInChildren<Animator>().Play("Hide", 1);
            RunGameManager.instance.AddCoin(100);
            ishit = true;
        }
    }
}
