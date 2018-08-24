using UnityEngine;

public static class Interpolate
{
    public static float EaseInOut(float t)
    {
        t = Mathf.Clamp01(t);
        float square = t * t;
        return square / (2.0f * (square - t) + 1.0f);
    }

    public static float EaseInOut(float start, float finish, float t)
    {
        return Linear(start, finish, EaseInOut(t));
    }

    public static float Linear(float start, float finish, float t)
    {
        t = Mathf.Clamp01(t);
        return t * (finish - start) + start;
    }
}