using System;
using UnityEngine;

[Serializable]
public class DelayedJumpMoveStats
{
    [SerializeField]
    private JumpMotorStats jumpMotorStats = new JumpMotorStats()
    {
        Velocity = 800f
    };
    [SerializeField]
    private MoveClip standardMoveClip = new MoveClip()
    {
        Duration = 0.18f
    };

    private MoveClip jumpMoveClip = new MoveClip()
    {
        Duration = 0
    };


    public JumpMotorStats JumpMotorStats {
        get { return jumpMotorStats; }
        set {
            if(value == null)
            {
                throw new ArgumentNullException("JumpMotorStats");
            }

            jumpMotorStats = value;
        }
    }


    public MoveClip GetStandardMoveClip(IMove move)
    {
        standardMoveClip.Move = move;
        return standardMoveClip;
    }

    public MoveClip GetJumpMoveClip(IMove move)
    {
        jumpMoveClip.Move = move;
        return jumpMoveClip;
    }
}
