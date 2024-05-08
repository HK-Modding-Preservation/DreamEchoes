using DreamEchoesCore.RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;

internal class ControlledIdle : State<SeerStateMachine>
{
    public override Type? Enter()
    {
        StateMachine.Velocity = Vector2.zero;
        StateMachine.Animator.PlayAnimation("Idle");
        return null;
    }
    public override Type? Update()
    {
        var inputActions = StateMachine.InputActions;
        if (inputActions.left.IsPressed || inputActions.right.IsPressed)
        {
            var direction = inputActions.left.IsPressed ? -1 : 1;
            if (StateMachine.Direction() != direction)
            {
                StateMachine.Turn();
            }
            return typeof(ControlledRun);
        }
        return null;
    }
}