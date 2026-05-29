using UnityEngine;

public class InstantObject : ChildSystem
{
    [Header("SurviveTimeRange")]
    [SerializeField]
    private float min;
    [SerializeField]
    private float max;

    protected float surviveTime;

    private InstantGenerator instantGenerator;

    public void LateSetMother(MotherSystem value)
    {
        SetMother(value);
    }

    protected override bool IsInstant()
    {
        return true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(instantGenerator != null)
        {
            instantGenerator.Remove(gameObject);
        }
    }

    public float[] SurviveTimeRange
    {
        get{return new float[]{min, max};}
    }

    public float SurviveTime
    {
        set{surviveTime = value;}
    }

    public InstantGenerator InstantGenerator
    {
        get{ return instantGenerator; }
        set{ instantGenerator = value; }
    }
}
