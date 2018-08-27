using System;
using UnityEngine;

public class JumpMotor
{
    private Rigidbody2D rigidbody;


    public float Velocity { get; set; }


    public JumpMotor(Rigidbody2D rigidbody)
    {
        if(rigidbody == null)
        {
            throw new ArgumentNullException("rigidbody");
        }

        this.rigidbody = rigidbody;
    }


    public void Update(float deltaTime)
    {
        rigidbody.AddForce(Vector2.up * Velocity, ForceMode2D.Impulse);
    }
}
