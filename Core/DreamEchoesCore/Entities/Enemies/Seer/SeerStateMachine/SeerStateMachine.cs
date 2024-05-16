using HKMirror.Reflection;
using RingLib.Components;
using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    public Config Config { get; }
    public Vector2 OriginalBoxCollider2DOffset;
    public Vector2 OriginalBoxCollider2DSize;
    public RingLib.Components.Animator Animator { get; private set; }
    public InputManager InputManager { get; private set; }
    private List<GameObject> attacks = [];

    public SeerStateMachine() : base(
        nameof(Idle),
        new Dictionary<string, string>
        {
            { "Stun", nameof(Stun) }
        },
        /*SpriteFacingLeft =*/true)
    {
        Config = new();
    }

    protected override void EntityStateMachineStart()
    {
        OriginalBoxCollider2DOffset = BoxCollider2D.offset;
        OriginalBoxCollider2DSize = BoxCollider2D.size;
        Rigidbody2D.gravityScale = Config.GravityScale;
        var animation = gameObject.transform.Find("Animation");
        Animator = animation.GetComponent<RingLib.Components.Animator>();
        InputManager = gameObject.AddComponent<InputManager>();
        InputManager.HeroActions = HeroController.instance.Reflect().inputHandler.inputActions;
        foreach (var attack in gameObject.GetComponentsInChildren<RingLib.Attacks.Attack>(true))
        {
            attacks.Add(attack.gameObject);
            RingLib.Log.LogInfo(GetType().Name, $"Attack {attack.name} discovered");
        }
        var entityHealth = gameObject.GetComponent<WeaverCore.Components.EntityHealth>();
        entityHealth.OnHealthChangeEvent += OnHit;
        entityHealth.OnDeathEvent += OnDeath;
    }

    protected override void EntityStateMachineUpdate()
    {
        var attackPressed = InputManager.AttackPressed;
        var controlldFree = CurrentState == nameof(ControlledIdle) || CurrentState == nameof(ControlledRun);
        if (attackPressed && controlldFree)
        {
            SetState(nameof(ControlledSlash));
        }
    }

    public GameObject Target()
    {
        return HeroController.instance.gameObject;
    }

    public bool FacingTarget()
    {
        return Mathf.Sign(Target().transform.position.x - transform.position.x) == Direction();
    }

    public IEnumerator<Transition> Turn()
    {
        var localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
        yield return new NoTransition();
    }

    public void OnHit(int previousHealth, int newHealth)
    {
        ReceiveEvent("Stun");
    }

    public void OnDeath(WeaverCore.HitInfo hitInfo)
    {
    }

    public void Reset()
    {
        BoxCollider2D.offset = OriginalBoxCollider2DOffset;
        BoxCollider2D.size = OriginalBoxCollider2DSize;
        Rigidbody2D.gravityScale = Config.GravityScale;
        foreach (var attack in attacks)
        {
            attack.SetActive(false);
        }
    }
}
