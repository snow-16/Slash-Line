using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class CrystalCore : InstantObject
{
    [SerializeField]
    private float baseFadeStep;

    private float fadestep;

    private Coroutine selfDestroy;
    private Coroutine blink;

    private CrystalAndBlades.CAndBType type;

    SpriteRenderer[] partsRenderer = new SpriteRenderer[4];
    
    protected override void Start()
    {
        base.Start();

        fadestep = -baseFadeStep;
        selfDestroy = StartCoroutine(WaitSelfDestroy());
    }

    void Update()
    {
        if(Mother.CanMoveScene())
        {
            FadeInOut();

            if(fadestep > baseFadeStep)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator WaitSelfDestroy()
    {
        while(true)
        {
            if(Mother.CanMoveScene())
            {
                if(fadestep == 0)
                {
                    yield return new WaitForSeconds(surviveTime - 1);
                    blink = StartCoroutine(BlinkCrystal());
                    yield return new WaitForSeconds(1);
                    StopCoroutine(blink);
                    ChangeLineVisibility(true);
                }
                fadestep++;
            }
            yield return null;
        }
    }

    private IEnumerator BlinkCrystal()
    {
        while(true)
        {
            ChangeLineVisibility(partsRenderer[0].color.a == 0);
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void ChangeLineVisibility(bool visible)
    {
        foreach(var part in partsRenderer)
        {
            Color color = part.color;
            color.a = visible ? 1 : 0;
            part.color = color;
        }
    }

    private void FadeInOut()
    {
        float fadeSpeed = -((Mathf.Pow(fadestep, 2) / 2) + 5);
        transform.localEulerAngles += new Vector3(0,0,fadeSpeed);

        float fadeScale = (-Mathf.Abs(fadestep) + baseFadeStep) / 50;
        transform.localScale = new Vector3(fadeScale, fadeScale, fadeScale);
    }

    public void SetType(CrystalAndBlades.CAndBType crystalType)
    {
        for(int i = 0; i < 4; i++)
        {
            partsRenderer[i] = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
        }
        
        type = crystalType;
        CrystalAndBlades.CAndBDatas data = Mother.FindCAndBData(type);
        SurviveTime = Random.Range(data.surviveTimeMin, data.surviveTimeMax + 1);
        foreach(SpriteRenderer part in partsRenderer)
        {
            part.color = data.crystal_color;
        }
    }

    public void Collect()
    {
        transform.DetachChildren();
        foreach(SpriteRenderer part in partsRenderer)
        {
            Destroy(part.gameObject, 0.3f);
            part.TryGetComponent(out Rigidbody2D rb);
            rb.AddForce(part.transform.up * 2, ForceMode2D.Impulse);
            rb.linearDamping = 5f;
        }
        if(blink != null)
        {
            StopCoroutine(blink);
            ChangeLineVisibility(true);
        }
        Destroy(gameObject);
    }

    public void Stop()
    {
        StopCoroutine(selfDestroy);
        if(blink != null)
        {
            StopCoroutine(blink);
        }
    }

    public CrystalAndBlades.CAndBType Type
    {
        get{return type;}
    }
}
