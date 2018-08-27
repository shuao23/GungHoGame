using System;
using UnityEngine;

public class HorizontalMotor
{
    private Rigidbody2D rigidbody;


    public float Acceleration { get; set; }
    public float MaxVelocity { get; set; }
    public float Direction { get; set; }


    public HorizontalMotor(Rigidbody2D rigidbody)
    {
        if (rigidbody == null)
        {
            throw new ArgumentNullException("rigidbody");
        }

        this.rigidbody = rigidbody;
    }


    public void Update(float deltaTime)
    {
        float currVelX = rigidbody.velocity.x;
        float targetXVelocity = -Direction * MaxVelocity;
        float newVelocity = currVelX;
        if (currVelX < targetXVelocity)
        {
            newVelocity = Mathf.Min(currVelX + Acceleration * deltaTime, targetXVelocity);
        }
        else if (currVelX > targetXVelocity)
        {
            newVelocity = Mathf.Max(currVelX - Acceleration * deltaTime, targetXVelocity);
        }
        rigidbody.velocity = new Vector2(newVelocity, rigidbody.velocity.y);
    }
}
