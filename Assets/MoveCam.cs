using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
