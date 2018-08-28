using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour {

    [SerializeField]
    private Character character;
    [SerializeField]
    private Animator animator;

	private void Awake () {
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


    private bool TryFindAnimator()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        return animator != null;
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
}
