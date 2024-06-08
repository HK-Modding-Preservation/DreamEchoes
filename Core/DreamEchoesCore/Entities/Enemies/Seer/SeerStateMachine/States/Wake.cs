using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private WeaverMusicCue startMusic;
    private WeaverMusicCue musicCue;

    [State]
    private IEnumerator<Transition> Wake()
    {
        // WakeLook
        if (FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        animator.PlayAnimation("WakeLook");
        GameManager.instance.AudioManager.ApplyMusicCue(startMusic, 0, 0, false);
        DreamEchoesCore.Instance.DisableWallJump = false;
        var oldStunCount = stunCount;
        bool check()
        {
            var myPosition = Position;
            var targetPosition = Target().Position();
            var distance = Vector2.Distance(myPosition, targetPosition);
            return distance < Config.WakeDistance || stunCount != oldStunCount;
        }
        yield return new WaitTill { Condition = check };

        // WakeIdle
        animator.Roof.SetActive(true);
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("WakeIdle") };

        // WakeWake
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("WakeWake") };
        GameManager.instance.AudioManager.ApplyMusicCue(musicCue, 0, 0, false);
        DreamEchoesCore.Instance.SaveSettings.seenSeer = true;
        DreamEchoesCore.Instance.DisableWallJump = true;
        foreach (var obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.name == "CameraLockArea(Clone)(Clone)")
            {
                obj.SetActive(false);
                var cameraLockArea = obj.GetComponent<CameraLockArea>();
                cameraLockArea.cameraYMax = cameraLockArea.cameraYMin;
                obj.SetActive(true);
                RingLib.Log.LogInfo("", "CameraLockArea found and locked");
            }
        }
        yield return new ToState { State = nameof(Idle) };
    }
}
