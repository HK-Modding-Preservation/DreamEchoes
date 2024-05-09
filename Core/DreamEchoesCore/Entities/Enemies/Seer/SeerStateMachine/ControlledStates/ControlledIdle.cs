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
        var inputActions = StateMachine.InputActions;
        if (inputActions.left.IsPressed || inputActions.right.IsPressed)
        {
            var direction = inputActions.left.IsPressed ? -1 : 1;
            if (StateMachine.Direction() != direction)
            {
                StateMachine.Turn();
            }
            return new ToState { State = typeof(ControlledRun) };
        }
        return new CurrentState();
    }
}