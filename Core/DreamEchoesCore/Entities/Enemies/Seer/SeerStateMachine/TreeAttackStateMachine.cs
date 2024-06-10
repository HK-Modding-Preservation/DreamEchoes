using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class TreeAttackStateMachine : StateMachine
{
    public bool Clockwise;

    SpriteRenderer spriteRenderer;

    [State]
    private IEnumerator<Transition> Begin()
    {
        // Rotate
        while (true)
        {
            var angleVel = 36;
            var angle = Clockwise ? angleVel : -angleVel;
            transform.Rotate(0, 0, angle * Time.deltaTime);
            yield return new NoTransition();
        }
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
}
