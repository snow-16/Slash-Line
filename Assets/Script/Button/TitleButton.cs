using UnityEngine;

public class TitleButton : ButtonBase
{
    private Collider2D collider2d;

    protected override void Start()
    {
        base.Start();

        collider2d = GetComponent<Collider2D>();
    }

    protected override void OnClick()
    {
        collider2d.enabled = false;
        Mother.CloseField();
    }
}
