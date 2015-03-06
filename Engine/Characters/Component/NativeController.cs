using UnityEngine;
using System.Collections;
using Assets.Engine.ComputerGraphics.Bezier;
using UnityEditorInternal;
using Assets.Engine.AI.FiniteStateMachine;
using Assets.Engine.AI.FiniteStateMachine.States;
using Assets.Engine.SimpleArrow;
using Engine.Characters.Entity;
using Engine.ComputerGraphics.Component;

namespace Engine.Characters.Entity.Characters
{
    [RequireComponent(typeof(FSMEngine))]

    public class NativeController : MonoBehaviour
{
    #region FIELD

    public float MaxSpeed = 1f;
    public FSMEngine FSM;

    private int _state_Wave = Animator.StringToHash("base.Wave");
    private int _parameter_Speed = Animator.StringToHash("Speed");
    private int _parameter_Direction = Animator.StringToHash("Direction");
    private int _parameter_Wave = Animator.StringToHash("Wave");

    #endregion FIELD

    #region ATTRIBUTE

    internal IslandBehaviour Island
    {
        get
        {
            if (_island != null) return _island;
            _island = FindObjectOfType<IslandBehaviour>();
            return _island;
        }
    }
    private IslandBehaviour _island = null;

    internal BackwardPersonCamera BackwardPersoncamera { get; set; }
    
    internal bool IsIdle
    {
        get
        {
            if (Path == null) return true;
            if (Path.CheckPoints.Count == 0) return true;

            return false;
        }
    }
    
    internal float Speed
    {
        get { return Character.Speed; }
        set
        {
            if (Character.Speed != value)
            {
                Character.Speed = value;
                SetSpeed(value);
            }
        }
    }
    
    public Path Path
    {
        get { return path; }
        set
        {
            if (path != null) {
                GroundQuiver.RemovePath(path);
            }
            path = value;
        }
    }
    private Path path = null;

    public Vector3 LastCheckPoint
    {
        get { return lastCheckPoint; }
        set { lastCheckPoint = value; }
    }
    private Vector3 lastCheckPoint = Vector3.zero;

    private Animator Animator 
    {
        get {
            if (animator != null) return animator;
            animator = GetComponent<Animator>();
            return animator;
        } 
    }
    private Animator animator = null;
    
    public Character Character { get; set; }

    #endregion ATTRIBUTE

    #region CONSTRUCTOR

    internal NativeController(Character character)
    {
        this.Character = character;
    }

    void Start () 
    {
        SetFSMEngine();
        
        BackwardPersoncamera = FindObjectOfType<BackwardPersonCamera>();
        animator = GetComponent<Animator>();

        //Character = new Character();
	}

    #endregion CONSTRUCTOR

    void Update()
    {
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);

        //Autome
        if (FSM != null && FSM.CurrentState == null)
        {
            FSM.CurrentState.Update();
        }

        //GetKeyDown();        

    }

    private void SetFSMEngine()
    {
        FSM = new FSMEngine(this);

        IdleState idle = new IdleState(FSM);
        FollowPathState follow = new FollowPathState(FSM);
        FlipState flip = new FlipState(FSM);
        WaveState waveState = new WaveState(FSM);

        // idle -> follow -> flip -> wave -> idle
        idle.AddTransition(follow);
        follow.AddTransition(flip);
        flip.AddTransition(waveState);
        waveState.AddTransition(idle);

        FSM.SetInitialState(idle);
        
    }
	
    private void GetKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Character.Speed = 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Character.Speed = 1f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Path = GroundQuiver.CreateBezierPath(
                        Island, transform.position, 
                        "testPath");
        }
    }

    internal float GetDirectionNextCheckPoint()
    {
        Vector3 posTarget = Path.FirstCheckPoint;
        return GetDirection(posTarget);
    }

    internal float GetDirection(Vector3 posTarget)
    {
        float direction;

        posTarget.y = transform.position.y;

        Vector3 pointPos_Local = transform.InverseTransformPoint(posTarget);
        Vector3 charaPos_Local = transform.InverseTransformPoint(transform.position + transform.forward);

        Vector3 perp = Vector3.Cross(charaPos_Local, pointPos_Local);
        float dir = Vector3.Dot(perp, Vector3.up);
        float angle = Vector3.Angle(charaPos_Local, pointPos_Local);
        float radAngle = Mathf.Sin(Mathf.Deg2Rad * angle);

        if (angle < 90)
        {
            direction = radAngle * Mathf.Sign(dir);
        }
        else
        {
            direction = Mathf.Sign(dir);
        }

        return direction;
    }

    #region Animation

    internal void SetSpeed(float speed)
    {
        Animator.SetFloat(_parameter_Speed, speed);
    }

    internal void SetDirection(float direction)
    {
        Animator.SetFloat(_parameter_Direction, direction);
    }

    internal void SetDirection(Vector3 posTarget)
    {
        float dir = GetDirection(posTarget);
        Animator.SetFloat(_parameter_Direction, dir);
    }

    internal void Wave()
    {
        Animator.SetBool(_parameter_Wave, true);
    }

    internal bool IsWave()
    {
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
        return info.nameHash == _state_Wave;
    }

    #endregion Animation

    //internal void GoOnNextCheckPoint()
    //{
    //    Character.GoOnNextCheckPoint();
    //}

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (Character == null) return;
        if (Path == null) return;

        foreach (Vector3 point in Path.CheckPoints)
        {
            if (Vector3.Distance(point, transform.position) < 5)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(point, point + Vector3.up * 20);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(point, point + Vector3.up * 20);
            }
        }
    }

    #endregion Gizmos

    
    
    }
}