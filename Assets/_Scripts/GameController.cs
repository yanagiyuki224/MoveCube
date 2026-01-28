using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerMove playerMove;
    public GroundLooper groundLooper;

    public void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Playing);
    }

    public void RestartGame()
    {
        groundLooper.ResetLooper();
        GameManager.Instance.isGameOver = false;
        playerMove.ResetPlayer();


    }
    public void Title()
    {
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
