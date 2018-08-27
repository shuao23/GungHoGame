using System;
using UnityEngine;

public class JumpMotor : Motor<JumpMotorStats>
{
    public JumpMotor(Rigidbody2D rigidbody, JumpMotorStats stats) : base(rigidbody, stats) { }
    public JumpMotor(Rigidbody2D rigidbody) : base(rigidbody, new JumpMotorStats()) { }


    public override void Update(float deltaTime)
    {
        Rigidbody.AddForce(Vector2.up * Stats.Velocity, ForceMode2D.Impulse);
    }
}
