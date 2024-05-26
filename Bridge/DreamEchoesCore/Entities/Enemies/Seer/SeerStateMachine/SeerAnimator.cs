using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerAnimator : MonoBehaviour
{
    public AudioClip WakeSound;
    public AudioClip WakeSlashSound;

    public GameObject JumpEffectPrefab;

    public AudioClip DashSound;

    public AudioClip Slash1Sound;
    public AudioClip Slash2Sound;

    public List<AudioClip> HugRadiantNailSounds;

    public AudioClip ParryCounterSound;

    public void PlayWakeSound() { }

    public void PlayWakeSlashSound() { }

    public void SpawnJumpEffect() { }

    public void PlayDashSound() { }

    public void PlaySlash1Sound() { }

    public void PlaySlash2Sound() { }

    public void SpawnRadiantNails() { }

    public void PlayParryCounterSound() { }
}
