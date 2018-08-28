using System;
using UnityEngine;

public class HorizontalMotor : Motor<HorizontalMotorStats>
{
    public float Direction { get; set; }

    public HorizontalMotor(Rigidbody2D rigidbody, HorizontalMotorStats stats) : base(rigidbody, stats) { }
    public HorizontalMotor(Rigidbody2D rigidbody) : base(rigidbody, new HorizontalMotorStats()) { }


    public override void Update(float deltaTime)
    {
        float currVelX = Rigidbody.velocity.x;
        float targetXVelocity = -Direction * Stats.MaxVelocity;
        float newVelocity = currVelX;
        if (currVelX < targetXVelocity)
        {
            newVelocity = Mathf.Min(currVelX + Stats.Acceleration * deltaTime, targetXVelocity);
        }
        else if (currVelX > targetXVelocity)
        {
            newVelocity = Mathf.Max(currVelX - Stats.Acceleration * deltaTime, targetXVelocity);
        }
        Rigidbody.velocity = new Vector2(newVelocity, Rigidbody.velocity.y);
    }


    
}
