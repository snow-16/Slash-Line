using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class EnemyCore : InstantObject
{
    [SerializeField]
    private float baseFadeStep;
    [SerializeField]
    private Sprite killed;

    private float hp = 1;
    private int haveWoods = 1;

    private float fadestep;
    private bool isLeaving = true;
    private bool changed = false;

    private Coroutine selfDestroy;
    private Coroutine blink;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        fadestep = -baseFadeStep;
        Vector2 pos = transform.position;
        pos.y -= 0.5f;
        transform.position = pos;

        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeSprite();

        selfDestroy = StartCoroutine(WaitSelfDestroy());
    }

    void Update()
    {
        FadeInOut();

        if(fadestep > baseFadeStep)
        {
            Destroy(gameObject);
        }

        if(changed)
        {
            UpdateHaveWoods();
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
                    isLeaving = false;
                    yield return new WaitForSeconds(surviveTime - 1);
                    blink = StartCoroutine(BlinkTree());
                    yield return new WaitForSeconds(1);
                    StopCoroutine(blink);
                    ChangeLineVisibility(true);
                    isLeaving = true;
                    fadestep++;
                }
                else if(fadestep < 0)
                {
                    fadestep++;
                }
                else
                {
                    fadestep += 3;
                }
            }
            yield return null;
        }
    }

    private IEnumerator BlinkTree()
    {
        while(true)
        {
            ChangeLineVisibility(spriteRenderer.color.a == 0);
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void ChangeLineVisibility(bool visible)
    {
        Color color = spriteRenderer.color;
        color.a = visible ? 1 : 0;
        spriteRenderer.color = color;
    }

    private void FadeInOut()
    {
        float powBaseStep = Mathf.Pow(baseFadeStep, 2);
        float fadeScale = (-Mathf.Pow(fadestep, 2) + powBaseStep) / (powBaseStep / 2);
        transform.localScale = new Vector3(transform.localScale.x, fadeScale, transform.localScale.z);
    }

    public int ReceiveDamage(float damage)
    {
        if(Mother.Slasher.BladeType == CrystalAndBlades.CAndBType.CURSE_DIAMOND)
        {
            hp = 0;
        }
        else
        {
            hp -= damage;
        }

        if(hp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            spriteRenderer.sprite = killed;
            StopCoroutine(selfDestroy);
            if(blink != null)
            {
                StopCoroutine(blink);
                ChangeLineVisibility(true);
            }
            Destroy(gameObject, 0.4f);
            return haveWoods;
        }
        ChangeSprite();
        return 0;
    }

    public void ChangeSprite()
    {
        EnemyDatas.EnemyData data = Mother.FindEnemyData(Mathf.CeilToInt(hp));
        spriteRenderer.sprite = data.alive_sprite;
        killed = data.dead_sprite;
        changed = true;
    }

    public void UpdateHaveWoods()
    {
        if(hp > 0)
        {
            EnemyDatas.EnemyData data = Mother.FindEnemyData(Mathf.CeilToInt(hp));
            haveWoods = data.haveWoods;
        }
        changed = false;
    }

    public void Stop()
    {
        StopCoroutine(selfDestroy);
        if(blink != null)
        {
            StopCoroutine(blink);
        }
    }

    public bool IsLeaving
    {
        get{return isLeaving;}
    }

    public float HP
    {
        get{return hp;}
        set{hp = value;}
    }
}
