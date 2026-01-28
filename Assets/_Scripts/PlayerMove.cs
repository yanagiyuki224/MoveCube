using UnityEngine;

public enum PlayerPosition
{
    Left,
    Center,
    Right
}

public enum Type
{
    Red,
    Pink,
    Green
}

public class PlayerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float laneOffset = 2f;
    [SerializeField] private float laneMoveSpeed = 12f; // レーン移動速度

    [SerializeField] UIController uiController;

    private Rigidbody rb; // Cache to prevent GC
    private Renderer cachedRenderer; // Cache

    public PlayerPosition CurrentPosition { get; private set; } = PlayerPosition.Center;
    public Type PlayerType = Type.Red;

    private float targetX; // 目標X（キャッシュ）

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        cachedRenderer = GetComponent<Renderer>();
        SetPlayerType(Type.Red);

        rb.interpolation = RigidbodyInterpolation.Interpolate; // 見た目のカクつき防止
        targetX = 0f;
    }

    private void Update()
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

    private void FixedUpdate()
    {
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

    private void TryMoveLane(int dir)
    {
        int next = (int)CurrentPosition + dir;
        if (next < 0 || next > 2)
            return;

        CurrentPosition = (PlayerPosition)next;
        targetX = (next - 1) * laneOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Ball ball = other.GetComponent<Ball>();
        if (ball == null)
            return;

        if (ball.BallType == PlayerType)
        {
            GameManager.Instance.AddScore(uiController, 1);
            if(GameManager.Instance.score % 10 == 0)
            {
                IncreaseSpeed();
            }
            SoundManager.Instance.PlaySE(SEType.Acquisition);
            ball.Deactivate(); // プール前提
        }
        else
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("Game Over");
#endif
            GameManager.Instance.Ranking(uiController);
            GameManager.Instance.isGameOver = true;
            uiController.ShowGameOver();
            gameObject.SetActive(false);
        }
    }

    public void SetPlayerType(Type type)
    {
        PlayerType = type;
        cachedRenderer.material.color = GameManager.Instance.typeColorMap[type];
    }
    public void IncreaseSpeed()
    {
        forwardSpeed += 2f;
        laneMoveSpeed += 3f;
    }
    public void ResetPlayer()
    {
        // 位置リセット
        transform.position = new Vector3(0f, 0.5f, 0f);
        rb.linearVelocity = Vector3.zero;
        CurrentPosition = PlayerPosition.Center;
        targetX = 0f;

        // 速度リセット
        forwardSpeed = 10f;
        laneMoveSpeed = 12f;

        // タイプリセット
        SetPlayerType(Type.Red);
        GameManager.Instance.ResetScore(uiController);

        // 表示リセット
        gameObject.SetActive(true);
    }
}
