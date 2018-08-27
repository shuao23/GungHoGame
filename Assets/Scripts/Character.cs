using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Foot foot;
    [SerializeField]
    private StandardMove.MoveStats groundMoveStats = new StandardMove.MoveStats()
    {
        Acceleration = 30,
        MaxVelocity = 6
    };
    [SerializeField]
    private StandardMove.MoveStats rootedMoveStats = new StandardMove.MoveStats()
    {
        Acceleration = 50,
        MaxVelocity = 0
    };
    [SerializeField]
    private StandardMove.MoveStats airMoveStats = new StandardMove.MoveStats()
    {
        Acceleration = 5,
        MaxVelocity = 8
    };
    [SerializeField]
    private DelayedJumpMoveStats jumpMoveStats = new DelayedJumpMoveStats()
    {
        Clip = new MoveClip()
        {
            Duration = 0.18f,
        },
        JumpMoveStats = new JumpMove.MoveStats()
        {
            Velocity = 800,
        }
    };
    #endregion


    #region Properties
    private MoveManager Root { get; set; }

    public IMove CurrentMove {
        get {
            return Root.BestCandidate;
        }
    }


    private IMove JumpMove { get; set; }

    private IMove StandardMoves { get; set; }


    public JumpMotor JumpMotor { get; set; }

    public HorizontalMotor StandardMotor { get; set; }
    #endregion


    #region Public Methods
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public void Jump()
    {
        JumpMove.Issue();
    }

    public void Move(float direction)
    {
        StandardMotor.Direction = direction;
        StandardMoves.Issue();
    }
    #endregion


    #region Unity Methods
    private void Awake()
    {
        if (!TryFindFoot())
        {
            Debug.LogWarning("Foot not assinged nor found. Disabling");
            enabled = false;
            return;
        }

        if (!TryFindRigidbody())
        {
            Debug.LogWarning("Rigidbody2D not assinged nor found. Disabling");
            enabled = false;
            return;
        }

        InitializeMotors();
        InitializeAndRegisterMoves();
    }

    private void FixedUpdate()
    {
        Root.Update(Time.fixedDeltaTime);
        Debug.Log(CurrentMove.Name);
    }


    private void Reset()
    {
        TryFindFoot();
        TryFindRigidbody();
    }
    #endregion


    #region Private Methods
    private void InitializeMotors()
    {
        StandardMotor = new HorizontalMotor(rigidBody);
        JumpMotor = new JumpMotor(rigidBody);
    }

    private void InitializeAndRegisterMoves()
    {
        MoveManager root = new MoveManager();

        ParallelMoveGroup<StandardMove> standardMoves = new ParallelMoveGroup<StandardMove>();

        StandardMove groundMove = new StandardMove("ground", groundMoveStats, StandardMotor)
        {
            OnInRightCondition = () => { return foot.IsGrounded; }
        };

        StandardMove airMove = new StandardMove("air", airMoveStats, StandardMotor)
        {
            OnInRightCondition = () => { return !foot.IsGrounded; }
        };

        JumpMove jumpMove = new JumpMove("jump", jumpMoveStats.JumpMoveStats, JumpMotor);

        SequentialMoveGroup delayedJumpMove = new SequentialMoveGroup("jump");


        MoveClip jumpMoveClip = jumpMoveStats.Clip;
        jumpMoveClip.Move = groundMove;
        MoveClip jumpClip = new MoveClip()
        {
            Duration = 0,
            Move = jumpMove
        };


        root.Register(standardMoves);
        standardMoves.Register(groundMove);
        standardMoves.Register(airMove);

        root.Register(delayedJumpMove);
        delayedJumpMove.Register(jumpMoveClip);
        delayedJumpMove.Register(jumpClip);

        Root = root;
        StandardMoves = standardMoves;
        JumpMove = delayedJumpMove;
    }

    private bool TryFindFoot()
    {
        if (foot == null)
        {
            foot = GetComponent<Foot>();
        }
        return foot != null;
    }

    private bool TryFindRigidbody()
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
        return rigidBody != null;
    }
    #endregion
}
