using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAbility : MonoBehaviour
{
    public static MagneticAbility instance;
    private void Awake()
    {
        instance = this;
    }
    public void RemoveItem(Transform transform)
    {
        items.Remove(transform);
    }

    // 자석에 끌린 Tr, 가속도
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<CoinItem>() == null)
            return;
        //items[collision.transform] = 0;
        items[collision.transform] = new RefFloat();
    }


    [SerializeField] float accelerate = 20f;
    class RefFloat
    {
        public float acc;
    }
    Dictionary<Transform, RefFloat> items = new Dictionary<Transform, RefFloat>();
    float acceleration;
    Transform coinTr;
    void Update()
    {
        var pos = transform.position;

        foreach (var item in items)
        {
            coinTr = item.Key;
            acceleration = item.Value.acc + accelerate * Time.deltaTime;
            items[item.Key].acc = acceleration;

            Vector2 dir = (pos - coinTr.position).normalized;
            Vector2 move = dir * acceleration * Time.deltaTime;
            coinTr.Translate(move);
        }
    }
}
