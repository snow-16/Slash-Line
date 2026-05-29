using UnityEngine;

public class SlashLineMover : MonoBehaviour
{
    [SerializeField]
    private float amplitude;

    private Vector2 centorPos;

    void Start()
    {
        Destroy(gameObject, 0.25f);
        centorPos = transform.position;
    }

    void Update()
    {
        transform.position = centorPos + (Vector2)transform.right * amplitude;
        amplitude = Mathf.Max(Mathf.Abs(amplitude) * 0.8f, 0) * -Mathf.Sign(amplitude);
    }
}
