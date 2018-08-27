using System;
using UnityEngine;

public class JumpMove : Move
{
    public JumpMotor Motor { get; private set; }
    public MoveStats Stats { get; private set; }

    public JumpMove(string name, MoveStats stats, JumpMotor motor) : base(name)
    {
        if (stats == null)
        {
            throw new ArgumentNullException("stats");
        }

        if (motor == null)
        {
            throw new ArgumentNullException("motor");
        }

        Stats = stats;
        Motor = motor;
    }

    protected override void NextMove(float deltaTime)
    {
        Motor.Update(deltaTime);
        Close();
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
