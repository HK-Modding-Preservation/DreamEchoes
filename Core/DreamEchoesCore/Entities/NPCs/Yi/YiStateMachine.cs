using RingLib.StateMachine;
using System.Collections.Generic;

namespace DreamEchoesCore.Entities.NPCs.Yi;

internal class YiStateMachine : StateMachine
{
    private YiAnimator animator;

    [State]
    private IEnumerator<Transition> Left()
    {
        if (!DreamEchoesCore.Instance.SaveSettings.seenSeer)
        {
            gameObject.SetActive(false);
        }

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
        startState: nameof(Left),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        var animation = gameObject.transform.Find("Animation");
        animator = animation.GetComponent<YiAnimator>();
    }
}
