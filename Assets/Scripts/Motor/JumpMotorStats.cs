using System;
using UnityEngine;

[Serializable]
public class JumpMotorStats
{
    [SerializeField]
    private float velocity = 800;

    public float Velocity {
        get { return velocity; }
        set { velocity = value; }
    }
}