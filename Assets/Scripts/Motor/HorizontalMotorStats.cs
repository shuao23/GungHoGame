using System;
using UnityEngine;

[Serializable]
public class HorizontalMotorStats
{
    [SerializeField]
    private float acceleration = 20;
    [SerializeField]
    private float maxVelocity = 6;


    public float Acceleration {
        get { return acceleration; }
        set { acceleration = value; }
    }

    public float MaxVelocity {
        get { return maxVelocity; }
        set { maxVelocity = value; }
    }
}