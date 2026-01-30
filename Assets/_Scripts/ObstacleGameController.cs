using UnityEngine;

public class ObstacleGameController : MonoBehaviour
{
    public ObstaclePlayerMove playerMove;
    public ObstacleGroundManager groundManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Playing);
    }

    public void RestartGame()
    {
        groundManager.ResetGround();
        GameManager.Instance.isGameOver = false;
        playerMove.ResetPlayer();


    }
    public void Title()
    {
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.score = 0;
        GameManager.Instance.comboCount = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
