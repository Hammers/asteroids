using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreDisplay : MonoBehaviour
{
    private Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        
        GameManager.Instance.ScoreUpdatedEvent += HandleScoreUpdated;

        _text.text = GameManager.Instance.Score.ToString();
    }

    // Update is called once per frame
    void OnDestroy()
    {
        GameManager.Instance.ScoreUpdatedEvent -= HandleScoreUpdated;
    }

    private void HandleScoreUpdated(int newScore)
    {
        _text.text = newScore.ToString();
    }
}
