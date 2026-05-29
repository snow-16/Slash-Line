using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;

public struct ScreenCore
{
    public static Vector2 MousePos()
    {
        return Mouse.current.position.ReadValue();
    }

    public static Vector3 WorldMousePos()
    {
        return ScreenToWorldPos(MousePos(), 10);
    }

    public static Vector2 CameraRange()
    {
        return ScreenToWorldPos(new Vector2(Screen.width, Screen.height), 10);
    }

    public static Vector3 ScreenToWorldPos(Vector2 point, float z)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(point.x, point.y, z));
    }

    public static float ScreenToWorldPosX(float x)
    {
        return ScreenToWorldPos(new Vector2(x, 0), 10).x;
    }

    public static float ScreenToWorldPosY(float y)
    {
        return ScreenToWorldPos(new Vector2(0, y), 10).y;
    }

     public static Vector3 WorldToScreenPos(Vector2 point)
    {
        return Camera.main.WorldToScreenPoint(new Vector3(point.x, point.y));
    }

    public static float RandomWidth()
    {
        return Random.Range(0, Screen.width);
    }

    public static float RandomHeight()
    {
        return Random.Range(0, Screen.height);
    }

    public static Vector3 RandomPositionInScreen()
    {
        return ScreenToWorldPos(new Vector2(RandomWidth(), RandomHeight()), 10);
    }

    public static Vector3 RandomPositionInPaddingScreen(float padding)
    {
        return RandomPositionInPaddingScreen(padding, padding, padding, padding);
    }

    public static Vector3 RandomPositionInPaddingScreen(float paddingR, float paddingL, float paddingT, float paddingB)
    {
        return ScreenToWorldPos(new Vector2(Mathf.Min(RandomWidth(), Screen.width - (paddingR + paddingL)) + paddingL, Mathf.Min(RandomHeight(), Screen.height - (paddingT + paddingB)) + paddingB), 10);
    }

    public static Vector3 RandomPositionInOuter()
    {
        Vector2 pos = Vector2.zero;
        bool side = Random.value < 0.5f;

        if(Random.value < 0.5f)
        {
            pos.x = RandomWidth();
            pos.y = Screen.height * Convert.ToInt32(side);
        }
        else
        {
            pos.x = Screen.width * Convert.ToInt32(side);
            pos.y = RandomHeight();
        }

        return ScreenToWorldPos(pos, 10);
    }

    public static float UnderScreen()
    {
        return ScreenToWorldPos(new Vector2(0,0), 10).y - 10;
    }

    public static bool OutTheScreenWidth(Vector3 pos)
    {
        Vector3 viewPoint = Camera.main.WorldToViewportPoint(pos);
        return !MathCore.IsInnerRange(viewPoint.x, 0, 1);
    }

    public static bool OutTheScreenHeight(Vector3 pos)
    {
        Vector3 viewPoint = Camera.main.WorldToViewportPoint(pos);
        return !MathCore.IsInnerRange(viewPoint.y, 0, 1);
    }

    public static bool OutTheScreen(Vector3 pos)
    {
        return OutTheScreenWidth(pos) || OutTheScreenHeight(pos);
    }
}
