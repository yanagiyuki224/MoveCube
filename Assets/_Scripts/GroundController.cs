
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField] private Transform[] ballSpawns;
    [SerializeField] private GameObject ballPrefab;

    private Ball[] balls; // Cache to prevent GC

    public int LaneCount => balls.Length;

    public bool isChange = false;
    public float changeChance = 0.05f;
    Type changedType;

    Renderer cachedRenderer;

    private static readonly Type[] allTypes =
    {
        Type.Red,
        Type.Pink,
        Type.Green
    };

    private void Awake()
    {
        int laneCount = ballSpawns.Length;
        cachedRenderer = GetComponent<Renderer>();
        balls = new Ball[laneCount];

        for (int i = 0; i < laneCount; i++)
        {
            GameObject obj = Instantiate(ballPrefab, ballSpawns[i], false);

            Transform t = obj.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;

            balls[i] = obj.GetComponent<Ball>(); // Cache
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 初期配置・再配置 共通（必ず3色）
    /// </summary>
    public void Reroll(Type safeType, float spawnChance = 0.8f)
    {
        Clear();

        if(Random.value < changeChance)
        {
            isChange = true;
            ChangePlayerType();
            return;
        }

        if (Random.value > spawnChance)
            return;

        int laneCount = balls.Length;

        // 通れるレーン
        int safeLane = Random.Range(0, laneCount);

        // --- safeType以外の2色を確定 ---
        Type[] otherTypes = new Type[2]; // 固定長・小サイズ（GC実質無視）
        int idx = 0;

        for (int i = 0; i < 3; i++)
        {
            Type t = (Type)i;
            if (t == safeType)
                continue;

            otherTypes[idx] = t;
            idx++;
        }

        // --- レーンへ割り当て ---
        int otherIndex = 0;

        for (int i = 0; i < laneCount; i++)
        {
            Ball ball = balls[i];
            ball.gameObject.SetActive(true);

            if (i == safeLane)
            {
                ball.SetType(safeType);
            }
            else
            {
                // 2色をランダム順で使う
                int pick = Random.Range(otherIndex, otherTypes.Length);
                Type chosen = otherTypes[pick];

                // swap（シャッフル）
                otherTypes[pick] = otherTypes[otherIndex];
                otherTypes[otherIndex] = chosen;

                ball.SetType(chosen);
                otherIndex++;
            }
        }
    }
    public void ChangePlayerType()
    {
        changedType = allTypes[Random.Range(0, allTypes.Length)];
        cachedRenderer.material.color = GameManager.Instance.typeColorMap[changedType];
    }
    public void Clear()
    {
        Debug.Log("GroundController Clear");
        isChange = false;
        cachedRenderer.material.color = Color.white;
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GroundController Collision"+collision.gameObject.name);
        if (!collision.gameObject.CompareTag("Player"))
            return;
        else
        {
            PlayerMove playerMove = collision.gameObject.GetComponent<PlayerMove>();
            if (isChange)
            {
                playerMove.SetPlayerType(changedType);


            }
        }
    }
}

