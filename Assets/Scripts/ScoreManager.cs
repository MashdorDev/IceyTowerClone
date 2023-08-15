using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour{
    public int score = 0;
    private float previousHeight;

    public TextMeshProUGUI scoreText;

    private void Start() {
        previousHeight = transform.position.y;
        UpdateScore();
    }

    private void Update(){
        CheckHeight();
    }

    private void CheckHeight(){
        if(transform.position.y > previousHeight + 1 ){
            score++;
            previousHeight = transform.position.y;
            UpdateScore();
        }
    }

    public void UpdateScore(){
     if(scoreText != null) scoreText.text = "Score: " + score.ToString();
    }
}
