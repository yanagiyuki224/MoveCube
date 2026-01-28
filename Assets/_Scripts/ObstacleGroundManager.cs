using UnityEngine;

public class ObstacleGroundManager : MonoBehaviour
{
    [SerializeField] Transform[] obstacleGrounds;
    [SerializeField] private ObstacleGroundController[] obstacleGroundControllers;
    [SerializeField] private Transform player;
    [SerializeField] float groundLength = 8f;
    private int nextIndex;
    private float nextZ;
    void Awake()
    {
        nextIndex = 0;

        // 配列のサイズを Obstacle Grounds に合わせる
        int count = obstacleGrounds.Length;
        obstacleGroundControllers = new ObstacleGroundController[count];

        nextZ = obstacleGrounds[count - 1].transform.position.z + groundLength;
        for (int i = 0; i < count; i++)
        {
            obstacleGroundControllers[i] = obstacleGrounds[i].GetComponent<ObstacleGroundController>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextIndex = 0;
        nextZ = obstacleGrounds[obstacleGrounds.Length - 1].position.z + groundLength;

        for (int i = 0; i < obstacleGroundControllers.Length; i++)
        {
            if (i >= 4)
            {
                obstacleGroundControllers[i].ActivateObstacle();
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (player.position.z > obstacleGrounds[nextIndex].transform.position.z + groundLength)
        {
            MoveGroundToFront();
        }
    }
    private void MoveGroundToFront()
    {
        Transform ground = obstacleGrounds[nextIndex];

        // 前へ移動
        ground.position = new Vector3(0f, -1f, nextZ);
        obstacleGroundControllers[nextIndex].ActivateObstacle();

        nextZ += groundLength;
        nextIndex++;

        if (nextIndex >= obstacleGrounds.Length)
            nextIndex = 0;
    }
    public void ResetGround()
    {
        Debug.Log("GroundLooper ResetLooper Called");
        // 1. 変数を完全に初期値に戻す
        nextIndex = 0;

        // 2. 地面を初期位置 (0, 8, 16...) に並べ直す
        for (int i = 0; i < obstacleGrounds.Length; i++)
        {
            obstacleGrounds[i].position = new Vector3(0, -1, i * groundLength);
            obstacleGroundControllers[i].Clear(); // 玉を消し、色を白に戻す
        }

        // 3. nextZ を「最後の地面の次」に設定し直す
        nextZ = obstacleGrounds[obstacleGrounds.Length - 1].position.z + groundLength;

        // 4. 最初から配置されている地面の抽選（Startと同じルールを適用）
        for (int i = 0; i < obstacleGroundControllers.Length; i++)
        {
            // 最初の4枚 (0,1,2,3) は何もしない、4枚目以降だけ玉を出す
            if (i >= 4)
            {
                obstacleGroundControllers[i].ActivateObstacle();
            }
        }
    }
}
