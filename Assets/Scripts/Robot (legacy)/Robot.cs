using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Robot : MonoBehaviour
{
    [SerializeField]
    private MoveStats groundStats = MoveStats.Default;
    [SerializeField]
    private MoveStats landingStats = new MoveStats()
    {
        Acceleration = 20,
        MaxVelocity = 0
    };
    [SerializeField]
    private MoveStats airStats = MoveStats.Default;
    [SerializeField]
    private JumpStats jumpingStats = JumpStats.Default;
    [SerializeField]
    private LayerMask groundCollision = ~0x0;
    [SerializeField]
    private Collider2D feetCollider;

    private RoboAttack attackQueue = RoboAttack.None;
    private float deltaXQueue = 0;
    private float jumpQueueTime = -1;
    private int groundCount = 0;
    private bool isLandStunned = false;
    private RoboAttack attacking = 0;


    public MoveStats AirStats {
        get { return airStats; }
    }

    public RoboAttack Attacking {
        get { return attacking; }
    }

    public MoveStats GroundStats {
        get { return groundStats; }
    }

    public bool IsGrounded {
        get { return groundCount > 0; }
    }

    public bool IsLandStunned {
        get { return isLandStunned; }
    }

    public JumpStats JumpingStats {
        get { return jumpingStats; }
    }

    public bool JumpQueued {
        get { return jumpQueueTime >= 0; }
    }

    public MoveStats LandingStats {
        get { return landingStats; }
    }

    public Rigidbody2D Rigidbody { get; private set; }


    private bool ShouldJump {
        get { return jumpQueueTime >= jumpingStats.JumpReadyTime; }
    }


    public void Attack(RoboAttack attack, UpdateMode mode = UpdateMode.Override)
    {
        if (isLandStunned || !IsGrounded)
        {
            return;
        }

        switch (mode)
        {
            case UpdateMode.Override:
                attackQueue = attack;
                break;
            case UpdateMode.Once:
                if (attackQueue == RoboAttack.None)
                {
                    attackQueue = attack;
                }
                break;
            case UpdateMode.Additive:
            default:
                throw new InvalidOperationException();
        }
    }

    public void Jump()
    {
        if (!JumpQueued)
        {
            jumpQueueTime = 0;
        }
    }

    /// <summary>
    /// Move on the horizontal axis. 
    /// </summary>
    /// <param name="unitDeltaX">The horizontal velocity usually from -1 to 1</param>
    public void Move(float unitDeltaX, UpdateMode mode = UpdateMode.Override)
    {
        if (isLandStunned || attacking != RoboAttack.None)
        {
            return;
        }

        unitDeltaX *= -1; //Becacuse x axis is opposite now
        switch (mode)
        {
            case UpdateMode.Additive:
                deltaXQueue += unitDeltaX;
                break;
            case UpdateMode.Override:
                deltaXQueue = unitDeltaX;
                break;
            case UpdateMode.Once:
                if (deltaXQueue == 0)
                {
                    deltaXQueue = unitDeltaX;
                }
                break;
            default:
                throw new InvalidOperationException();
        }
    }


    #region Unity Methods
    private void Awake()
    {
        if (feetCollider == null)
        {
            Debug.LogWarning("Feet collider not assinged. Disabling");
            enabled = false;
            return;
        }

        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (JumpQueued)
        {
            jumpQueueTime += Time.fixedDeltaTime;
        }

        if (IsGrounded)
        {
            if (isLandStunned)
            {
                ApplyMove(landingStats, Time.fixedDeltaTime);
            }
            else if (attacking != RoboAttack.None)
            {
                //StartCoroutine(OnAttack());
                ApplyMove(landingStats, Time.fixedDeltaTime);
            }
            else
            {
                if (ShouldJump)
                {
                    ApplyJump();
                }

                ApplyMove(groundStats, Time.fixedDeltaTime);
            }
        }
        else
        {
            if (ShouldJump)
            {
                ClearJumpQueue();
            }

            ApplyMove(airStats, Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGroundCollision(collision) &&
            (ReferenceEquals(collision.collider, feetCollider) ||
            ReferenceEquals(collision.otherCollider, feetCollider)))
        {
            groundCount++;

            if (groundCount == 1)
            {
                float collisionVelocity = collision.relativeVelocity.y;
                if (collisionVelocity >= jumpingStats.StunTriggerVelocity)
                {
                    StartCoroutine(OnLandingStunned());
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGroundCollision(collision))
        {
            groundCount--;
        }
    }
    #endregion


    #region Unity Coroutines
    private IEnumerator OnLandingStunned()
    {
        isLandStunned = true;
        deltaXQueue = 0;
        yield return new WaitForSeconds(jumpingStats.StunDuration);
        isLandStunned = false;
    }
    #endregion


    private void ApplyJump()
    {

        Rigidbody.AddForce(Vector2.up * jumpingStats.JumpVelocity, ForceMode2D.Impulse);
        ClearJumpQueue();
    }

    private void ApplyMove(MoveStats stats, float deltaTime)
    {
        float currentXVelocity = Rigidbody.velocity.x;
        float targetXVelocity = deltaXQueue * stats.MaxVelocity;
        float newVelocity = currentXVelocity;
        if (currentXVelocity < targetXVelocity)
        {
            newVelocity = Mathf.Min(currentXVelocity + stats.Acceleration * deltaTime, targetXVelocity);
        }
        else if (currentXVelocity > targetXVelocity)
        {
            newVelocity = Mathf.Max(currentXVelocity - stats.Acceleration * deltaTime, targetXVelocity);
        }
        Rigidbody.velocity = new Vector2(newVelocity, Rigidbody.velocity.y);
        deltaXQueue = 0;
    }

    private void ClearJumpQueue()
    {
        jumpQueueTime = -1;
    }

    private bool IsGroundCollision(Collision2D collision)
    {
        return (groundCollision.value & (1 << collision.gameObject.layer)) != 0;
    }


    [Serializable]
    public struct MoveStats
    {
        [SerializeField]
        private float acceleration;
        [SerializeField]
        private float maxVelocity;


        public static MoveStats Default {
            get {
                return new MoveStats()
                {
                    acceleration = 25,
                    maxVelocity = 8
                };
            }
        }

        public float Acceleration {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public float MaxVelocity {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }
    }

    [Serializable]
    public struct JumpStats
    {
        [SerializeField]
        private float jumpReadyTime;
        [SerializeField]
        private float jumpVelocity;
        [SerializeField]
        private float stunDuration;
        [SerializeField]
        private float stunTriggerVelocity;


        public static JumpStats Default {
            get {
                return new JumpStats()
                {
                    jumpReadyTime = 0.5f,
                    jumpVelocity = 800,
                    stunDuration = 1,
                    StunTriggerVelocity = 10
                };
            }
        }

        public float JumpReadyTime {
            get { return jumpReadyTime; }
            set { jumpReadyTime = value; }
        }

        public float JumpVelocity {
            get { return jumpVelocity; }
            set { jumpVelocity = value; }
        }

        public float StunDuration {
            get { return stunDuration; }
            set { value = stunDuration; }
        }

        public float StunTriggerVelocity {
            get { return stunTriggerVelocity; }
            set { value = stunTriggerVelocity; }
        }
    }


    public enum UpdateMode
    {
        Override, Additive, Once
    }

    public enum RoboAttack
    {
        None = 0,
        Normal = 1,
        RocketPunch = 2
    }
}
