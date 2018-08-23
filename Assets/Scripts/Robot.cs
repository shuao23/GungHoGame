using System;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBod;
    [SerializeField]
    private MoveStats moveStats;

    private float deltaXQueue = 0;
    private bool jumpQueued = false;


    public void Jump()
    {
        jumpQueued = true;
    }

    /// <summary>
    /// Move on the horizontal axis. 
    /// </summary>
    /// <param name="unitDeltaX">The horizontal velocity usually from -1 to 1</param>
    public void Move(float unitDeltaX, MoveMode mode = MoveMode.Override)
    {
        switch (mode)
        {
            case MoveMode.Additive:
                deltaXQueue += unitDeltaX;
                break;
            case MoveMode.Override:
                deltaXQueue = unitDeltaX;
                break;
            default:
                throw new InvalidOperationException();
        }
    }


    #region Unity Methods
    private void Awake()
    {
        if (!TryFindRigidbody())
        {
            Debug.LogWarning("Rigidbody not assigned nor found. Disabling");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (jumpQueued)
        {
            ApplyJump();
        }

        ApplyMove(Time.fixedDeltaTime);
    }

    private void Reset()
    {
        TryFindRigidbody();
    }
    #endregion


    private void ApplyJump()
    {
        rigidBod.AddForce(Vector2.up * moveStats.JumpVelocity, ForceMode2D.Impulse);
        jumpQueued = false;
    }

    private void ApplyMove(float deltaTime)
    {
        float currentXVelocity = rigidBod.velocity.x;
        float targetXVelocity = deltaXQueue * moveStats.MaxVelocity;
        float newVelocity = currentXVelocity;
        if (currentXVelocity < targetXVelocity)
        {
            newVelocity = Mathf.Min(currentXVelocity + moveStats.Acceleration * deltaTime, targetXVelocity);
        }
        else if (currentXVelocity > targetXVelocity)
        {
            newVelocity = Mathf.Max(currentXVelocity - moveStats.Acceleration * deltaTime, targetXVelocity);
        }
        rigidBod.velocity = new Vector2(newVelocity, rigidBod.velocity.y);
        deltaXQueue = 0;
    }

    /// <summary>
    /// Tries to find rigid body on same gameobject
    /// </summary>
    /// <returns>True if a rigidbody was found</returns>
    private bool TryFindRigidbody()
    {
        if (rigidBod == null)
        {
            rigidBod = GetComponent<Rigidbody2D>();
        }
        return rigidBod != null;
    }


    [Serializable]
    public class MoveStats
    {
        [SerializeField]
        private float acceleration = 0.5f;
        [SerializeField]
        private float jumpVelocity = 1;
        [SerializeField]
        private float maxVelocity = 2;


        public float Acceleration {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public float JumpVelocity {
            get { return jumpVelocity; }
            set { jumpVelocity = value; }
        }

        public float MaxVelocity {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }
    }


    public enum MoveMode
    {
        Override, Additive
    }
}
