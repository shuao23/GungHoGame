using System;
using UnityEngine;

public class StandardMove : Move
{
    public bool Continous { get; set; }
    public HorizontalMotor Motor { get; private set; }


    public StandardMove(string name, HorizontalMotor motor) : base(name)
    {
        if (motor == null)
        {
            throw new ArgumentNullException("motor");
        }

        Motor = motor;
        Continous = true;
    }

    protected override void NextMove(float deltaTime)
    {
        Motor.Update(deltaTime);
        if (!Continous)
        {
            Close();
        }
    }
}