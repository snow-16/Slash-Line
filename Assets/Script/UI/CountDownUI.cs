using System.Collections;
using UnityEngine;

public class CountDownUI : ChildSystem
{
    private TMPro.TextMeshProUGUI count;
    private int countNum = 3;
    
    protected override void Start()
    {
        base.Start();

        count = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
    }


    public void StartCountDown()
    {
        count.gameObject.SetActive(true);
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while(countNum > 0)
        {
            count.text = $"{countNum}";
            yield return new WaitForSeconds(0.8f);
            countNum--;
        }

        countNum = 3;
        count.text = "3";
        count.gameObject.SetActive(false);
        Mother.StartStage();
    }
}
