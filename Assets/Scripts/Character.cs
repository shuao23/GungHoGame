using System;
using UnityEngine;

using HorizontalMove = StandardMove<HorizontalMotor, HorizontalMotorStats>;
using JumpMove = StandardMove<JumpMotor, JumpMotorStats>;

public class Character : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Foot foot;
    [SerializeField]
    private HorizontalMotorStats groundMotorStats = new HorizontalMotorStats()
    {
        Acceleration = 30,
        MaxVelocity = 6
    };
    [SerializeField]
    private HorizontalMotorStats rootedMotorStats = new HorizontalMotorStats()
    {
        Acceleration = 50,
        MaxVelocity = 0
    };
    [SerializeField]
    private HorizontalMotorStats airMotorStats = new HorizontalMotorStats()
    {
        Acceleration = 5,
        MaxVelocity = 8
    };
    [SerializeField]
    private DelayedJumpMoveStats jumpMoveStats = new DelayedJumpMoveStats() { };
    #endregion


    #region Properties
    private MoveManager Root { get; set; }

    private IMove JumpMove { get; set; }
    private IMove StandardMoves { get; set; }

    public IMove CurrentMove {
        get {
            return Root.BestCandidate;
        }
    }


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
        StandardMoves.Issue();
        Root.Update(Time.fixedDeltaTime);
        Debug.Log("Current Move: " + CurrentMove.Name);
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
        //Initialize
        Action<HorizontalMotor, HorizontalMotorStats> OnHoriMotorSetup = 
            (HorizontalMotor motor, HorizontalMotorStats stats) =>
        {
            motor.Stats = stats;
        };

        Action<HorizontalMotor, HorizontalMotorStats> OnPostHoriMotorUpdate = 
            (HorizontalMotor motor, HorizontalMotorStats stats) =>
        {
            motor.Direction = 0;
        };

        MoveManager root = new MoveManager();

        ParallelMoveGroup standardMoves = new ParallelMoveGroup();

        HorizontalMove groundMove = new HorizontalMove("ground", StandardMotor, groundMotorStats)
        {
            Continous = true,
            OnInRightCondition = () => { return foot.IsGrounded; },
            OnMotorSetup = OnHoriMotorSetup,
            OnPostMotorUpdate = OnPostHoriMotorUpdate
        };

        HorizontalMove airMove = new HorizontalMove("air", StandardMotor, airMotorStats)
        {
            Continous = true,
            OnInRightCondition = () => { return !foot.IsGrounded; },
            OnMotorSetup = OnHoriMotorSetup,
            OnPostMotorUpdate = OnPostHoriMotorUpdate
        };

        JumpMove jumpMove = new JumpMove("jump", JumpMotor, jumpMoveStats.JumpMotorStats)
        {
            OnInRightCondition = () => { return foot.IsGrounded; }
        };

        SequentialMoveGroup delayedJumpMove = new SequentialMoveGroup("jump");

        //Register
        root.Register(standardMoves);
        standardMoves.Register(airMove);
        standardMoves.Register(groundMove);

        root.Register(delayedJumpMove);
        delayedJumpMove.Register(jumpMoveStats.GetStandardMoveClip(groundMove));
        delayedJumpMove.Register(jumpMoveStats.GetJumpMoveClip(jumpMove));

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
