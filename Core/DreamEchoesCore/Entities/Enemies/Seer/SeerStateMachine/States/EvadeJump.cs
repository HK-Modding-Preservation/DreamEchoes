using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private static void RewriteActions(PlayMakerFSM fsm)
    {
        foreach (var state in fsm.FsmStates)
        {
            for (int i = 0; i < state.Actions.Length; ++i)
            {
                var action = state.Actions[i];
                var type = action.GetType();
                if (action is HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPoolOverTimeV2)
                {
                    var oldAction = action as HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPoolOverTimeV2;
                    var newAction = new Utils.SpawnObjectFromGlobalPoolOverTimeV2
                    {
                        gameObject = oldAction.gameObject,
                        spawnPoint = oldAction.spawnPoint,
                        position = oldAction.position,
                        rotation = oldAction.rotation,
                        frequency = oldAction.frequency,
                        scaleMin = oldAction.scaleMin,
                        scaleMax = oldAction.scaleMax,
                    };
                    state.Actions[i] = newAction;
                    RingLib.Log.LogInfo("Rewrite", $"    Rewrote {state.Name} : {i} of type {type.Name}");
                }
            }
        }
    }

    private void LandWave()
    {
        if (true)
        {
            var shockwave = Instantiate(WaveTemplate);
            shockwave.transform.position = transform.position;
            shockwave.transform.Translate(0, -2f, 0);
            shockwave.LocateMyFSM("shockwave").FsmVariables.GetFsmBool("Facing Right").Value = true;
            shockwave.LocateMyFSM("shockwave").FsmVariables.GetFsmFloat("Speed").Value = 50;
            shockwave.LocateMyFSM("shockwave").RemoveTransition("Move", "WALL");
            shockwave.LocateMyFSM("shockwave").RemoveTransition("Move", "HIT");
            shockwave.transform.SetScaleX(2);
            shockwave.AddComponent<RingLib.EntityManagement.DestroyAfter>().Seconds = 2;
            RewriteActions(shockwave.LocateMyFSM("shockwave"));
        }
        if (true)
        {
            var shockwave = Instantiate(WaveTemplate);
            shockwave.transform.position = transform.position;
            shockwave.transform.Translate(0, -2f, 0);
            shockwave.LocateMyFSM("shockwave").FsmVariables.GetFsmBool("Facing Right").Value = false;
            shockwave.LocateMyFSM("shockwave").FsmVariables.GetFsmFloat("Speed").Value = 50;
            shockwave.LocateMyFSM("shockwave").RemoveTransition("Move", "WALL");
            shockwave.LocateMyFSM("shockwave").RemoveTransition("Move", "HIT");
            shockwave.transform.SetScaleX(2);
            shockwave.AddComponent<RingLib.EntityManagement.DestroyAfter>().Seconds = 2;
            RewriteActions(shockwave.LocateMyFSM("shockwave"));
        }
    }

    [State]
    private IEnumerator<Transition> EvadeJump()
    {
        // JumpStart
        var jumpRadiusMin = Config.EvadeJumpRadiusMin;
        var jumpRadiusMax = Config.EvadeJumpRadiusMax;
        var jumpRadius = Random.Range(jumpRadiusMin, jumpRadiusMax);
        var targetXLeft = Target().Position().x - jumpRadius;
        var targetXRight = Target().Position().x + jumpRadius;
        float targetX;
        if (Mathf.Abs(Position.x - targetXLeft) < Mathf.Abs(Position.x - targetXRight))
        {
            targetX = targetXRight;
        }
        else
        {
            targetX = targetXLeft;
        }
        var velocityX = (targetX - Position.x) * Config.EvadeJumpVelocityXScale;
        var velocityY = Config.EvadeJumpVelocityY;
        if (Mathf.Sign(velocityX) != Direction())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpStart") };

        // JumpAscend
        Velocity = new Vector2(velocityX, velocityY);
        animator.PlayAnimation("JumpAscend");
        yield return new WaitTill { Condition = () => Velocity.y <= 0 };

        // JumpDescend
        animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = Landed };
        animator.PlayLandSound();
        LandWave();

        // JumpEnd
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = nameof(Attack) };
    }
}
