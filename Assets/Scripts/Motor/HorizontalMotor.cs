using System;
using UnityEngine;

public class HorizontalMotor
{
    private Rigidbody2D rigidbody;

    public Stats MotorStats { get; private set; } 


    public HorizontalMotor(Rigidbody2D rigidbody, Stats motorStats) {
        if (rigidbody == null)
        {
            throw new ArgumentNullException("rigidbody");
        }

        if (motorStats == null)
        {
            throw new ArgumentNullException("motorStats");
        }

        this.rigidbody = rigidbody;
        MotorStats = motorStats;
    }

    public HorizontalMotor(Rigidbody2D rigidbody) : this(rigidbody, new Stats()) { }


    public void Update(float deltaTime)
    {
        float currVelX = rigidbody.velocity.x;
        float targetXVelocity = -MotorStats.Direction * MotorStats.MaxVelocity;
        float newVelocity = currVelX;
        if (currVelX < targetXVelocity)
        {
            newVelocity = Mathf.Min(currVelX + MotorStats.Acceleration * deltaTime, targetXVelocity);
        }
        else if (currVelX > targetXVelocity)
        {
            newVelocity = Mathf.Max(currVelX - MotorStats.Acceleration * deltaTime, targetXVelocity);
        }
        rigidbody.velocity = new Vector2(newVelocity, rigidbody.velocity.y);
    }


    [Serializable]
    public class Stats
    {
        [SerializeField]
        private float acceleration = 20;
        [SerializeField]
        private float maxVelocity = 6;


        public float Acceleration {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public float Direction { get; set; }

        public float MaxVelocity {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }
    }
}
