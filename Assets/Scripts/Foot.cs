﻿using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Foot : MonoBehaviour
{
    [SerializeField]
    private Collider2D feetCollider;
    [SerializeField]
    private LayerMask groundCollision = ~0x0;

    private int groundCount = 0;


    public bool IsGrounded {
        get { return groundCount > 0; }
    }


    public event EventHandler OnLiftoff;
    public event EventHandler<LandingEventArgs> OnLanding;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGroundCollision(collision))
        {
            groundCount++;

            if (groundCount == 1 && OnLanding != null)
            {
                OnLanding(this, new LandingEventArgs(collision.relativeVelocity, collision.gameObject));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGroundCollision(collision))
        {
            groundCount--;

            if(groundCount == 0 && OnLiftoff != null)
            {
                OnLiftoff(this, EventArgs.Empty);
            }
        }
    }


    private bool IsGroundCollision(Collision2D collision)
    {
        bool validLayerOnCollider = (groundCollision.value & (1 << collision.gameObject.layer)) != 0;
        bool collisionOnFeet = ReferenceEquals(collision.otherCollider, feetCollider);
        return validLayerOnCollider && collisionOnFeet;
    }


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
}