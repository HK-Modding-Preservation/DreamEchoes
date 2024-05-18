using HutongGames.PlayMaker.Actions;
using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class HugRadiantNail : MonoBehaviour
{
    private PlayMakerFSM fsm;
    public float Speed;

    private void Start()
    {
        fsm = gameObject.LocateMyFSM("Control");
        var state = fsm.GetState("Fire CW");
        state.RemoveAction<FloatAdd>(2);
        state.RemoveAction<FaceAngle>(1);
        state.GetAction<SetVelocityAsAngle>(0).speed.Value = Speed;
    }

    private void Update()
    {
        if (fsm.ActiveStateName == "Appear")
        {
            fsm.SendEvent("FAN ANTIC");
        }
        else if (fsm.ActiveStateName == "Fan Ready")
        {
            fsm.SendEvent("FAN ATTACK CW");
        }
        var bottomRay = new Vector2(transform.position.x, transform.position.y);
        var raycastHit2D = Physics2D.Raycast(bottomRay, -Vector2.up, 0.4f, 1 << 8);
        if (raycastHit2D.collider != null)
        {
            var state = fsm.GetState("Fire CW");
            var action = state.GetAction<SetVelocityAsAngle>(0);
            action.speed.Value = 0;
        }
    }
}

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Hug()
    {
        // HugStart
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        var direction = Direction();
        var velocityX = Config.HugVelocityX * -direction;
        var velocityY = Config.HugVelocityY;
        Transition updater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            var currentVelocityY = Mathf.Lerp(0, velocityY, normalizedTime);
            Velocity = new Vector2(currentVelocityX, currentVelocityY);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("HugStart", updater)
        };

        // Hug
        Rigidbody2D.gravityScale = 0;
        Velocity = Vector2.zero;
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("Hug", updater)
        };

        // HugEnd
        Rigidbody2D.gravityScale = Config.GravityScale;
        animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = () => Landed() };
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = nameof(Idle) };
    }
}
