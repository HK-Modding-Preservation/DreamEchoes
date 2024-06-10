using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class TreeAttackStateMachine : StateMachine
{
    public bool Clockwise;

    SpriteRenderer spriteRenderer;

    public bool isAttacking = false;

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

        yield return new CoroutineTransition
        {
            Routine = Track()
        };
    }

    public TreeAttackStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        var animation = gameObject.transform.Find("Animation");
        spriteRenderer = animation.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            gameObject.SetActive(false);
        }
    }
}
