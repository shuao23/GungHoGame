using UnityEngine;

public class RobotAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody2D robotRigidbody;

    private AnimationHash hash = new AnimationHash();

    private void Awake()
    {
        if (!TryFindAnimator())
        {
            Debug.LogWarning("Rigidbody not assigned nor found. Disabling");
            enabled = false;
        }

        if(robotRigidbody == null)
        {
            Debug.LogWarning("Rigidbody2D not assigned. Disabling");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat(hash.WalkSpeed, Mathf.Abs(robotRigidbody.velocity.x));
    }

    private void Reset()
    {
        TryFindAnimator();
    }


    private bool TryFindAnimator()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
        return animator != null;
    }


    private static class AnimationParameters
    {
        public const string WALK_SPEED = "walk speed";
        public const string IS_JUMPING = "isPunch";
        public const string IS_PUNCHING = "isJump";
    }

    private class AnimationHash {

        public int WalkSpeed { get; private set; }
        public int IsJumping { get; private set; }
        public int IsPunching { get; private set; }

        public AnimationHash()
        {
            WalkSpeed = Animator.StringToHash(AnimationParameters.WALK_SPEED);
            IsJumping = Animator.StringToHash(AnimationParameters.IS_JUMPING);
            IsPunching = Animator.StringToHash(AnimationParameters.IS_PUNCHING);
        }
    }
}
