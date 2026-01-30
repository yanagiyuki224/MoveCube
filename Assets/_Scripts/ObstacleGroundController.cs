using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations; // Listを使うために必要

public class ObstacleGroundController : MonoBehaviour
{
    [SerializeField] private Transform[] obstaclesSpawns;
    [SerializeField] private GameObject obstaclesPrefab;
    [SerializeField] private ObstacleController[] obstacles;

    float pointSpawn = 0.5f;
    float obstacleSpawn = 0.85f;

    void Awake()
    {
        int laneCount = obstaclesSpawns.Length;
        // 配列の初期化を忘れずに（インスペクターで設定していない場合）
        if (obstacles == null || obstacles.Length == 0)
            obstacles = new ObstacleController[laneCount];

        for (int i = 0; i < laneCount; i++)
        {
            GameObject obj = Instantiate(obstaclesPrefab, obstaclesSpawns[i], false);
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
            obstacles[i] = obj.GetComponent<ObstacleController>();
        }
    }

    public void ActivateObstacle()
    {
        Clear();

        // 1. 使用可能なレーン番号のリストを作成 (0, 1, 2)
        List<int> availableLanes = new List<int>();
        for (int i = 0; i < obstacles.Length; i++)
        {
            availableLanes.Add(i);
        }

        bool shouldSpawnPoint = Random.value < pointSpawn;
        bool shouldSpawnObstacle = Random.value < obstacleSpawn;

        // 2. 障害物の配置
        if (shouldSpawnObstacle)
        {
            float randomValue = Random.value;
            int r = (randomValue < 0.55f) ? 1 : 2;
            Debug.Log("Obstacle Count: " + r + "," + randomValue);
            for (int i = 0; i < r; i++)
            {
                int index = GetRandomLane(availableLanes);
                ActivateObjectAtLane(index, ObstacleType.Obstacle);
            }
        }

        // 3. ポイントの配置（障害物で使われたレーンは availableLanes から消えているため被らない）
        if (shouldSpawnPoint && availableLanes.Count > 0)
        {
            int index = GetRandomLane(availableLanes);
            float pointTypeRoll = Random.value;
            if (pointTypeRoll < 0.7f)
            {
                ActivateObjectAtLane(index, ObstacleType.PinkApple);
            }
            else
            {
                ActivateObjectAtLane(index, ObstacleType.Apple);
            }
        }
    }

    // リストからランダムにレーン番号を取得し、リストから削除する関数
    private int GetRandomLane(List<int> lanes)
    {
        int randomIndex = Random.Range(0, lanes.Count);
        int laneNumber = lanes[randomIndex];
        lanes.RemoveAt(randomIndex); // 選ばれたレーンを候補から消す
        return laneNumber;
    }

    private void ActivateObjectAtLane(int laneIndex, ObstacleType type)
    {
        obstacles[laneIndex].gameObject.SetActive(true);
        obstacles[laneIndex].Setup(type);
        // ここで ObstacleController 側のメソッドを呼び、見た目や属性を切り替える
        // 例: obstacles[laneIndex].Setup(type);
    }

    public void Clear()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].gameObject.SetActive(false);
        }
    }
}