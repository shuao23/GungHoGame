using System;
using UnityEngine;

[Serializable]
public class SmoothVerticalFactor
{
    [SerializeField]
    private float normalizePower = 1;
    [SerializeField]
    private float dampingRate = 5;

    public float NormalizedVertical { get; set; }

    public void Initialize(float velocityY)
    {
        NormalizedVertical = SmoothNormalize(velocityY, normalizePower);
    }

    public void Update(float deltaTime, float velocityY)
    {
        float vertTarget = SmoothNormalize(velocityY, normalizePower);
        NormalizedVertical = Mathf.Lerp(NormalizedVertical, vertTarget, deltaTime * dampingRate);
    }

    private float SmoothNormalize(float value, float pow)
    {
        float normalized;
        if (value < 0)
        {
            normalized = 1 / Mathf.Pow(-value + 1, pow) - 1;
        }
        else
        {
            normalized = -1 / Mathf.Pow(value + 1, pow) + 1;
        }

        return normalized / 2 + 0.5f;
    }
}
