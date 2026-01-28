using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject highScoreText;
    public GameObject[] heartIcons;
    void Awake()
    {
        UpdateScore(0);
        gameOverPanel.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highScoreText.SetActive(false);
    }
    public void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore.ToString();
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "Score: " + GameManager.Instance.score.ToString();
        // Implement game over UI display logic here
        Debug.Log("Game Over UI Shown");
    }
    public void UpdateHighScore()
    {
        
        highScoreText.SetActive(true);
    }
    public void DamageEffect(int currentHealth)
    {
        if(GameManager.Instance.gameMode != GameMode.ObstacleMode)
            return;
        // Implement damage effect logic here
        Debug.Log("Damage Effect Triggered");
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < currentHealth)
            {
                heartIcons[i].SetActive(true);
            }
            else
            {
                heartIcons[i].SetActive(false);
            }
        }
    }
    public void ResetHearts()
    {
        if(GameManager.Instance.gameMode != GameMode.ObstacleMode)
            return;
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(true);
        }
    }
}
