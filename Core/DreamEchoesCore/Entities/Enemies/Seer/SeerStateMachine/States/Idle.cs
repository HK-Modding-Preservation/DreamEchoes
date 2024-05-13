using RingLib.Utils;
using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Idle : State<SeerStateMachine>
{
    private RandomSelector<Type> randomSelector = new([
        new(typeof(Run), 1, 2),
        new(typeof(EvadeJump), 1, 2)
    ]);

    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }

    private IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            StateMachine.Turn();
        }
        StateMachine.Velocity = Vector2.zero;
        StateMachine.Animator.PlayAnimation("Idle");
        yield return new WaitFor { Seconds = StateMachine.Config.IdleDuration };
        yield return new ToState { State = randomSelector.Get() };
    }
}
