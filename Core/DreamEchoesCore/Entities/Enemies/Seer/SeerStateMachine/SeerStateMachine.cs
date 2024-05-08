using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;
using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;
using DreamEchoesCore.RingLib.StateMachine;
using HKMirror.Reflection;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerStateMachine : StateMachine
{
    public Config Config { get; }
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    public Vector2 Velocity
    {
        get
        {
            return rigidbody2D.velocity;
        }
        set
        {
            rigidbody2D.velocity = value;
        }
    }
    public RingLib.Animator Animator { get; private set; }
    public HeroActions InputActions;
    public SeerStateMachine() : base(typeof(Idle), new Dictionary<string, Type> {
        { "Control", typeof(ControlledIdle) } })
    {
        Config = new();
    }
    protected override void StateMachineStart()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = Config.GravityScale;
        var animation = gameObject.transform.Find("Animation");
        Animator = animation.GetComponent<RingLib.Animator>();
        InputActions = HeroController.instance.Reflect().inputHandler.inputActions;
    }
    protected override void StateMachineUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ReceiveMessage("Control");
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
        if (rigidbody2D.velocity.y > 0)
        {
            return false;
        }
        var bottomRays = new List<Vector2>
        {
            boxCollider2D.bounds.min,
            new Vector2(boxCollider2D.bounds.center.x, boxCollider2D.bounds.min.y),
            new Vector2(boxCollider2D.bounds.max.x, boxCollider2D.bounds.min.y)
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
}
