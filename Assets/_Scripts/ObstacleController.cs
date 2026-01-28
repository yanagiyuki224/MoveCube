using UnityEngine;

public enum ObstacleType
{
    Point,
    Obstacle
}
public class ObstacleController : MonoBehaviour
{
    public ObstacleType obstacleType;
    public Renderer cachedRenderer;
    public Color pointColor = Color.lightBlue;
    public Color obstacleColor = Color.purple;
    public void Awake()
    {
        cachedRenderer = GetComponent<Renderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(ObstacleType type)
    {
        obstacleType = type;
        switch (type)
        {
            case ObstacleType.Point:
                cachedRenderer.material.color = pointColor;
                break;
            case ObstacleType.Obstacle:
                cachedRenderer.material.color = obstacleColor;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
