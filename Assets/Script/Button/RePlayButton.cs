using UnityEngine;

public class RePlayButton : ButtonBase
{
    private Collider2D collider2d;

    protected override void Start()
    {
        base.Start();

        collider2d = GetComponent<Collider2D>();
    }

    protected override void OnClick()
    {
        Mother.RefreshField();
        Mother.StartCountDown();
        collider2d.enabled = false;
    }
}
