using System;
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
    public const int ID_ATTACK = 4;
    public const int ID_ROCKET_ATTACK = 5;
    #endregion

    #region Fields
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private PhysicalInteractor interactor;
    [SerializeField]
    private AttackVolume rightHand;
    [SerializeField]
    private AttackVolume leftHand;
    [SerializeField]
    private DamageVolume damageVolume;
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
    private float jumpReadyTime = 0.18f;
    [Header("Landing")]
    [SerializeField]
    private float landDuration = 0.35f;
    [SerializeField]
    private float landTriggerVelocity = 5;
    [SerializeField]
    [Header("Attacking")]
    private float attackDuration = 1.2f;
    [SerializeField]
    private float rocketAttackDuration = 2.5f;

    private HorizontalMotor standardMotor;
    private JumpMotor jumpMotor;
    private HorizontalMotor rootedMotor;

    private MoveManager root;
    private IMove lastUpdated;

    private ParallelMoveGroup standardMoves;
    private HorizontalMove jumpSetupMove;
    private SequentialMoveGroup jumpMove;
    private HorizontalMove landMove;
    private HorizontalMove attackMove;

    private int lastAttack;
    #endregion


    #region Properties
    public bool Initialized { get; private set; }

    public Rigidbody2D Rigidbody {
        get { return rigidBody; }
    }
    #endregion


    #region Events
    public event EventHandler OnMoveUpdated;
    #endregion


    #region Public Methods
    public void Attack()
    {
        AttackHelper(ID_ATTACK, attackDuration);
    }

    public void RocketAttack()
    {
        AttackHelper(ID_ROCKET_ATTACK, rocketAttackDuration);
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
        if (!TryFindPhysicalInteractor())
        {
            Debug.LogWarning("PhysicalInteractor not assinged nor found. Disabling");
            enabled = false;
            return;
        }

        if (!TryFindRigidbody())
        {
            Debug.LogWarning("Rigidbody2D not assinged nor found. Disabling");
            enabled = false;
            return;
        }

        if(leftHand == null || rightHand == null)
        {
            Debug.LogWarning("attack volume not assinged. Disabling");
            enabled = false;
            return; 
        }

        if(damageVolume == null)
        {
            Debug.LogWarning("damage volume not assinged. Disabling");
            enabled = false;
            return;
        }

        InitializeMotors();
        InitializeAndRegisterMoves();
        Initialized = true;
    }

    private void OnEnable()
    {
        interactor.OnLanding += Interactor_OnLanding;
        attackMove.OnMoveStart += AttackMove_OnMoveStart;
        attackMove.OnMoveEnd += AttackMove_OnMoveEnd;
        damageVolume.OnDamaged += DamageVolume_OnDamaged; ;
    }

    private void FixedUpdate()
    {
        standardMoves.Issue();
        lastUpdated = root.Update(Time.fixedDeltaTime);
        if(OnMoveUpdated != null)
        {
            OnMoveUpdated(lastUpdated, EventArgs.Empty);
        }
    }

    private void OnDisable()
    {
        interactor.OnLanding -= Interactor_OnLanding;
        attackMove.OnMoveStart -= AttackMove_OnMoveStart;
        attackMove.OnMoveEnd -= AttackMove_OnMoveEnd;
        damageVolume.OnDamaged -= DamageVolume_OnDamaged;
    }

    private void Reset()
    {
        TryFindPhysicalInteractor();
        TryFindRigidbody();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Initialized)
        {
            //Kind of hacky but in order to sync inspector variables and non serialized variables
            jumpSetupMove.Duration = jumpReadyTime;
            landMove.Duration = landDuration;
        }
    }
#endif
    #endregion


    #region Event Handlers
    private void AttackMove_OnMoveStart(object sender, EventArgs e)
    {
        IMove move = sender as IMove;

        if (move != null)
        {
            leftHand.IsEnabled = true;
        }
    }

    private void AttackMove_OnMoveEnd(object sender, EventArgs e)
    {
        IMove move = sender as IMove;

        if (move != null)
        {
            leftHand.IsEnabled = false;
        }
    }

    private void DamageVolume_OnDamaged(object sender, DamageVolume.DamageEventArgs e)
    {
        Debug.Log("damaged");
    }

    private void Interactor_OnLanding(object sender, LandingEventArgs eventArgs)
    {
        if (eventArgs.Velocity.y >= landTriggerVelocity)
        {
            landMove.Issue();
        }
    }
    #endregion


    #region Private Methods
    //Temporary solution for different types of attacking
    private void AttackHelper(int attackId, float duration)
    {
        if (!attackMove.Issued && lastUpdated.Id != attackId)
        {
            attackMove.SetId(attackId);
            attackMove.Duration = duration;
            attackMove.Issue();
        }
    }

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

        Func<bool> isGrounded = () => { return interactor.IsGrounded; };
        Func<bool> isNotGrounded = () => { return !interactor.IsGrounded; };


        root = new MoveManager();

        standardMoves = new ParallelMoveGroup();

        HorizontalMove groundMove = new HorizontalMove(ID_GROUND, standardMotor, groundMotorStats)
        {
            OnInRightCondition = isGrounded,
            OnPostMotorUpdate = ResetMotorDirection
        };

        HorizontalMove airMove = new HorizontalMove(ID_AIR, standardMotor, airMotorStats)
        {
            OnInRightCondition = isNotGrounded,
            OnPostMotorUpdate = ResetMotorDirection
        };

        jumpSetupMove = new HorizontalMove(ID_JUMP, standardMotor, groundMotorStats)
        {
            Duration = jumpReadyTime,
            OnInRightCondition = isGrounded,
            OnPostMotorUpdate = ResetMotorDirection
        };

        JumpMove jumpUpMove = new JumpMove(ID_JUMP, jumpMotor, jumpMotorStats)
        {
            Duration = 0,
            OnInRightCondition = isGrounded
        };

        jumpMove = new SequentialMoveGroup(ID_JUMP);

        landMove = new HorizontalMove(ID_LAND, rootedMotor, rootedMotorStats)
        {
            Duration = landDuration,
            OnInRightCondition = isGrounded,
            OnMotorSetup = null
        };

        attackMove = new HorizontalMove(ID_ATTACK, rootedMotor, rootedMotorStats)
        {
            OnInRightCondition = isGrounded,
            OnMotorSetup = null
        };

        //Register
        root.Register(standardMoves);
        standardMoves.Register(airMove);
        standardMoves.Register(groundMove);

        root.Register(jumpMove);
        jumpMove.Register(jumpSetupMove);
        jumpMove.Register(jumpUpMove);

        root.Register(attackMove);

        root.Register(landMove);
    }

    private bool TryFindPhysicalInteractor()
    {
        if (interactor == null)
        {
            interactor = GetComponent<PhysicalInteractor>();
        }
        return interactor != null;
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
