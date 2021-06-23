using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGameManager : MonoBehaviour
{
    public static RunGameManager instance;
    public float speed = 6f;
    void Awake()
    {
        instance = this;
    }
}
