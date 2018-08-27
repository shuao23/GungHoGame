using System;
using UnityEngine;

public abstract class Motor<T> where T : class
{
    private Rigidbody2D rigidbody;
    private T stats;

    public T Stats {
        get { return stats; }
        set {
            if (value == null)
            {
                throw new ArgumentNullException("Stats");
            }

            stats = value;
        }
    }

    public Rigidbody2D Rigidbody {
        get {
            return rigidbody;
        }

        set {
            if (value == null)
            {
                throw new ArgumentNullException("Rigidbody");
            }

            rigidbody = value;
        }
    }

    public Motor(Rigidbody2D rigidbody, T stats)
    {
        Rigidbody = rigidbody;
        Stats = stats;
    }


    public abstract void Update(float deltaTime);
}