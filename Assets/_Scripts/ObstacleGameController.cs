using UnityEngine;

public class ObstacleGameController : MonoBehaviour
{
    public ObstaclePlayerMove playerMove;
    public ObstacleGroundManager groundManager;

    public GameObject damageCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Playing);
    }

    public void RestartGame()
    {
        GameManager.Instance.score = 0;
        GameManager.Instance.comboCount = 0;
        playerMove.ResetPlayer();
        groundManager.ResetGround();
        GameManager.Instance.isGameOver = false;
        damageCanvas.SetActive(true);
        


    }
    public void Title()
    {
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.isDamage = false;
        GameManager.Instance.isHealAppleSpawned = false;
        GameManager.Instance.score = 0;
        GameManager.Instance.comboCount = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
