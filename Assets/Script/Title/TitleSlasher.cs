using UnityEngine;

public class TitleSlasher : ChildSystem
{
    private GameObject[] slashLine = new GameObject[2];

    protected override void Start()
    {
        base.Start();

        slashLine[0] = transform.GetChild(5).gameObject;
        slashLine[1] = transform.GetChild(6).gameObject;
    }

    void Update()
    {
        if(Mother.IsTitle)
        {
            transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z - 3);
        }
    }

    public void StartGame()
    {
        transform.eulerAngles = Vector3.zero;
        Destroy(DrawLine(0), 0.5f);
        Destroy(DrawLine(1), 0.5f);
        Mother.Sound.SoundKill();
        
        gameObject.SetActive(false);
    }

    private GameObject DrawLine(int index)
    {
        GameObject line = Instantiate(slashLine[index], slashLine[index].transform.position, slashLine[index].transform.rotation);
        line.transform.SetParent(transform.parent);
        line.SetActive(true);
        return line;
    }
}
