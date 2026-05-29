using System;
using UnityEngine;

[Serializable]
public struct Timer : ISerializationCallbackReceiver
{
    [SerializeField]
    private float maxTime;
    [SerializeField]
    private bool fullTime;

    private float time;

    public Timer(float _maxTime, bool _fullTime)
    {
        maxTime = _maxTime;
        fullTime = _fullTime;
        time = 0;
    }

    public void OnAfterDeserialize()
    {
        if (fullTime)
        {
            time = maxTime;
        }
    }

    public bool IsEnd()
    {
        return time >= maxTime;
    }

    public void ResetTimer()
    {
        time = 0;
    }

    public void ChangeMaxTime(float value)
    {
        maxTime = value;
    }

    public void ResetAndChangeTimer(float value)
    {
        ChangeMaxTime(value);
        ResetTimer();
    }

    public bool ProceedTime()
    {
        if (IsEnd())
        {
            return true;
        }

        time += Time.deltaTime;
        return IsEnd();
    }

    public bool LoopedProceedTime()
    {
        bool procced = ProceedTime();
        if (procced)
        {
            ResetTimer();
        }
        return procced;
    }

    public bool RandomlyProceedTime(Randomizer randomizer)
    {
        bool procced = ProceedTime();
        if (procced)
        {
            ResetAndChangeTimer((float)randomizer.Run());
        }
        return procced;
    }

    public float GetPercentage()
    {
        return time / maxTime;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
