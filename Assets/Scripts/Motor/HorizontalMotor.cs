using System;
using UnityEngine;

public class HorizontalMotor : Motor<HorzontalMotorStats>
{

    public HorizontalMotor(Rigidbody2D rigidbody, HorzontalMotorStats stats) : base(rigidbody, stats) { }
    public HorizontalMotor(Rigidbody2D rigidbody) : base(rigidbody, new HorzontalMotorStats()) { }


    public override void Update(float deltaTime)
    {
        float currVelX = rigidbody.velocity.x;
        float targetXVelocity = -Stats.Direction * Stats.MaxVelocity;
        float newVelocity = currVelX;
        if (currVelX < targetXVelocity)
        {
            newVelocity = Mathf.Min(currVelX + Stats.Acceleration * deltaTime, targetXVelocity);
        }
        else if (currVelX > targetXVelocity)
        {
            newVelocity = Mathf.Max(currVelX - Stats.Acceleration * deltaTime, targetXVelocity);
        }
        rigidbody.velocity = new Vector2(newVelocity, rigidbody.velocity.y);
    }


    
}
