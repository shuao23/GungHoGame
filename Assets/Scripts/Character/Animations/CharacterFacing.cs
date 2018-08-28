using System;
using UnityEngine;

[Serializable]
public class CharacterFacing
{
    [SerializeField]
    private float speed = 800;

    private float rawFacingAngle;

    public float Speed {
        get { return speed; }
    }

    public float FaceAngle {
        get {
            return Interpolate.EaseInOut(-90, 90, MyMath.Normalize(rawFacingAngle, -90, 90));
        }
    }

    public bool IsFacingRight { get; private set; }


    public CharacterFacing()
    {
        IsFacingRight = true;
    }


    public void Update(float deltaTime, float velocityX)
    {
        if (velocityX < 0)
        {
            IsFacingRight = true;
        }
        else if (velocityX > 0)
        {
            IsFacingRight = false;
        }


        rawFacingAngle = IsFacingRight ?
            Mathf.Max(rawFacingAngle - speed * deltaTime, -90) :
            Mathf.Min(rawFacingAngle + speed * deltaTime, 90);
    }
}
