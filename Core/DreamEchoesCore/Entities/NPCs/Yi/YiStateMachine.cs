using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.NPCs.Yi;

internal class YiStateMachine : StateMachine
{
    public AudioClip Seeya;

    private YiAnimator animator;

    [State]
    private IEnumerator<Transition> Begin()
    {
        if (!DreamEchoesCore.Instance.SaveSettings.seenSeer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            foreach (var obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (obj.name == "Dream Moth")
                {
                    var audioSource = obj.GetComponent<AudioSource>();
                    Destroy(audioSource);
                    LogInfo("YiStateMachine", "Destroyed Dream Moth AudioSource");
                }
            }
        }
        yield return new ToState { State = nameof(Left) };
    }

    [State]
    private IEnumerator<Transition> Left()
    {
        animator.PlayAnimation("Left");
        yield return new WaitTill
        {
            Condition = () => HeroController.instance.transform.position.x > gameObject.transform.position.x
        };
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("TurnRight")
        };
        yield return new ToState
        {
            State = nameof(Right)
        };
    }

    [State]
    private IEnumerator<Transition> Right()
    {
        animator.PlayAnimation("Right");
        yield return new WaitTill
        {
            Condition = () => HeroController.instance.transform.position.x < gameObject.transform.position.x
        };
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("TurnLeft")
        };
        yield return new ToState
        {
            State = nameof(Left)
        };
    }

    public YiStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        var animation = gameObject.transform.Find("Animation");
        animator = animation.GetComponent<YiAnimator>();
        if (DreamEchoesCore.Instance.SaveSettings.yileft)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private float lastHeroX = 1000;

    private bool OutRange(float heroX)
    {
        float threshold = 9;
        return heroX - gameObject.transform.position.x > threshold;
    }

    protected override void StateMachineUpdate()
    {
        if (!OutRange(lastHeroX) && OutRange(HeroController.instance.transform.position.x))
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(Seeya);
        }
        lastHeroX = HeroController.instance.transform.position.x;
    }
}
