using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlasherLineDrawer : ChildSystem
{
    [SerializeField]
    private Sprite dead;
    [SerializeField]
    private Sprite collect;
    [SerializeField]
    private Sprite blade;

    private float paddingX = 0;
    private float paddingYTop = 0;
    private float paddingYBottom = 0;

    private bool isDrawing = false;
    private Vector2 targetPos;
    private LineHitHolder lineHitHolder;

    private SpriteRenderer slashLine;
    private SlasherCore slasherCore;
    private SlasherSlash slasherSlash;

    private Coroutine safetyView = null;

    protected override void Start()
    {
        base.Start();

        slashLine = transform.GetChild(5).GetComponent<SpriteRenderer>();
        slasherSlash = GetComponent<SlasherSlash>();
    }

    void Update()
    {
        if(slasherCore == null)
        {
            Debug.LogError("SlasherCoreが設定されていません。");
            return;
        }

        if(Mother.CanMoveScene())
        {
            var mouse = Mouse.current;
            if(mouse.leftButton.wasPressedThisFrame && MathCore.IsInnerRange2D(ScreenCore.MousePos(), new Vector2(paddingX, paddingYBottom), new Vector2(Screen.width - paddingX, Screen.height - paddingYTop)))
            {
                isDrawing = true;
                targetPos = ScreenCore.WorldMousePos();
                targetPos = ArrangeArea(transform.position, targetPos);

                switch(slasherCore.Level)
                {
                    case SlasherCore.DangerLevel.WARNING :
                        safetyView = StartCoroutine(BlinkLine(0.2f));
                        break;
                    case SlasherCore.DangerLevel.DANGER :
                        safetyView = StartCoroutine(BlinkLine(0.05f));
                        break;
                }
            }
            else if(mouse.leftButton.wasReleasedThisFrame && isDrawing)
            {
                isDrawing = false;
                slashLine.gameObject.SetActive(false);
                slasherSlash.Core = slasherCore;
                slasherSlash.Slash(targetPos, lineHitHolder);
                
                if(safetyView != null)
                {
                    StopCoroutine(safetyView);
                    safetyView = null;
                    ChangeLineVisibility(true);
                }
            }
            else if(isDrawing)
            {
                DrawLine();
            }
            else
            {
                isDrawing = false;
                if(safetyView != null)
                {
                    StopCoroutine(safetyView);
                    safetyView = null;
                    ChangeLineVisibility(true);
                }
            }
        }
        else if(slashLine.gameObject.activeSelf)
        {
            isDrawing = false;
            slashLine.gameObject.SetActive(false);
        }
    }

    //現在地からマウス座標までガイド線を引く
    //画面サイズより少し小さい範囲内で線をカットする
    private void DrawLine()
    {
        targetPos = ScreenCore.WorldMousePos();
        targetPos = ArrangeArea(transform.position, targetPos);

        slashLine.gameObject.SetActive(true);
        slashLine.transform.eulerAngles = Vector3.zero;
        slashLine.transform.rotation = Quaternion.FromToRotation(slashLine.transform.up, targetPos - (Vector2)transform.position);
        slashLine.sprite = dead;

        RaycastHit2D[] lineHits = Physics2D.LinecastAll(transform.position, targetPos);
        List<CrystalCore> hitCrystals = new();
        List<EnemyCore> hitEnemies = new();
        bool canSurvive = false;
        if(lineHits.Length > 0)
        {
            SetColor(Color.gray);
            foreach(RaycastHit2D hit in lineHits)
            {
                switch (hit.collider.tag)
                {
                    case "Enemy":
                        EnemyCore enemy = hit.collider.GetComponent<EnemyCore>();
                        hitEnemies.Add(enemy);
                        if(CanKillingEnemies(hitEnemies))
                        {
                            SetColor(Color.white);
                            slashLine.sprite = blade;
                            canSurvive = true;
                        }
                        else if(!enemy.IsLeaving)
                        {
                            SetColor(Color.crimson);
                            slashLine.sprite = dead;
                            canSurvive = false;
                        }
                        else
                        {
                            hitEnemies.Remove(enemy);
                        }
                        break;
                    case "Crystal":
                        CrystalCore crystal = hit.collider.GetComponent<CrystalCore>();
                        hitCrystals.Add(crystal);
                        if(hitEnemies.Count == 0)
                        {
                            SetColor(Color.lightBlue);
                            slashLine.sprite = collect;
                            canSurvive = true;
                        }
                        break;
                }
            }
        }
        else
        {
            SetColor(Color.gray);
        }
        lineHitHolder = new(hitCrystals, hitEnemies, canSurvive, hitCrystals.Count + hitEnemies.Count > 0);

        Vector2 size = slashLine.size;
        size.y = Vector2.Distance(targetPos, transform.position) / slashLine.transform.lossyScale.x;
        slashLine.size = size;
    }

    //一次関数を利用し、ガイドラインを一定範囲内に切り取る
    private Vector2 ArrangeArea(Vector2 startPos, Vector2 endPos)
    {
        Vector2 screen_sPos = ScreenCore.WorldToScreenPos(startPos);
        Vector2 screen_ePos = ScreenCore.WorldToScreenPos(endPos);

        if(screen_ePos.x > Screen.width - paddingX)
        {
            screen_ePos.y = MathCore.AssignLinearFunctionOfX(screen_sPos, screen_ePos, Screen.width - paddingX);
            screen_ePos.x = Screen.width - paddingX;
        }
        else if(screen_ePos.x < 0 + paddingX)
        {
            screen_ePos.y = MathCore.AssignLinearFunctionOfX(screen_sPos, screen_ePos, 0 + paddingX);
            screen_ePos.x = 0 + paddingX;
        }

        if(screen_ePos.y > Screen.height - paddingYTop)
        {
            screen_ePos.x = MathCore.AssignLinearFunctionOfY(screen_sPos, screen_ePos, Screen.height - paddingYTop);
            screen_ePos.y = Screen.height - paddingYTop;
        }
        else if(screen_ePos.y < 0 + paddingYBottom)
        {
            screen_ePos.x = MathCore.AssignLinearFunctionOfY(screen_sPos, screen_ePos, 0 + paddingYBottom);
            screen_ePos.y = 0 + paddingYBottom;
        }

        return ScreenCore.ScreenToWorldPos(screen_ePos, 10);
    }

    private bool CanKillingEnemies(List<EnemyCore> hitEnemies)
    {
        if(slasherCore.BladeCount == 0)
        {
            return false;
        }

        List<float> bladeDamages = slasherCore.BladeDamages();
        foreach(var enemy in hitEnemies)
        {
            float hp = enemy.HP;
            float receivedDamage = 0;
            int loopCount = bladeDamages.Count;

            if(loopCount == 0)
            {
                return false;
            }

            for(int i = 0; i < loopCount && hp > 0; i++)
            {
                hp -= bladeDamages[0];
                receivedDamage += bladeDamages[0];
                bladeDamages.RemoveAt(0);

                if(i == loopCount - 1 && receivedDamage == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void SetColor(Color color)
    {
        color.a = slashLine.color.a;
        slashLine.color = color;
    }

    public void ChangeSafety(SlasherCore.DangerLevel level)
    {
        switch(level)
        {
            case SlasherCore.DangerLevel.SAFE :
                {
                    if(safetyView != null)
                    {
                        StopCoroutine(safetyView);
                        safetyView = null;
                        ChangeLineVisibility(true);
                    }
                    return;
                }
            case SlasherCore.DangerLevel.WARNING :
                {
                    if(safetyView != null)
                    {
                        StopCoroutine(safetyView);
                    }

                    if(isDrawing)
                    {
                        safetyView = StartCoroutine(BlinkLine(0.2f));
                    }
                    return;
                }
            case SlasherCore.DangerLevel.DANGER :
                {
                    if(safetyView != null)
                    {
                        StopCoroutine(safetyView);
                    }

                    if(isDrawing)
                    {
                        safetyView = StartCoroutine(BlinkLine(0.05f));
                    }
                    return;
                }
        }
    }

    IEnumerator BlinkLine(float interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            ChangeLineVisibility(slashLine.color.a == 0);
        }
    }

    private void ChangeLineVisibility(bool visible)
    {
        Color color = slashLine.color;
        color.a = visible ? 1 : 0;
        slashLine.color = color;
    }

    public SlasherCore Core
    {
        set{slasherCore = value;}
    }

    public float[] Paddings
    {
        get{return new float[]{paddingX, paddingYTop, paddingYBottom};}
        set
        {
            if(value[0] > 0)
            {
                paddingX = value[0];
            }

            if(value[1] > 0)
            {
                paddingYTop = value[1];
            }

            if(value[2] > 0)
            {
                paddingYBottom = value[2];
            }
        }
    }

    public struct LineHitHolder
    {
        public List<CrystalCore> hitCrystals;
        public List<EnemyCore> hitEnemies;
        public bool canSurvive;
        public bool hitAny;

        public LineHitHolder(List<CrystalCore> hitC, List<EnemyCore> hitE, bool survive, bool hit)
        {
            hitCrystals = hitC;
            hitEnemies = hitE;
            canSurvive = survive;
            hitAny = hit;
        }
    }
}
