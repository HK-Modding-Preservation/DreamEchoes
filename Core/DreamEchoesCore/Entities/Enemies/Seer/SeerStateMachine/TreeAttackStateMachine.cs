using HutongGames.PlayMaker.Actions;
using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class OrbMod : MonoBehaviour
{
    private DamageHero damageHero;
    private tk2dSprite tk2DSprite;
    private PlayMakerFSM fsm;
    private bool update = false;
    private void Start()
    {
        damageHero = GetComponentInChildren<DamageHero>();
        tk2DSprite = GetComponentInChildren<tk2dSprite>();
        fsm = GetComponent<PlayMakerFSM>();
    }
    private void Update()
    {
        var scale = transform.localScale;
        scale.x = 1.5f;
        scale.y = 1.5f;
        transform.localScale = scale;
        damageHero.damageDealt = 2;

        tk2DSprite.color = new Color(0.6f, 0.4f, 1, 0.7f);

        if (update)
        {
            return;
        }
        var state = fsm.GetState("Impact");
        var action = state.GetAction<Wait>(7);
        if (action.time.Value <= 0.02f)
        {
            update = true;
        }
        action.time.Value = 0.01f;
        state = fsm.GetState("Stop Particles");
        action = state.GetAction<Wait>(1);
        action.time.Value = 0.01f;
    }
}

internal class TreeAttackStateMachine : StateMachine
{
    public bool Clockwise;

    SpriteRenderer spriteRenderer;

    public bool isAttacking = false;

    private GameObject orbTemplate;

    private IEnumerator<Transition> Track()
    {
        while (true)
        {
            var myPos = transform.position;
            var targetPos = HeroController.instance.transform.position;
            var direction = targetPos - myPos;
            var scale = 8;
            var velocity = direction * scale;
            if (velocity.magnitude < 8)
            {
                velocity = velocity.normalized * 8;
            }
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = velocity;
            yield return new NoTransition();
        }
    }

    [State]
    private IEnumerator<Transition> Begin()
    {
        // Rotate
        while (!isAttacking)
        {
            var angleVel = 36;
            var angle = Clockwise ? angleVel : -angleVel;
            transform.Rotate(0, 0, angle * Time.deltaTime);
            yield return new NoTransition();
        }

        var orb = Instantiate(orbTemplate, transform.position, Quaternion.identity);
        var randomAngle = Random.Range(0, 360);
        var rangleVelocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        var rigidbody = orb.GetComponent<Rigidbody2D>();
        rigidbody.velocity = rangleVelocity * 8;
        var fsm = orb.LocateMyFSM("Orb Control");
        fsm.SendEvent("FIRE");
        orb.AddComponent<OrbMod>();
        gameObject.SetActive(false);
        yield return new NoTransition();
        /*
        yield return new CoroutineTransition
        {
            Routine = Track()
        };
        */
    }

    public TreeAttackStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        var animation = gameObject.transform.Find("Animation");
        spriteRenderer = animation.GetComponent<SpriteRenderer>();
        var mageKnight = DreamEchoesCore.GetPreloaded("GG_Mage_Knight", "Mage Knight");
        var fsm = mageKnight.LocateMyFSM("Mage Knight");
        var state = fsm.GetState("Shoot");
        var action = state.GetAction<SpawnObjectFromGlobalPool>(3);
        orbTemplate = action.gameObject.Value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            gameObject.SetActive(false);
        }
    }
}
