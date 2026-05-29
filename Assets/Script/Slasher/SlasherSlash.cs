using System.Collections.Generic;
using UnityEngine;

public class SlasherSlash : ChildSystem
{
    [SerializeField]
    private GameObject _slashLine;

    private SlasherCore _slasherCore;

    public void Slash(Vector2 targetPos, SlasherLineDrawer.LineHitHolder lineHitHolder)
    {
        bool canSurvive = lineHitHolder.canSurvive;
        bool canStop = lineHitHolder.hitAny;

        if(_slasherCore.BladeType == CrystalAndBlades.CAndBType.SOUL_STONE)
        {
            canSurvive = canStop = true;
            _slasherCore.UseBlade(1);
        }

        Vector2 beforePos = transform.position;
        transform.position = targetPos;

        Quaternion lookAt = Quaternion.FromToRotation(_slashLine.transform.up, targetPos - beforePos);
        SpriteRenderer newLine = Instantiate(_slashLine, beforePos, lookAt).GetComponent<SpriteRenderer>();
        newLine.color = Color.white;
        Vector2 size = newLine.size;
        size.y = Vector2.Distance(targetPos, beforePos) / _slashLine.transform.lossyScale.x;
        size.y *= canStop ? 1 : 100;
        newLine.size = size;

        if(canSurvive)
        {
            _slasherCore.ResetRotVelocity();

            List<EnemyCore> hitEnemies = lineHitHolder.hitEnemies;
            List<CrystalCore> hitCrystals = lineHitHolder.hitCrystals;

            if(hitEnemies.Count > 0)
            {
                Mother.Sound.SoundKill();
                var collectRate = 0f;
                var enemyCount = lineHitHolder.hitEnemies.Count;
                for(int i = 0; i < enemyCount; i++)
                {
                    collectRate += OnHitEnemy(hitEnemies[i]);
                }
                _slasherCore.AddWood(Mathf.FloorToInt(collectRate * _slasherCore.GetBonusMultiplier() * (enemyCount - (enemyCount - 1) * 0.5f)));
            }

            if(hitCrystals.Count > 0)
            {
                Mother.Sound.SoundCollect();
                foreach(var crystal in hitCrystals)
                {
                    OnHitCrystal(crystal);
                }
            }
        }
        else
        {
            _slasherCore.EndRotation();
            Mother.EndGame();

            if(!lineHitHolder.hitAny)
            {
                transform.position = new Vector2(100, 100);
            }
            else
            {
                _slasherCore.ResetBlades();
            }

            Mother.Sound.SoundAway();
        }
    }

    private float OnHitEnemy(EnemyCore hitEnemy)
    {
        if(hitEnemy != null)
        {
            float collectRate = 1;

            int bladeCount = _slasherCore.BladeCount;
            float hp = hitEnemy.HP;
            float totalDamage = 0;
            int count = 0;
            for(int i = 0; i < bladeCount; i++)
            {
                for(int j = 0; j < _slasherCore.GetCanUsingCount(i); j++)
                {
                    if(_slasherCore.GetCollectRate() > collectRate)
                    {
                        collectRate = _slasherCore.GetCollectRate();
                    }

                    float damage = _slasherCore.UsingBlade(i);
                    totalDamage += damage;
                    hp -= damage;
                    count++;

                    if(hp <= 0)
                    {
                        break;
                    }
                }

                if(hp <= 0)
                {
                    break;
                }
            }

            _slasherCore.UseBlade(count);
            int dropWoods = hitEnemy.ReceiveDamage(totalDamage);
            return dropWoods * collectRate;
        }

        return 0;
    }

    private void OnHitCrystal(CrystalCore hitCrystal)
    {
        if(hitCrystal != null)
        {
            _slasherCore.ResetRotVelocity();
            hitCrystal.Collect();

            if(_slasherCore.BladeCount < 4)
            {
                _slasherCore.Collect(hitCrystal.Type);
            }
        }
    }

    public SlasherCore Core
    {
        set{_slasherCore = value;}
    }
}
