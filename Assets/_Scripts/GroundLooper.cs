

using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Transform[] grounds;
    [SerializeField] private float groundLength = 8f;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnChance = 0.8f;

    private GroundController[] controllers; // Cache
    private int nextIndex;
    private float nextZ;

    private void Awake()
    {
        int length = grounds.Length;
        controllers = new GroundController[length];

        for (int i = 0; i < length; i++)
        {
            controllers[i] = grounds[i].GetComponent<GroundController>();
        }
    }

    private void Start()
    {
        nextIndex = 0;
        nextZ = grounds[grounds.Length - 1].position.z + groundLength;

        // ★ 最初に置かれている Ground も全て抽選
        Type playerType = playerMove.PlayerType;

        for (int i = 0; i < controllers.Length; i++)
        {
            if (i >= 4)
            {
                controllers[i].Reroll(playerType, spawnChance);
            }

        }
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;

        if (player.position.z > grounds[nextIndex].position.z + groundLength)
        {
            MoveGroundToFront();
        }
    }

    private void MoveGroundToFront()
    {
        Transform ground = grounds[nextIndex];
        GroundController controller = controllers[nextIndex];

        // 前へ移動
        ground.position = new Vector3(0f, -1f, nextZ);

        // 再抽選（Destroyなし）
        controller.Reroll(playerMove.PlayerType, spawnChance);

        nextZ += groundLength;
        nextIndex++;

        if (nextIndex >= grounds.Length)
            nextIndex = 0;
    }
    public void ResetLooper()
    {
        Debug.Log("GroundLooper ResetLooper Called");
        // 1. 変数を完全に初期値に戻す
        nextIndex = 0;

        // 2. 地面を初期位置 (0, 8, 16...) に並べ直す
        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i].position = new Vector3(0, -1, i * groundLength);
            controllers[i].Clear(); // 玉を消し、色を白に戻す
        }

        // 3. nextZ を「最後の地面の次」に設定し直す
        nextZ = grounds[grounds.Length - 1].position.z + groundLength;

        // 4. 最初から配置されている地面の抽選（Startと同じルールを適用）
        Type playerType = playerMove.PlayerType;
        for (int i = 0; i < controllers.Length; i++)
        {
            // 最初の4枚 (0,1,2,3) は何もしない、4枚目以降だけ玉を出す
            if (i >= 4)
            {
                controllers[i].Reroll(playerType, spawnChance);
            }
        }
    }
}

