using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run
{
    public class FinishFlag : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                RunGameManager.instance.EndStage();
        }
    }
}
