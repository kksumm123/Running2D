using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Run
{
    public class RunGameManager : MonoBehaviour
    {
        TextMeshProUGUI timeText;
        TextMeshProUGUI scoreUI;

        internal void EndStage()
        {
            gameStateType = GameStateType.End;
            Player.instance.OnEndStage();
            timeText.text = "CLEAR";
        }

        public static RunGameManager instance;
        public float speed = 6f;
        [SerializeField] int waitSeconds = 3;
        [SerializeField] int score;
        internal void AddCoin(int value)
        {
            score += value;
            scoreUI.text = score.ToString();
        }

        void Awake()
        {
            instance = this;
        }
        IEnumerator Start()
        {
            timeText = transform.Find("Canvas/TimeText").GetComponent<TextMeshProUGUI>();
            scoreUI = transform.Find("Canvas/ScoreUI").GetComponent<TextMeshProUGUI>();
            for (int i = waitSeconds; i > 0; i--)
            {
                timeText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            timeText.text = "START!";
            yield return new WaitForSeconds(0.5f);
            gameStateType = GameStateType.Playing;
            timeText.text = "";
        }
        public static bool IsPlaying()
        {
            return instance.gameStateType == GameStateType.Playing;
        }
        public GameStateType gameStateType = GameStateType.Ready;
        public enum GameStateType
        {
            Ready,
            Playing,
            End,
        }
    }
}