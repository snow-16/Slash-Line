using System.Collections.Generic;
using UnityEngine;

public class InstantGenerator : MonoBehaviour
{
    private List<GameObject> generated = new();
    private GameObject prefab;
    
    public InstantGenerator Set(GameObject _prefab)
    {
        prefab = _prefab;
        return this;
    }

    //オブジェクトを生成し、名前の末尾に他の同種オブジェクトと被らない番号を追加する。
    public GameObject Generate(Vector2 pos, Quaternion rot)
    {
        GameObject obj = Instantiate(prefab, pos, rot);
        obj.GetComponent<InstantObject>().InstantGenerator = this;
        int count = 0;
        if(generated.Count == 0)
        {
            obj.name = prefab.name + ":0";
        }
        else
        {
            for(count = 0; count < generated.Count; count++)
            {
                int name_num = int.Parse(generated[count].name.Split(":")[1]);
                if(count != name_num)
                {
                    break;
                }
            }
            obj.name = prefab.name + ":" + count;
        }
        generated.Insert(count, obj);
        return obj;
    }

    public Quaternion Identity()
    {
        return prefab.transform.rotation;
    }

    public void Remove(GameObject obj)
    {
        generated.Remove(obj);
    }

    public List<GameObject> Generated
    {
        get{return generated;}
    }
}
