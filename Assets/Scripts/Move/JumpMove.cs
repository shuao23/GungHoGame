using System;
using UnityEngine;

public class JumpMove : Move
{
    public JumpMotor Motor { get; private set; }

    public JumpMove(string name, JumpMotor motor) : base(name)
    {
        if (motor == null)
        {
            throw new ArgumentNullException("motor");
        }

        Motor = motor;
    }

    protected override void NextMove(float deltaTime)
    {
        Motor.Update(deltaTime);
        Close();
    }
}
