using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class InputActionHolder : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset asset;

    private Dictionary<string, Dictionary<string, InputAction>> actions = new();

    void Start()
    {
        foreach (var map in asset.actionMaps)
        {
            Dictionary<string, InputAction> action = new();
            foreach (var reference in map.actions)
            {
                action.Add(reference.name, reference);
            }
            actions.Add(map.name, action);
        }

        DontDestroyOnLoad(gameObject);
    }

    public ActionMapHolder Map(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.Log(name + "はInputActionAssetに含まれていません。");
            return new ActionMapHolder();
        }
        return new ActionMapHolder(name, actions[name]);
    }

    public InputAction Action(ActionMapHolder map, string name)
    {
        if (!map.map.ContainsKey(name))
        {
            Debug.Log(name + "はActionMap:" + map.name + "に含まれていません。");
            return null;
        }
        return map.map[name];
    }

    public struct ActionMapHolder
    {
        public string name;
        public Dictionary<string, InputAction> map;

        public ActionMapHolder(string _name, Dictionary<string, InputAction> _map)
        {
            name = _name;
            map = _map;
        }
    }

    [Serializable]
    public class InputActionGetter
    {
        [SerializeField]
        private List<string> actions;

        private string map;

        private InputActionHolder holder;

        public void SetMap(string _map)
        {
            map = _map;
            holder = GameObject.Find("InputActionHolder").GetComponent<InputActionHolder>();
        }

        public InputAction Read(string name)
        {
            if(actions.Contains(name))
            {
                return holder.Action(holder.Map(map), name);
            }
            Debug.Log("InputAction:" + name + "はGetterに登録されていない名称です。");
            return null;
        }

        public float ReadValue(string name)
        {
            return Read(name).ReadValue<float>();
        }

        public Vector2 ReadVec2Value(string name)
        {
            return Read(name).ReadValue<Vector2>();
        }

        public bool Pressed(string name)
        {
            return Read(name).IsPressed();
        }

        public bool PressedNow(string name)
        {
            return Read(name).WasPressedThisFrame();
        }

        public bool ReleasedNow(string name)
        {
            return Read(name).WasReleasedThisFrame();
        }
    }
}
