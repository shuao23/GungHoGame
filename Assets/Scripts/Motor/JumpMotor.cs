using System;
using UnityEngine;

public class JumpMotor
{
    private Rigidbody2D rigidbody;

    public Stats MotorStats { get; private set; }


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


    [Serializable]
    public class Stats
    {
        [SerializeField]
        private float velocity;

        public float Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
    }
}
