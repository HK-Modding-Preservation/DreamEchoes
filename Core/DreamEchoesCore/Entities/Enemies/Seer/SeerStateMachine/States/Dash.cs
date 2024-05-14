using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Dash : State<SeerStateMachine>
{
    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }

    private IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            yield return new CoroutineTransition { Routine = StateMachine.Turn() };
        }
        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.DashVelocityX * direction;
        StateMachine.BoxCollider2D.offset = StateMachine.Config.DashStartColliderOffset;
        StateMachine.BoxCollider2D.size = StateMachine.Config.DashStartColliderSize;
        var startDuration = StateMachine.Animator.PlayAnimation("DashStart");
        var timer = 0f;
        while (timer < startDuration)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, timer / startDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new CurrentState();
        }
        StateMachine.BoxCollider2D.offset = StateMachine.Config.DashColliderOffset;
        StateMachine.BoxCollider2D.size = StateMachine.Config.DashColliderSize;
        StateMachine.Rigidbody2D.gravityScale = 0;
        StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Dash");
        yield return new WaitFor { Seconds = StateMachine.Config.DashDuration };
        StateMachine.BoxCollider2D.offset = StateMachine.Config.DashEndColliderOffset;
        StateMachine.BoxCollider2D.size = StateMachine.Config.DashEndColliderSize;
        StateMachine.Rigidbody2D.gravityScale = StateMachine.Config.GravityScale;
        var endDuration = StateMachine.Animator.PlayAnimation("DashEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, timer / endDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new CurrentState();
        }
        StateMachine.BoxCollider2D.offset = StateMachine.OriginalBoxCollider2DOffset;
        StateMachine.BoxCollider2D.size = StateMachine.OriginalBoxCollider2DSize;
        yield return new ToState { State = typeof(Idle) };
    }
}
