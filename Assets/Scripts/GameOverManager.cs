using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class GameOverManager : MonoBehaviour{
    public GameObject playerNameInput;
    public GameObject submitScoreButton;
    public TextMeshProUGUI highScoreText;
    public HighScoreManager highScoreManager;
    public RectTransform scoreUIRectTransform;
    public ScoreManager scoreManager;
    public TextMeshProUGUI yourScoreText;
    public TextMeshProUGUI notHighScoreText;
    public TextMeshProUGUI returnPromptText;


    private const string TopScoreKey = "TopScore";
    private const string TopScoreNameKey = "TopScoreName";
    private bool isWaitingForKeyPress = false;

    void Start(){
        DisplayHighScore();
    }

    void Update(){
        if (isWaitingForKeyPress && Input.anyKeyDown){
            isWaitingForKeyPress = false;
            StopCoroutine(FlashText(returnPromptText, 100f));
            returnPromptText.enabled = true;
            LoadMainMenu();
        }
    }

    private IEnumerator FlashText(TextMeshProUGUI text, float interval){
        while (true)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(interval);
        }
    }


    private IEnumerator MoveToCenter(){
        Vector2 startPosition = scoreUIRectTransform.anchoredPosition;
        Vector2 targetPosition = Vector2.zero;
        float journeyLength = Vector2.Distance(startPosition, targetPosition);
        float startTime = Time.time;
        float distanceCovered, fractionOfJourney;

        while (Vector2.Distance(scoreUIRectTransform.anchoredPosition, targetPosition) > 0.1f)
        {
            distanceCovered = (Time.time - startTime) * 800f;
            fractionOfJourney = distanceCovered / journeyLength;
            scoreUIRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null;
        }

        scoreUIRectTransform.anchoredPosition = targetPosition;
    }

    public void CenterScoreUI(){
        StartCoroutine(MoveToCenter());
    }


    public void CheckForHighScore(){
        CenterScoreUI();

        int tenthScore = PlayerPrefs.GetInt(HighScoreManager.HighScoreKey + "9", 0);
        yourScoreText.text = "Your Score: " + scoreManager.score;

        if (scoreManager.score > tenthScore){
            playerNameInput.SetActive(true);
            submitScoreButton.SetActive(true);
        } else {
            notHighScoreText.gameObject.SetActive(true);
            returnPromptText.gameObject.SetActive(true);
            StartCoroutine(FlashText(returnPromptText, 10f));
            isWaitingForKeyPress = true;
        }
    }

    public void SubmitScore(){
        TMP_InputField inputField = playerNameInput.GetComponent<TMP_InputField>();
        if (inputField)
        {
            string playerName = inputField.text;
            highScoreManager.SaveHighScore(scoreManager.score, playerName);
            LoadMainMenu();
        } else  {
            Debug.LogError("TMP_InputField component is missing or not properly set up on playerNameInput!");
        }
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("Menu");
    }

    public void DisplayHighScore(){
        int topScore = PlayerPrefs.GetInt(TopScoreKey, 0);
        string topScoreName = PlayerPrefs.GetString(TopScoreNameKey, "No Name");
        highScoreText.text = "Top Score: " + topScoreName + " - " + topScore;
    }
}
