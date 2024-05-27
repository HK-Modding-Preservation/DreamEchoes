using HutongGames.PlayMaker.Actions;
using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    internal class HugRadiantNail : MonoBehaviour
    {
        private static GameObject hugRadiantNailPrefab;
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
        }

        public static void SpawnHugRadiantNail(GameObject animation)
        {
            if (hugRadiantNailPrefab == null)
            {
                var absoluteRadiance = DreamEchoesCore.GetPreloaded("GG_Radiance", "Boss Control/Absolute Radiance");
                var fsm = absoluteRadiance.LocateMyFSM("Attack Commands");
                var state = fsm.GetState("CW Spawn");
                var action = state.GetAction<SpawnObjectFromGlobalPool>(0);
                hugRadiantNailPrefab = action.gameObject.Value;
            }
            var seer = animation.transform.parent.gameObject;
            var seerStateMachine = seer.GetComponent<SeerStateMachine>();
            var radiantNailPlaceholders = animation.transform.Find("RadiantNails");
            for (int i = 0; i < radiantNailPlaceholders.childCount; i++)
            {
                var radiantNailPlaceholder = radiantNailPlaceholders.GetChild(i);
                var currentPosition = radiantNailPlaceholder.position;
                var currentRotation = radiantNailPlaceholder.rotation;
                currentRotation *= Quaternion.Euler(0, 0, 90);
                var currentScale = radiantNailPlaceholder.lossyScale;
                if (currentScale.x < 0)
                {
                    currentRotation *= Quaternion.Euler(0, 0, 180);
                }
                var radiantNail = Instantiate(hugRadiantNailPrefab, currentPosition, currentRotation);
                radiantNail.AddComponent<HugRadiantNail>().Speed = seerStateMachine.Config.HugRadiantNailSpeed;
                radiantNail.SetActive(true);
            }
        }
    }

    [State]
    private IEnumerator<Transition> Hug()
    {
        // HugStart
        speak.PlayOneShot(animator.HugWords);
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
