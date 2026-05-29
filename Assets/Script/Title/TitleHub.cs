using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-4)]
public class TitleHub : ChildSystem
{
    private GameObject leftWall;
    private GameObject rightWall;
    private TitleSlasher titleSlasher;
    private StartButton startButton;

    private RectTransform rectTransform;

    protected override void Start()
    {
        base.Start();

        leftWall = transform.GetChild(0).gameObject;
        rightWall = transform.GetChild(1).gameObject;
        titleSlasher = transform.GetChild(2).GetComponent<TitleSlasher>();
        startButton = transform.GetChild(3).GetComponent<StartButton>();

        rectTransform = GetComponent<RectTransform>();

        if(Mother.IsLoading)
        {
            leftWall.GetComponent<RectTransform>().anchoredPosition = new Vector2(-rectTransform.rect.width - 350, 150);
            rightWall.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectTransform.rect.width + 350, 150);
            titleSlasher.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        titleSlasher.StartGame();

        StartCoroutine(OpenWall(new Vector2(1,0)));
    }

    public void BackTitle()
    {
        StartCoroutine(CloseWall(new Vector2(-1,0)));
    }

    IEnumerator OpenWall(Vector3 front)
    {
        leftWall.transform.position += -front / 8;
        rightWall.transform.position += front / 8;

        yield return new WaitForSeconds(0.3f);

        while(rightWall.transform.position.x < 50)
        {
            leftWall.transform.position += -front / 8;
            rightWall.transform.position += front / 8;
            front *= 1.1f;
            yield return null;
        }

        Mother.GoToField();
    }

    IEnumerator CloseWall(Vector3 front)
    {
        while(rightWall.transform.position.x > 0)
        {
            leftWall.transform.position += -front;
            rightWall.transform.position += front;
            front *= 1.2f;
            yield return null;
        }
        leftWall.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);
        rightWall.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);

        titleSlasher.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }
}
