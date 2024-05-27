using RingLib.Components;
using RingLib.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    internal class StunEvent : RingLib.StateMachine.Event { }

    internal class DefeatedEvent : RingLib.StateMachine.Event { }

    public Config Config = new();
    private Vector2 originalBoxCollider2DOffset;
    private Vector2 originalBoxCollider2DSize;
    private SeerAnimator animator;
    private InputManager inputManager;
    private List<GameObject> attacks = [];

    private int stunCount;

    public SeerStateMachine() : base(
        startState: nameof(Wake),
        globalTransitions: new Dictionary<Type, string>
        {
            { typeof(StunEvent), nameof(Stun) },
            { typeof(DefeatedEvent), nameof(Defeated) }
        },
        spriteFacingLeft: true)
    { }

    protected override void EntityStateMachineStart()
    {
        originalBoxCollider2DOffset = BoxCollider2D.offset;
        originalBoxCollider2DSize = BoxCollider2D.size;
        Rigidbody2D.gravityScale = Config.GravityScale;
        var animation = gameObject.transform.Find("Animation");
        animator = animation.GetComponent<SeerAnimator>();
        inputManager = gameObject.AddComponent<InputManager>();
        foreach (var attack in gameObject.GetComponentsInChildren<RingLib.Attacks.Attack>(true))
        {
            attacks.Add(attack.gameObject);
            RingLib.Log.LogInfo(GetType().Name, $"Attack {attack.name} discovered");
        }
        var entityHealth = gameObject.GetComponent<WeaverCore.Components.EntityHealth>();
        entityHealth.OnHealthChangeEvent += OnHit;
        entityHealth.OnDeathEvent += OnDeath;
        startMusic = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("StartMusic");
        musicCue = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("DreamEchoesSeerMusicCue");
    }

    protected override void EntityStateMachineUpdate()
    {
        var attackPressed = inputManager.AttackPressed;
        var controlldFree = CurrentState == nameof(ControlledIdle) || CurrentState == nameof(ControlledRun);
        if (attackPressed && controlldFree)
        {
            SetState(nameof(ControlledSlash));
        }
    }

    private GameObject Target()
    {
        return HeroController.instance.gameObject;
    }

    private bool FacingTarget()
    {
        return Mathf.Sign(Target().transform.position.x - transform.position.x) == Direction();
    }

    private IEnumerator<Transition> Turn()
    {
        var localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        gameObject.transform.localScale = localScale;
        yield return new NoTransition();
    }

    private void OnHit(int previousHealth, int newHealth)
    {
        ++stunCount;
        if (stunCount >= Config.StunThreshold)
        {
            stunCount = int.MinValue;
            ReceiveEvent(new StunEvent());
        }
    }

    private void OnDeath(HitInfo hitInfo)
    {
        ReceiveEvent(new DefeatedEvent());
    }

    private void Reset()
    {
        BoxCollider2D.offset = originalBoxCollider2DOffset;
        BoxCollider2D.size = originalBoxCollider2DSize;
        Rigidbody2D.gravityScale = Config.GravityScale;
        Velocity = Vector2.zero;
        foreach (var attack in attacks)
        {
            attack.SetActive(false);
        }
    }
}
