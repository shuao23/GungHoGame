using UnityEngine;

public class CharacterGamepadInput : MonoBehaviour
{
    [SerializeField]
    private Character character;
    [SerializeField]
    private float minimumX = 0.2f;


    private void Awake()
    {
        if (!TryFindCharacter())
        {
            Debug.LogWarning("Character not assigned nor found. Disabling");
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        float unitDeltaX = Input.GetAxis("Horizontal");
        if (Mathf.Abs(unitDeltaX) >= minimumX)
        {
            character.Move(unitDeltaX);
        }
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            character.Jump();
        }

        if (Input.GetButton("Fire1"))
        {
            character.Attack();
        }

        if (Input.GetButton("Fire2"))
        {
            character.RocketAttack();
        }
    }

    private void Reset()
    {
        TryFindCharacter();
    }


    private bool TryFindCharacter()
    {
        if (character == null)
        {
            character = GetComponent<Character>();
        }
        return character != null;
    }
}
