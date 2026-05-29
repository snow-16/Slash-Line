using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    [SerializeField]
    private float targetHeight = 800;

    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    void Update()
    {
        float ratio = targetHeight / Screen.height;
        canvasScaler.scaleFactor = 1 / ratio;
    }
}
