using System;
using UnityEngine;

public class JumpMove : Move
{
    public  JumpMotor Motor { get; private set; }
    public MoveStats Stats { get; private set; }

    public JumpMove(MoveStats stats, JumpMotor motor)
    {
        if (stats == null)
        {
            throw new ArgumentNullException("motor");
        }

        if (motor == null)
        {
            throw new ArgumentNullException("motor");
        }

        Stats = stats;
        Motor = motor;
    }

    protected override bool TryNextMove(float deltaTime)
    {
        Motor.Update(deltaTime);
        return false;
    }


    [Serializable]
    public class MoveStats
    {
        [SerializeField]
        private float velocity;

        public float Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
    }
}
