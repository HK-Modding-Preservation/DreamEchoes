using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class EvadeJump : State<SeerStateMachine>
{
    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }
    private IEnumerator<Transition> Routine()
    {
        // JumpStart
        var jumpRadius = UnityEngine.Random.Range(StateMachine.Config.EvadeJumpRadiusMin, StateMachine.Config.EvadeJumpRadiusMax);
        var targetXLeft = StateMachine.Target().transform.position.x - jumpRadius;
        var targetXRight = StateMachine.Target().transform.position.x + jumpRadius;
        float targetX;
        if (Mathf.Abs(StateMachine.transform.position.x - targetXLeft) < Mathf.Abs(StateMachine.transform.position.x - targetXRight))
        {
            targetX = targetXRight;
        }
        else
        {
            targetX = targetXLeft;
        }
        var velocityX = (targetX - StateMachine.transform.position.x) * StateMachine.Config.EvadeJumpVelocityXScale;
        if (Mathf.Sign(velocityX) != StateMachine.Direction())
        {
            StateMachine.Turn();
        }
        yield return new WaitFor { Seconds = StateMachine.Animator.PlayAnimation("JumpStart") };

        // JumpAscend
        StateMachine.Velocity = new Vector2(velocityX, StateMachine.Config.EvadeJumpVelocityY);
        StateMachine.Animator.PlayAnimation("JumpAscend");
        while (StateMachine.Velocity.y > 0)
        {
            yield return new CurrentState();
        }

        // JumpDescend
        StateMachine.Animator.PlayAnimation("JumpDescend");
        while (!StateMachine.Landed())
        {
            yield return new CurrentState();
        }

        // JumpEnd
        StateMachine.Velocity = Vector2.zero;
        yield return new WaitFor { Seconds = StateMachine.Animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = typeof(Attack) };
    }
}
