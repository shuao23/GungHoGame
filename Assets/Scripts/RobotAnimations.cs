using UnityEngine;

public class RobotAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Robot robot;
    [SerializeField]
    private Transform meshTransform;
    [SerializeField]
    private float normalizedVerticalSmoothing = 0.75f;
    [SerializeField]
    private float normalizedVerticalDamping = 1f;
    [SerializeField]
    private float facingSpeed;

    private bool isFacingRight = true;
    private float rotY;
    private AnimationHash hash = new AnimationHash();


    public Transform MeshTransform {
        get {
            return meshTransform == null ? transform : meshTransform;
        }
    }


    private void Awake()
    {
        if (!TryFindAnimator())
        {
            Debug.LogWarning("Rigidbody not assigned nor found. Disabling");
            enabled = false;
            return;
        }

        if (robot == null)
        {
            Debug.LogWarning("Robot not assigned. Disabling");
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat(hash.WalkSpeed, Mathf.Abs(robot.Rigidbody.velocity.x));
        animator.SetFloat(hash.Vertical, ComputeNormalizedVertical(
            animator.GetFloat(hash.Vertical), Time.fixedDeltaTime));
    }

    private void Update()
    {
        animator.SetInteger(hash.State, (int)ComputeRobotState());
        UpdateFacing(Time.deltaTime);
    }

    private void Reset()
    {
        TryFindAnimator();
    }


    private float ComputeNormalizedVertical(float currentVertical, float deltaTime)
    {
        float vertTarget = SmoothNormalize(robot.Rigidbody.velocity.y, normalizedVerticalSmoothing);
        return Mathf.Lerp(currentVertical, vertTarget, deltaTime * normalizedVerticalDamping);
    }

    private RobotState ComputeRobotState() {
        if (robot.IsGrounded)
        {
            if (robot.JumpQueued)
            {
                return RobotState.Jumping;
            }

            if (robot.IsLandStunned)
            {
                return RobotState.Landing;
            }

            return RobotState.Locomotion;
        }
        else
        {
            return RobotState.Air;
        }
    }

    private float SmoothNormalize(float value, float pow)
    {
        float normalized;
        if(value < 0)
        {
            normalized = 1 / Mathf.Pow(-value + 1, pow) - 1;
        }
        else
        {
            normalized =  - 1 / Mathf.Pow(value + 1, pow) + 1;
        }

        return normalized / 2 + 0.5f;
    }

    private bool TryFindAnimator()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        return animator != null;
    }

    private void UpdateFacing(float deltaTime)
    {
        float velX = robot.Rigidbody.velocity.x;
        if (velX < 0)
        {
            isFacingRight = true;
        }
        else if(velX > 0)
        {
            isFacingRight = false;
        }

        rotY = isFacingRight ? 
            Mathf.Max(rotY - facingSpeed * deltaTime, -90) :
            Mathf.Min(rotY + facingSpeed * deltaTime, 90);
        float angleY = Interpolate.EaseInOut(-90, 90, MyMath.Normalize(rotY, -90, 90));
        transform.eulerAngles = new Vector3(0, angleY, 0);
    }


    private static class AnimationParameters
    {
        public const string WALK_SPEED = "walk speed";
        public const string STATE = "state";
        public const string VERTICAL = "normalized vertical";
    }

    private class AnimationHash
    {

        public int WalkSpeed { get; private set; }
        public int State { get; private set; }
        public int Vertical { get; private set; }

        public AnimationHash()
        {
            WalkSpeed = Animator.StringToHash(AnimationParameters.WALK_SPEED);
            State = Animator.StringToHash(AnimationParameters.STATE);
            Vertical = Animator.StringToHash(AnimationParameters.VERTICAL);
        }
    }

    private enum RobotState
    {
        Locomotion = 0,
        Air = 1,
        Jumping = 2,
        Landing = 3,
        Attacking = 4
    }
}
