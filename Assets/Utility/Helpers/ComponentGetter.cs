using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ComponentGetter
{
    private MonoBehaviour getter;
    private Dictionary<string, Component> components;

    public ComponentGetter(MonoBehaviour _getter)
    {
        getter = _getter;
        components = new Dictionary<string, Component>();
    }

    private Component GetComponent(string key)
    {
        if(components.ContainsKey(key))
        {
            return components[key];
        }
        else
        {
            Debug.Log(key + "はリストに含まれていません。");
            return null;
        }
    }

    public ComponentGetter Rigidbody2D()
    {
        components.Add("Rigidbody2D", getter.GetComponent<Rigidbody2D>());
        return this;
    }
    public Rigidbody2D rb2D
    {
        get{
            return (Rigidbody2D)GetComponent("Rigidbody2D");
        }
    }

    public ComponentGetter Rigidbody()
    {
        components.Add("Rigidbody", getter.GetComponent<Rigidbody>());
        return this;
    }
    public Rigidbody rb
    {
        get{
            return (Rigidbody)GetComponent("Rigidbody");
        }
    }

    public ComponentGetter SpriteRenderer()
    {
        components.Add("SpriteRenderer", getter.GetComponent<SpriteRenderer>());
        return this;
    }
    public SpriteRenderer spriteRenderer
    {
        get{
            return (SpriteRenderer)GetComponent("SpriteRenderer");
        }
    }

    public ComponentGetter Image()
    {
        components.Add("Image", getter.GetComponent<Image>());
        return this;
    }
    public Image image
    {
        get{
            return (Image)GetComponent("Image");
        }
    }

    public ComponentGetter TextMeshProUGUI()
    {
        components.Add("TextMeshProUGUI", getter.GetComponent<TextMeshProUGUI>());
        return this;
    }
    public TextMeshProUGUI textMeshProUGUI
    {
        get{
            return (TextMeshProUGUI)GetComponent("TextMeshProUGUI");
        }
    }

    public ComponentGetter Camera()
    {
        components.Add("Camera", getter.GetComponent<Camera>());
        return this;
    }
    public Camera camera
    {
        get{
            return (Camera)GetComponent("Camera");
        }
    }
}
