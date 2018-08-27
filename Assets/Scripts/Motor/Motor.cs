using System;
using UnityEngine;

public abstract class Motor<T> where T : class
{
    protected Rigidbody2D rigidbody;

    public T Stats { get; protected set; }


    public Motor(Rigidbody2D rigidbody, T stats)
    {
        if (rigidbody == null)
        {
            throw new ArgumentNullException("rigidbody");
        }

        if (stats == null)
        {
            throw new ArgumentNullException("motorStats");
        }

        this.rigidbody = rigidbody;
        Stats = stats;
    }


    public abstract void Update(float deltaTime);
}