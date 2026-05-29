using UnityEngine;

[DefaultExecutionOrder(10)]
public class LoadSceneAnker : ChildSystem
{
    [SerializeField]
    private bool isField;

    protected override void Start()
    {
        base.Start();

        if(isField)
        {
            Mother.ArrivedField();
            Mother.ReloadUI();
        }
        else
        {
            Mother.ArrivedTitle();
        }
    }
}
