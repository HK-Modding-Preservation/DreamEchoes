using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;

internal class ControlledIdle : State<SeerStateMachine>
{
    public override Transition Enter()
    {
        StateMachine.Velocity = Vector2.zero;
        StateMachine.Animator.PlayAnimation("Idle");
        return new CurrentState();
    }
    public override Transition Update()
    {
        if (StateMachine.InputManager.Direction != 0)
        {
            return new ToState { State = typeof(ControlledRun) };
        }
        return new CurrentState();
    }
}