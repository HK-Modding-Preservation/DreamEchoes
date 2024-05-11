using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;
using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;
using RingLib.Attacks;
using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerStateMachine : StateMachine
{
    public Config Config { get; }
    public BoxCollider2D BoxCollider2D;
    public Vector2 OriginalBoxCollider2DOffset;
    public Vector2 OriginalBoxCollider2DSize;
    public Rigidbody2D Rigidbody2D;
    public Vector2 Velocity
    {
        get
        {
            return Rigidbody2D.velocity;
        }
        set
        {
            Rigidbody2D.velocity = value;
        }
    }
    public RingLib.Animator Animator { get; private set; }
    public RingLib.InputManager InputManager;
    public SeerStateMachine() : base(typeof(Idle), [])
    {
        Config = new();
    }
    protected override void StateMachineStart()
    {
        BoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        OriginalBoxCollider2DOffset = BoxCollider2D.offset;
        OriginalBoxCollider2DSize = BoxCollider2D.size;
        Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        Rigidbody2D.gravityScale = Config.GravityScale;
        var animation = gameObject.transform.Find("Animation");
        Animator = animation.GetComponent<RingLib.Animator>();
        InputManager = gameObject.AddComponent<RingLib.InputManager>();
    }
    protected override void StateMachineUpdate()
    {
        var attackPressed = InputManager.AttackPressed;
        var controlldFree = CurrentState == typeof(ControlledIdle).Name || CurrentState == typeof(ControlledRun).Name;
        if (attackPressed && controlldFree)
        {
            SetState(typeof(ControlledSlash));
        }
    }
    public GameObject Target()
    {
        return HeroController.instance.gameObject;
    }
    public float Direction()
    {
        var direction = Mathf.Sign(gameObject.transform.localScale.x);
        return Config.SpriteFacingLeft ? -direction : direction;
    }
    public bool FacingTarget()
    {
        return Mathf.Sign(Target().transform.position.x - transform.position.x) == Direction();
    }
    public void Turn()
    {
        var localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
    }
    public bool Landed()
    {
        if (Rigidbody2D.velocity.y > 0)
        {
            return false;
        }
        var bottomRays = new List<Vector2>
        {
            BoxCollider2D.bounds.min,
            new Vector2(BoxCollider2D.bounds.center.x, BoxCollider2D.bounds.min.y),
            new Vector2(BoxCollider2D.bounds.max.x, BoxCollider2D.bounds.min.y)
        };
        for (var k = 0; k < 3; k++)
        {
            RaycastHit2D raycastHit2D3 = Physics2D.Raycast(bottomRays[k], -Vector2.up, 0.05f, 1 << 8);
            if (raycastHit2D3.collider != null)
            {
                return true;
            }
        }
        return false;
    }
    public void Reset()
    {
        BoxCollider2D.offset = OriginalBoxCollider2DOffset;
        BoxCollider2D.size = OriginalBoxCollider2DSize;
        Rigidbody2D.gravityScale = Config.GravityScale;
        foreach (var attack in gameObject.GetComponentsInChildren<Attack>())
        {
            attack.gameObject.SetActive(false);
        }
    }
}
