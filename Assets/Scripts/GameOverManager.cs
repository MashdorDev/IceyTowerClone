using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject playerNameInput;
    public GameObject submitScoreButton;
    public TextMeshProUGUI highScoreText;
    public HighScoreManager highScoreManager;

    private const string TopScoreKey = "TopScore";
    private const string TopScoreNameKey = "TopScoreName";

    public void CheckForHighScore()
    {
        int tenthScore = PlayerPrefs.GetInt(HighScoreManager.HighScoreKey + "9", 0);
        if (playerController.score > tenthScore)
        {
            playerNameInput.SetActive(true);
            submitScoreButton.SetActive(true);
        }
        else
        {
            Debug.Log("No High Score: " + playerController.score);
            LoadMainMenu();
        }
    }

    public void SubmitScore()
    {
        TMP_InputField inputField = playerNameInput.GetComponent<TMP_InputField>();
        if (inputField)
        {
            string playerName = inputField.text;
            highScoreManager.SaveHighScore(playerController.score, playerName);
            LoadMainMenu();
        }
        else
        {
            Debug.LogError("TMP_InputField component is missing or not properly set up on playerNameInput!");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void DisplayHighScore()
    {
        int topScore = PlayerPrefs.GetInt(TopScoreKey, 0);
        string topScoreName = PlayerPrefs.GetString(TopScoreNameKey, "No Name");
        highScoreText.text = "Top Score: " + topScoreName + " - " + topScore;
    }

    void Start()
    {
        DisplayHighScore();
    }
}
