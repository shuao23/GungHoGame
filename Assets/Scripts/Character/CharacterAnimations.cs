using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Character character;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private CharacterFacing facing;
    [SerializeField]
    private SmoothVerticalFactor fallingFactor;

    private AnimationHash hash = new AnimationHash();
    #endregion


    #region Unity Methods
    private void Awake()
    {
        if (character == null)
        {
            Debug.LogWarning("Character not assigned. Disabling");
            enabled = false;
            return;
        }

        if (!TryFindAnimator())
        {
            Debug.LogWarning("Animator not assigned nor found. Disabling");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        fallingFactor.Initialize(character.Rigidbody.velocity.y);
    }

    private void OnEnable()
    {
        character.OnMoveUpdated += Character_OnMoveUpdated;
    }

    private void Update()
    {
        UpdateFacing(Time.deltaTime);
        UpdateAnimatorParams(Time.deltaTime);
    }

    private void OnDisable()
    {
        character.OnMoveUpdated -= Character_OnMoveUpdated;
    }

    private void Reset()
    {
        TryFindAnimator();
    }
    #endregion

    #region Event Handlers
    private void Character_OnMoveUpdated(object sender, System.EventArgs e)
    {
        IMove move = (IMove)sender;
        animator.SetInteger(hash.State, move.Id);
    }

    #endregion


    #region Private Methods
    private void UpdateAnimatorParams(float deltaTime)
    {
        animator.SetFloat(hash.WalkSpeed, Mathf.Abs(character.Rigidbody.velocity.x));

        fallingFactor.Update(deltaTime, character.Rigidbody.velocity.y);
        animator.SetFloat(hash.Vertical, fallingFactor.NormalizedVertical);
    }

    private void UpdateFacing(float deltaTime)
    {
        facing.Update(deltaTime, character.Rigidbody.velocity.x);
        transform.eulerAngles = new Vector3(0, facing.FaceAngle, 0);
    }

    private bool TryFindAnimator()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        return animator != null;
    }
    #endregion


    #region Nested Classes
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
    #endregion
}
