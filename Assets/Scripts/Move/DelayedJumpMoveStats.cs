using System;
using UnityEngine;

[Serializable]
public class DelayedJumpMoveStats
{
    [SerializeField]
    private JumpMove.MoveStats jumpMoveStats;

    [SerializeField]
    private MoveClip clip;
    

    public JumpMove.MoveStats JumpMoveStats {
        get { return jumpMoveStats; }
        set { jumpMoveStats = value; }
    }

    public MoveClip Clip {
        get { return clip; }
        set { clip = value; }
    }
}
