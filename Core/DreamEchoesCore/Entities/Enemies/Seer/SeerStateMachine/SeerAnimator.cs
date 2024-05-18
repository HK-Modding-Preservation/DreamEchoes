using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerAnimator : RingLib.Components.Animator
{
    public GameObject JumpEffectPrefab;

    public AudioClip DashSound;

    public AudioClip Slash1Sound;
    public AudioClip Slash2Sound;

    public List<AudioClip> HugRadiantNailSounds;

    public AudioClip ParryCounterSound;

    public void SpawnJumpEffect()
    {
        var currentPosition = JumpEffectPrefab.transform.position;
        var currentRotation = JumpEffectPrefab.transform.rotation;
        var currentScale = JumpEffectPrefab.transform.lossyScale;
        var jumpEffect = Instantiate(JumpEffectPrefab, currentPosition, currentRotation);
        jumpEffect.transform.localScale = currentScale;
        jumpEffect.SetActive(true);
    }

    public void PlayDashSound()
    {
        PlaySound(DashSound);
    }

    public void PlaySlash1Sound()
    {
        PlaySound(Slash1Sound);
    }

    public void PlaySlash2Sound()
    {
        PlaySound(Slash2Sound);
    }

    public void SpawnRadiantNails()
    {
        SeerStateMachine.HugRadiantNail.SpawnHugRadiantNail(gameObject);
        if (HugRadiantNailSounds == null || HugRadiantNailSounds.Count == 0)
        {
            RingLib.Log.LogError(GetType().Name, "No hug radiant nail sounds found");
            return;
        }
        if (HugRadiantNailSounds.Count > 0)
        {
            var sound = HugRadiantNailSounds[Random.Range(0, HugRadiantNailSounds.Count)];
            PlaySound(sound);
        }
    }

    public void PlayParryCounterSound()
    {
        PlaySound(ParryCounterSound);
    }
}
