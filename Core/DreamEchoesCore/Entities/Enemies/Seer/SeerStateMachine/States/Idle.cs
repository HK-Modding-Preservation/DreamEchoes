
using DreamEchoesCore.RingLib.StateMachine;
using System.Collections;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Idle : State<SeerStateMachine>
{
    public override Type? Enter()
    {
        StartCoroutine(Routine());
        return null;
    }
    private IEnumerator Routine()
    {
        StateMachine.Velocity = Vector2.zero;
        StateMachine.Animator.PlayAnimation("Idle");
        yield return StateMachine.Config.IdleDuration;
        yield return typeof(Run);
    }
}