using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Apple,
    PinkApple,
    Obstacle
}

[System.Serializable]
public class ObstacleData
{
    public ObstacleType obstacleType;
    public GameObject obstacleObject;
    public int score;
}
public class ObstacleController : MonoBehaviour
{
    [SerializeField]public List<ObstacleData> obstacleDataList;
    public ObstacleType obstacleType;
    public Renderer cachedRenderer;
    public int scoreValue = 10;
    public void Awake()
    {
        cachedRenderer = GetComponent<Renderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(ObstacleType type)
    {
        obstacleType = type;
        foreach (var data in obstacleDataList)
        {
            if (data.obstacleType == obstacleType)
            {
                data.obstacleObject.SetActive(true);
                scoreValue = data.score;
            }
            else
            {
                data.obstacleObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
