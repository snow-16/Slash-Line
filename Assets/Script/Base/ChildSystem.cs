using UnityEngine;

public class ChildSystem : MonoBehaviour
{
    private MotherSystem mother;

    protected virtual void Start()
    {
        //他にmotherを取得済みのスクリプトを持っているならそこからmotherを受け取る。
        ChildSystem[] attachedSystems = GetComponents<ChildSystem>();
        if(attachedSystems.Length > 1)
        {
            foreach(ChildSystem check in attachedSystems)
            {
                if(check.HaveMother())
                {
                    SetMother(check.Mother, false);
                    return;
                }
            }
        }

        //親がChildSystemの継承先スクリプトを持っているならそこからmotherを受け取る。
        Transform checkObj = transform;
        while(checkObj.parent != null)
        {
            checkObj = checkObj.parent;
            if(checkObj.TryGetComponent(out ChildSystem parentSystem))
            {
                if(!parentSystem.HaveMother())
                {
                    Debug.LogError($"{parentSystem.gameObject.name}のスクリプトの優先度を上げてください。");
                    return;
                }
                SetMother(parentSystem.Mother);
                return;
            }
        }

        //このオブジェクトが他オブジェクトに生成されたプレハブオブジェクトなら、生成元スクリプトからmotherを受け取る。
        if(IsInstant())
        {
            return;
        }

        SetMother(GameObject.Find("MotherSystem").GetComponent<MotherSystem>());
    }

    protected void SetMother(MotherSystem value)
    {
        SetMother(value, true);
    }

    private void SetMother(MotherSystem value, bool addList)
    {
        mother = value;
        if(addList)
        {
            mother.InputObject(gameObject);
        }
    }

    protected virtual bool IsInstant()
    {
        return false;
    }

    public MotherSystem Mother
    {
        get 
        {
            if(HaveMother())
            {
                return mother;
            }
            else
            {
                Debug.LogError($"{gameObject.name}はMotherSystemを見つけられませんでした。");
                return null;
            }
        }
    }

    public bool HaveMother()
    {
        return mother != null;
    }

    protected virtual void OnDestroy()
    {
        if(gameObject.tag != "SceneWall")
        {
            if(Time.frameCount == 0)
            {
                return;
            }

            Mother.RemoveObject(gameObject);
        }
    }
}
