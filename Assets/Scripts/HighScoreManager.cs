using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;
    public const string HighScoreKey = "HighScore";
    public const string HighScoreNameKey = "HighScoreName";

    public void SaveHighScore(int score, string playerName)
    {
        bool newHighScore = false;
        for (int i = 0; i < 10; i++)
        {
            if (score > PlayerPrefs.GetInt(HighScoreKey + i, 0))
            {
                for (int j = 9; j > i; j--)
                {
                    PlayerPrefs.SetInt(HighScoreKey + j, PlayerPrefs.GetInt(HighScoreKey + (j - 1)));
                    PlayerPrefs.SetString(HighScoreNameKey + j, PlayerPrefs.GetString(HighScoreNameKey + (j - 1)));
                }
                PlayerPrefs.SetInt(HighScoreKey + i, score);
                PlayerPrefs.SetString(HighScoreNameKey + i, playerName);
                newHighScore = true;
                break;
            }
        }
        if (newHighScore)
        {
            PlayerPrefs.Save();
        }
    }

    public void LoadHighScores()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            int highScore = PlayerPrefs.GetInt(HighScoreKey + i, 0);
            string highScoreName = PlayerPrefs.GetString(HighScoreNameKey + i, "Anonymous");
            scoreTexts[i].text = (i + 1) + ". " + highScoreName + ": " + highScore;
            Debug.Log("Loaded High Score: " + highScoreName + " - " + highScore);
        }
    }

    private void Start()
    {
        LoadHighScores();
    }
}
