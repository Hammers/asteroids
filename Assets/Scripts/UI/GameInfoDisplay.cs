using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameInfoDisplay : MonoBehaviour
{
        private Text _text;

        void Start()
        {
                _text = GetComponent<Text>();
                _text.gameObject.SetActive(false);
                GameManager.Instance.LevelBeginEvent += HandleLevelBegin;
                GameManager.Instance.GameOverEvent += HandleGameOver;
        }

        private void HandleGameOver()
        {
                ShowText("Game Over");
        }

        private void HandleLevelBegin(int level)
        {
                ShowText($"Wave {level}");
        }
        
        private void ShowText(string text)
        {
                CancelInvoke(nameof(HideText));
                _text.text = text;
                _text.gameObject.SetActive(true);
                Invoke(nameof(HideText),3f);
        }

        private void HideText()
        {
                _text.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
                GameManager.Instance.LevelBeginEvent -= HandleLevelBegin;
                GameManager.Instance.GameOverEvent -= HandleGameOver;
        }
}
