using HutongGames.PlayMaker.Actions;
using RingLib.Components;
using RingLib.StateMachine;
using RingLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    internal class StunEvent : RingLib.StateMachine.Event { }
    public bool dreamNailed = false;

    internal class DefeatedEvent : RingLib.StateMachine.Event { }

    public Config Config = new();
    private Vector2 originalBoxCollider2DOffset;
    private Vector2 originalBoxCollider2DSize;
    private SeerAnimator animator;
    public AudioSource speak;
    private InputManager inputManager;
    private List<GameObject> attacks = [];

    private int stunCount;

    private float minX;
    private float maxX;

    public GameObject traitorWave;

    WeaverCore.Components.EntityHealth entityHealth;
    private int fullHP;
    private int treeHP;
    private int shadowHP;

    public GameObject WaveTemplate;
    public TreeStateMachine treeStateMachine;

    public int IdleCount = 0;//listen to idle event

    public bool shadownCanOut = false;

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
        speak = animation.transform.Find("Speak").gameObject.GetComponent<AudioSource>();
        inputManager = gameObject.AddComponent<InputManager>();
        foreach (var attack in gameObject.GetComponentsInChildren<RingLib.Attacks.Attack>(true))
        {
            attacks.Add(attack.gameObject);
            RingLib.Log.LogInfo(GetType().Name, $"Attack {attack.name} discovered");
        }
        entityHealth = gameObject.GetComponent<WeaverCore.Components.EntityHealth>();
        entityHealth.OnHealthChangeEvent += OnHit;
        entityHealth.OnDeathEvent += OnDeath;
        fullHP = entityHealth.Health;
        treeHP = fullHP / 3 * 2;
        shadowHP = fullHP / 3;
        //shadowHP = fullHP - 1;
        startMusic = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("StartMusic");
        musicCue = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("DreamEchoesSeerMusicCue");
        emptyMusic = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("EmptyMusic");

        minX = float.MinValue;
        maxX = float.MaxValue;
        var col2d = gameObject.GetComponent<BoxCollider2D>();
        var leftRays = new List<Vector2>();
        leftRays.Add(col2d.bounds.min);
        leftRays.Add(new Vector2(col2d.bounds.min.x, col2d.bounds.center.y));
        leftRays.Add(new Vector2(col2d.bounds.min.x, col2d.bounds.max.y));
        for (int l = 0; l < 3; l++)
        {
            RaycastHit2D raycastHit2D4 = Physics2D.Raycast(leftRays[l], -Vector2.right, float.MaxValue, 1 << 8);
            if (raycastHit2D4.collider != null)
            {
                minX = Mathf.Max(minX, raycastHit2D4.point.x);
            }
        }
        minX += col2d.size.x * 2;
        var rightRays = new List<Vector2>();
        rightRays.Add(col2d.bounds.max);
        rightRays.Add(new Vector2(col2d.bounds.max.x, col2d.bounds.center.y));
        rightRays.Add(new Vector2(col2d.bounds.max.x, col2d.bounds.min.y));
        for (int j = 0; j < 3; j++)
        {
            RaycastHit2D raycastHit2D2 = Physics2D.Raycast(rightRays[j], Vector2.right, float.MaxValue, 1 << 8);
            if (raycastHit2D2.collider != null)
            {
                maxX = Mathf.Min(maxX, raycastHit2D2.point.x);
            }
        }
        maxX -= col2d.size.x * 2;

        LogInfo(GetType().Name, $"minX: {minX}, maxX: {maxX}");

        var battleScene = DreamEchoesCore.GetPreloaded("GG_Traitor_Lord", "Battle Scene");
        var traitorLord = battleScene.transform.Find("Wave 3").gameObject.transform.Find("Mantis Traitor Lord").gameObject;
        var fsm = traitorLord.LocateMyFSM("Mantis");
        var state = fsm.GetState("Waves");
        var action = state.GetAction<SpawnObjectFromGlobalPool>(0);
        traitorWave = action.gameObject.Value;

        var mageLord = DreamEchoesCore.GetPreloaded("GG_Soul_Master", "Mage Lord");
        var mageState = mageLord.LocateMyFSM("Mage Lord").GetState("Quake Waves");
        WaveTemplate = mageState.GetAction<SpawnObjectFromGlobalPool>(0).gameObject.Value;

        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.name == "Tree")
            {
                var treeanim = root.transform.Find("Animation");
                treeStateMachine = treeanim.GetComponent<TreeStateMachine>();
            }
        }
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

    private int previousStunCount;
    private void LockStun()
    {
        previousStunCount = stunCount;
        stunCount = int.MinValue;
    }

    private void UnlockStun()
    {
        stunCount = previousStunCount;
    }

    public void SetHalfCollider()
    {
        BoxCollider2D.offset = Config.StunColliderOffset;
        BoxCollider2D.size = Config.StunColliderSize;
    }

    public void SetFullCollider()
    {
        BoxCollider2D.offset = originalBoxCollider2DOffset;
        BoxCollider2D.size = originalBoxCollider2DSize;
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
        animator.LaserHit.SetActive(false);
        animator.StopRunSound();
    }
}
