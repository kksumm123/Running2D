using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAbility : MonoBehaviour
{
    // 자석에 끌린 Tr, 가속도
    Dictionary<Transform, float> items = new Dictionary<Transform, float>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<CoinItem>() == null)
            return;
        //items.Add(collision.transform, 0);
        items[collision.transform] = 0;
    }

    [SerializeField] float accelerate = 20f;
    private void Update()
    {
        var pos = transform.position;

        foreach (var item in items)
        {
            var coinTr = item.Key;
            float acceleration = item.Value;
            Vector2 dir = (pos - coinTr.position).normalized;
            Vector2 move = dir * (acceleration + accelerate) * Time.deltaTime;
            coinTr.Translate(move);
        }
    }
}
