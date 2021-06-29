using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMonster : BaseMonster
{
    // 왼쪽, 오른쪽 진행방향에 따라 Rotation 하도록

    AIPath aiPath;
    IEnumerator Start()
    {
        animator = GetComponentInChildren<Animator>();
        aiPath = GetComponent<AIPath>();
        while (true)
        {
            if (aiPath.desiredVelocity.x > 0)
                transform.rotation = Quaternion.identity;
            else if (aiPath.desiredVelocity.x < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);

            yield return null;
        }
    }
}

