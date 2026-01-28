using System.Collections.Generic;
using UnityEngine;
using unityroom.Api;
public enum GameMode
{
    Normal,
    ObstacleMode,

}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool isGameOver = false;
    public int highScore = 0;
    public int ObstacleHighScore = 0;
    public GameMode gameMode = GameMode.Normal;
    public Dictionary<Type, Color> typeColorMap = new Dictionary<Type, Color>()
    {
        { Type.Red, Color.red },
        { Type.Pink, Color.pink },
        { Type.Green, Color.green }
    };
    public int score = 0;
    protected override void Awake()
    {
        base.Awake();

        highScore = PlayerPrefs.GetInt("HIGHSCORE", 0);
    }
    public void AddScore(UIController uiController, int amount)
    {
        score += amount;
        uiController.UpdateScore(score);
    }
    public void ResetScore(UIController uiController)
    {


        score = 0;
        uiController.highScoreText.SetActive(false);
        uiController.gameOverPanel.SetActive(false);
        uiController.UpdateScore(score);
    }
    public void Ranking(UIController uiController)
    {
        if (gameMode == GameMode.ObstacleMode)
        {
            if (score > ObstacleHighScore)
            {
                PlayerPrefs.SetInt("OBSTACLEHIGHSCORE", score);
                PlayerPrefs.Save(); // 確実に保存を実行
                UnityroomApiClient.Instance.SendScore(2, score, ScoreboardWriteMode.HighScoreDesc);
                ObstacleHighScore = score;
                uiController.UpdateHighScore();
            }
            else
            {
                // スコアが更新されなかった場合も、現在のスコアを送信したい場合はここに記述
                // (ランキングの仕様によりますが、毎回送るのが一般的です)
                UnityroomApiClient.Instance.SendScore(2, (float)score, ScoreboardWriteMode.HighScoreDesc);
            }
            return;
        }
        else
        {
            if (score > highScore)
            {
                PlayerPrefs.SetInt("HIGHSCORE", highScore);
                PlayerPrefs.Save(); // 確実に保存を実行
                UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
                highScore = score;
                uiController.UpdateHighScore();
            }
            else
            {
                // スコアが更新されなかった場合も、現在のスコアを送信したい場合はここに記述
                // (ランキングの仕様によりますが、毎回送るのが一般的です)
                UnityroomApiClient.Instance.SendScore(1, (float)score, ScoreboardWriteMode.HighScoreDesc);
            }
        }

    }

}
