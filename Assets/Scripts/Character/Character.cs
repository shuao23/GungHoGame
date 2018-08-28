﻿using System;
using UnityEngine;

using HorizontalMove = StandardMove<HorizontalMotor, HorizontalMotorStats>;
using JumpMove = StandardMove<JumpMotor, JumpMotorStats>;

public class Character : MonoBehaviour
{
    #region Constants
    public const int ID_GROUND = 0;
    public const int ID_AIR = 1;
    public const int ID_JUMP = 2;
    public const int ID_LAND = 3;
    #endregion

    #region Fields
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Foot foot;
    [Header("Horizontal Movement")]
    [SerializeField]
    private HorizontalMotorStats groundMotorStats = new HorizontalMotorStats()
    {
        Acceleration = 30,
        MaxVelocity = 6
    };
    [SerializeField]
    private HorizontalMotorStats airMotorStats = new HorizontalMotorStats()
    {
        Acceleration = 5,
        MaxVelocity = 8
    };
    [SerializeField]
    private HorizontalMotorStats rootedMotorStats = new HorizontalMotorStats()
    {
        Acceleration = 50,
        MaxVelocity = 0
    };
    [Header("Jumping")]
    [SerializeField]
    private JumpMotorStats jumpMotorStats = new JumpMotorStats()
    {
        Velocity = 800
    };
    [SerializeField]
    [Tooltip("Edit during playtime has no effect")]
    private float jumpReadyTime = 0.18f;
    [Header("Landing")]
    [SerializeField]
    [Tooltip("Edit during playtime has no effect")]
    private float landDuration = 0.35f;
    [SerializeField]
    private float landTriggerVelocity = 5;

    private HorizontalMotor standardMotor;
    private JumpMotor jumpMotor;
    private HorizontalMotor rootedMotor;

    private MoveManager root;
    private IMove lastUpdated;

    private ParallelMoveGroup standardMoves;
    private SequentialMoveGroup jumpMove;
    private HorizontalMove landMove;
    #endregion


    #region Properties
    public bool Initialized { get; private set; }
    #endregion


    #region Events
    public 
    #endregion


    #region Public Methods
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public void Jump()
    {
        jumpMove.Issue();
    }

    public void Move(float direction)
    {
        standardMotor.Direction = direction;
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
        Initialized = true;
    }

    private void OnEnable()
    {
        foot.OnLanding += OnLanding;
    }

    private void FixedUpdate()
    {
        standardMoves.Issue();
        lastUpdated = root.Update(Time.fixedDeltaTime);
    }

    private void OnDisable()
    {
        foot.OnLanding -= OnLanding;
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
        standardMotor = new HorizontalMotor(rigidBody);
        jumpMotor = new JumpMotor(rigidBody);
        rootedMotor = new HorizontalMotor(rigidBody, rootedMotorStats);
    }

    private void InitializeAndRegisterMoves()
    {
        //Initialize
        Action<HorizontalMotor, HorizontalMotorStats> ResetMotorDirection =
            (HorizontalMotor motor, HorizontalMotorStats stats) =>
        {
            motor.Direction = 0;
        };

        Func<bool> IsGrounded = () => { return foot.IsGrounded; };
        Func<bool> IsNotGrounded = () => { return !foot.IsGrounded; };


        root = new MoveManager();

        standardMoves = new ParallelMoveGroup();

        HorizontalMove groundMove = new HorizontalMove(ID_GROUND, standardMotor, groundMotorStats)
        {
            OnInRightCondition = IsGrounded,
            OnPostMotorUpdate = ResetMotorDirection
        };

        HorizontalMove airMove = new HorizontalMove(ID_AIR, standardMotor, airMotorStats)
        {
            OnInRightCondition = IsNotGrounded,
            OnPostMotorUpdate = ResetMotorDirection
        };

        HorizontalMove jumpSetupMove = new HorizontalMove(ID_JUMP, standardMotor, groundMotorStats)
        {
            Duration = jumpReadyTime,
            OnInRightCondition = IsGrounded,
            OnPostMotorUpdate = ResetMotorDirection
        };

        JumpMove jumpUpMove = new JumpMove(ID_JUMP, jumpMotor, jumpMotorStats)
        {
            Duration = 0,
            OnInRightCondition = IsGrounded
        };

        jumpMove = new SequentialMoveGroup(ID_JUMP);

        landMove = new HorizontalMove(ID_LAND, rootedMotor, rootedMotorStats)
        {
            Duration = landDuration,
            OnInRightCondition = IsGrounded,
            OnMotorSetup = null
        };

        //Register
        root.Register(standardMoves);
        standardMoves.Register(airMove);
        standardMoves.Register(groundMove);

        root.Register(jumpMove);
        jumpMove.Register(jumpSetupMove);
        jumpMove.Register(jumpUpMove);

        root.Register(landMove);
    }

    private void OnLanding(object sender, Foot.LandingEventArgs eventArgs)
    {
        if (eventArgs.Velocity.y >= landTriggerVelocity)
        {
            landMove.Issue();
        }
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