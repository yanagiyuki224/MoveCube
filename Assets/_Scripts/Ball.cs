using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Ball : MonoBehaviour
{
    public Type BallType;

    [Header("Apples")]
    public GameObject PinkApple;
    public GameObject RedApple;
    public GameObject GreenApple;

    public GameObject currentApple;

    private Renderer cachedRenderer; // Cache to prevent GC

    private void Awake()
    {
        cachedRenderer = GetComponent<Renderer>();
    }


    public void SetType(Type type)
    {
        BallType = type;
        ApplyMaterial();
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    private void ApplyMaterial()
    {
        if(currentApple != null)
        {
            currentApple.SetActive(false);
        }
        switch (BallType)
        {
            case Type.Red:
                currentApple = RedApple;
                currentApple.SetActive(true);
                break;
            case Type.Pink:
                currentApple = PinkApple;
                currentApple.SetActive(true);
                break;
            case Type.Green:
                currentApple = GreenApple;
                currentApple.SetActive(true);
                break;
        }
    }
}
