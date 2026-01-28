using UnityEngine;

public class StartController : MonoBehaviour
{
    public GameObject MainCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMType.Start);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        GameManager.Instance.gameMode = GameMode.Normal;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");


    }
    public void ObstacleGame()
    {
        GameManager.Instance.gameMode = GameMode.ObstacleMode;
        UnityEngine.SceneManagement.SceneManager.LoadScene("ObstacleScene");
    }
    public void Setting()
    {
        MainCanvas.SetActive(false);
        SettingOption option = new SettingOption()
        {

            OnClose = () =>
            {
                MainCanvas.SetActive(true);
            },

        };
        SettingUI.SettingShow(option);
    }
}
