using UnityEngine;

public struct PhysicsCore
{
    public static bool IsInCollider2D(Vector2 pos, Collider2D collider)
    {
        return pos == collider.ClosestPoint(pos);
    }
}
