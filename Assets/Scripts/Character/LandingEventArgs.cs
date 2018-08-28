using System;
using UnityEngine;

public class LandingEventArgs : EventArgs
{
    private readonly Vector2 velocity;
    private readonly GameObject gameObject;


    /// <summary>
    /// Contact velocity of the landing
    /// </summary>
    public Vector2 Velocity { get { return velocity; } }
    /// <summary>
    /// The gameobject that this landed on.
    /// </summary>
    public GameObject GameObject { get { return gameObject; } }


    public LandingEventArgs(Vector2 velocity, GameObject gameObject)
    {
        this.velocity = velocity;
        this.gameObject = gameObject;
    }
}