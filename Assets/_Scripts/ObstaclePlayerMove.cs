using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObstaclePlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb; // Cache to prevent GC
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float laneMoveSpeed = 10f;
    [SerializeField] private float laneOffset = 2f;
    public PlayerPosition CurrentPosition { get; private set; } = PlayerPosition.Center;
    private float targetX; // 目標X（キャッシュ）
    private int health = 3;
    public UIController uiController; // UIControllerへの参照
    void Awake()
    {
        GameManager.Instance.gameMode = GameMode.ObstacleMode;
        rb = GetComponent<Rigidbody>();
        targetX = 0f;
        CurrentPosition = PlayerPosition.Center;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver)
            return;
        Vector3 pos = rb.position;

        // 前進
        pos.z += forwardSpeed * Time.fixedDeltaTime;

        // レーン移動（滑らか）
        pos.x = Mathf.MoveTowards(
            pos.x,
            targetX,
            laneMoveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(pos);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMoveLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMoveLane(1);
        }
    }
    private void TryMoveLane(int dir)
    {
        int next = (int)CurrentPosition + dir;
        if (next < 0 || next > 2)
            return;

        CurrentPosition = (PlayerPosition)next;
        targetX = (next - 1) * laneOffset;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ObstacleController obstacle = other.GetComponent<ObstacleController>();
            if (obstacle.obstacleType == ObstacleType.Obstacle)
            {
                health--;
                SoundManager.Instance.PlaySE(SEType.Damage);
                uiController.DamageEffect(health);
                if (health <= 0)
                {
                    GameManager.Instance.Ranking(uiController);
                    GameManager.Instance.isGameOver = true;
                    uiController.ShowGameOver();
                    gameObject.SetActive(false);
                }
            }
            else if (obstacle.obstacleType == ObstacleType.Point)
            {
                // ポイント取得の処理
                GameManager.Instance.AddScore(uiController, 10);
                SoundManager.Instance.PlaySE(SEType.Acquisition);
                if (GameManager.Instance.score % 50 == 0)
                {
                    SpeedUp();
                }
            }
            other.gameObject.SetActive(false);
            
        }
    }
    void SpeedUp()
    {
        forwardSpeed += 2f;
        laneMoveSpeed += 2f;
    }
    public void ResetPlayer()
    {
        // 位置リセット
        transform.position = new Vector3(0f, 0.5f, 0f);
        rb.linearVelocity = Vector3.zero;
        CurrentPosition = PlayerPosition.Center;
        targetX = 0f;
        health = 3;
        uiController.ResetHearts();

        // 速度リセット
        forwardSpeed = 10f;
        laneMoveSpeed = 12f;

        // タイプリセット
        GameManager.Instance.ResetScore(uiController);

        // 表示リセット
        gameObject.SetActive(true);
    }
}
