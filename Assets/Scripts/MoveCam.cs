using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Run
{
    public class MoveCam : MonoBehaviour
    {
        [SerializeField] float speed;
        private void Start()
        {
            speed = RunGameManager.instance.speed;
        }
        void Update()
        {
            if (RunGameManager.IsPlaying() == false)
                return;

            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }
}
