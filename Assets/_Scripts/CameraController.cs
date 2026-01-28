using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The target for the camera to follow
    public Vector3 offset;   // Offset from the target position
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return; // Stop following the target if the game is over
        }
        transform.position = new Vector3(offset.x, offset.y, target.position.z + offset.z);
    }
}
