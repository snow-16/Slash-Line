using System;
using System.Collections.Generic;
using UnityEngine;

public struct MathCore
{
    public static bool IsInnerRange(float value, float min, float max)
    {
        return value >= min && value <= max;
    }

    public static bool IsInnerRange2D(Vector2 value, Vector2 min, Vector2 max)
    {
        bool innerX = value.x >= min.x && value.x <= max.x;
        bool innerY = value.y >= min.y && value.y <= max.y;
        return innerX && innerY;
    }

    public static float AssignLinearFunctionOfX(Vector2 startPos, Vector2 endPos, float value)
    {
        float[] sAndI = CulculateLinearFunction(startPos, endPos);
        float slope = sAndI[0];
        float intercept = sAndI[1];

        return slope * value + intercept;
    }

    public static float AssignLinearFunctionOfY(Vector2 startPos, Vector2 endPos, float value)
    {
        float[] sAndI = CulculateLinearFunction(startPos, endPos);
        float slope = sAndI[0];
        float intercept = sAndI[1];

        return (value - intercept) / slope;
    }

    public static float[] CulculateLinearFunction(Vector2 startPos, Vector2 endPos)
    {
        float vectorX = endPos.x - startPos.x;
        float vectorY = endPos.y - startPos.y;
        float slope = vectorY / (vectorX != 0 ? vectorX : 1);
        float intercept = startPos.y - slope * startPos.x;
        return new float[]{slope, intercept};
    }
}
