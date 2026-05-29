using UnityEngine;

public class StartButton : ButtonBase
{
    private Rigidbody2D rb2d;
    private Collider2D collider2d;

    protected override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    protected override void OnClick()
    {
        Mother.LeaveTitle();
        collider2d.enabled = false;
        rb2d.gravityScale = 5;
        rb2d.AddForce(new Vector2(Random.Range(-1.0f, 1.1f), 8), ForceMode2D.Impulse);
    }
}
