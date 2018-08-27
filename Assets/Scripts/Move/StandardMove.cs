using System;
using UnityEngine;

public class StandardMove : Move
{
    public bool Continous { get; set; }
    public HorizontalMotor Motor { get; private set; }
    public MoveStats Stats { get; private set; }


    public StandardMove(MoveStats stats, HorizontalMotor motor)
    {
        if(stats == null)
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
        Motor.Acceleration = Stats.Acceleration;
        Motor.MaxVelocity = Stats.MaxVelocity;
        Motor.Update(deltaTime);
        if (!Continous)
        {
            Close();
        }
    }


    [Serializable]
    public class MoveStats
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
}