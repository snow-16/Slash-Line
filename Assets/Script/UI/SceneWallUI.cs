using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-4)]
public class SceneWallUI : ChildSystem
{
    private static bool created = false;

    private RectTransform leftWall;
    private RectTransform rightWall;

    private Canvas canvas;

    protected override void Start()
    {
        if(!created)
        {
            base.Start();

            leftWall = transform.GetChild(0).GetComponent<RectTransform>();
            rightWall = transform.GetChild(1).GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();

            DontDestroyOnLoad(gameObject);

            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    public void OpenWall()
    {
        StartCoroutine(OpenWall(new Vector2(1, 0)));
    }

    public void CloseWall()
    {
        StartCoroutine(CloseWall(new Vector2(-1, 0)));
    }

    IEnumerator OpenWall(Vector3 front)
    {
        while(rightWall.position.x < 20)
        {
            rightWall.position += front * 2;
            leftWall.position += -front * 2;
            front = new Vector2(Mathf.Min(front.x * 1.5f, 5f), 0);
            yield return null;
        }

        Mother.StartCountDown();
    }

    IEnumerator CloseWall(Vector3 front)
    {
        while(rightWall.anchoredPosition.x > 0)
        {
            rightWall.position += front * 2;
            leftWall.position += -front * 2;
            front = new Vector2(Mathf.Min(front.x * 1.5f, 3f), 0);
            yield return null;
        }
        rightWall.anchoredPosition = leftWall.anchoredPosition = Vector2.zero;

        Mother.RefreshField();
        Mother.GoToTitle();
    }
}
