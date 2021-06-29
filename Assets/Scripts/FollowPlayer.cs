using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTr;
    void Start()
    {
        playerTr = Player.instance.transform;
    }
    Vector3 newPos;
    [SerializeField] float lerpValue = 0.01f;

    void Update()
    {
        newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, playerTr.position.x, lerpValue);
        transform.position = newPos;
    }
}
