using System;
using UnityEngine;

[Serializable]
public class MoveClip
{
    [SerializeField]
    private float duration;

    public float Duration {
        get { return duration; }
        set { duration = value; }
    }

    public IMove Move { get; set; }
}