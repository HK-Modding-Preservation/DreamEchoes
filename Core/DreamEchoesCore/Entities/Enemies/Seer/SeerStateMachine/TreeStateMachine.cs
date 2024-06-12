using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class TreeStateMachine : StateMachine
{
    public GameObject Attacks;

    private AudioSource audioSource;
    public AudioClip GrowSound;

    private SpriteRenderer spriteRenderer;

    public int status = 0;

    public SeerStateMachine seerFSM;

    private IEnumerator<Transition> Flash()
    {
        var originalColor = spriteRenderer.color;
        var purpleColor = new Color(0.75f, 0.5f, 0.75f, 1);
        while (true)
        {
            spriteRenderer.color = originalColor;
            yield return new WaitFor { Seconds = 0.1f };
            spriteRenderer.color = purpleColor;
            yield return new WaitFor { Seconds = 0.1f };
        }
    }

    private IEnumerator<Transition> WaitForIdle(int count)
    {
        status = 1;
        while (status <= count)
        {
            yield return new NoTransition();
        }
    }

    private IEnumerator<Transition> WaitForIdleV2()
    {
        var currentIdleCnt = seerFSM.IdleCount;
        while (seerFSM.IdleCount == currentIdleCnt)
        {
            yield return new NoTransition();
        }
    }

    [State]
    private IEnumerator<Transition> Begin()
    {
        // Wait
        RingLib.Log.LogInfo("TreeStateMachine", "WaitFirst");
        yield return new CoroutineTransition
        {
            Routine = WaitForIdle(7)
        };

        var currentIdleCnt = seerFSM.IdleCount;
        while (seerFSM.IdleCount == currentIdleCnt)
        {
            yield return new NoTransition();
        }

        RingLib.Log.LogInfo("TreeStateMachine", "Flash2");
        var originalColor = spriteRenderer.color;
        // Flash & Wait
        yield return new CoroutineTransition
        {
            Routines = [
                Flash(),
                WaitForIdleV2(),
            ]
        };
        spriteRenderer.color = originalColor;

        RingLib.Log.LogInfo("TreeStateMachine", "End");
        PlayGrowSound();
        foreach (var attack in Attacks.GetComponentsInChildren<TreeAttackStateMachine>())
        {
            attack.isAttacking = true;
        }
        float currentAlpha = 1;
        float alphaVel = 1;
        while (currentAlpha > 0)
        {
            currentAlpha -= alphaVel * Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, currentAlpha);
            yield return new NoTransition();
        }
        GameObject.Destroy(gameObject);
    }

    public TreeStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void StartAttacks()
    {
        var attackPos = Attacks.transform.position;
        attackPos.x = transform.parent.position.x;
        Attacks.transform.position = attackPos;
        Attacks.SetActive(true);
    }

    public void PlayGrowSound()
    {
        audioSource.PlayOneShot(GrowSound);
    }
}
