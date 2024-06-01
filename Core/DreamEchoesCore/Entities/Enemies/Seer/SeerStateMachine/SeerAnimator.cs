using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerAnimator : RingLib.Components.Animator
{
    public GameObject Roof;

    public AudioClip WakeSound;
    public AudioClip WakeSlashSound;

    public GameObject JumpEffectPrefab;

    public AudioClip DashSound;

    public AudioClip Slash1Sound;
    public AudioClip Slash1Words;
    public AudioClip Slash2Sound;
    public AudioClip Slash2Words;
    public AudioClip Slash3Words;

    public List<AudioClip> HugRadiantNailSounds;
    public AudioClip HugWords;

    public AudioClip ParryCounterSound;

    public AudioClip StunWords;

    public GameObject TeleSlashGrub;

    public void PlayWakeSound()
    {
        PlaySound(WakeSound);
    }

    public void PlayWakeSlashSound()
    {
        PlaySound(WakeSlashSound);
    }

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
