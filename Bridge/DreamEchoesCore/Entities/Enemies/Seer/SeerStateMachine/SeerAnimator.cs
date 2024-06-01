using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerAnimator : MonoBehaviour
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

    public AudioClip TeleSlashWord;
    public GameObject TeleSlashGrub;

    public void PlayWakeSound() { }

    public void PlayWakeSlashSound() { }

    public void SpawnJumpEffect() { }

    public void PlayDashSound() { }

    public void PlaySlash1Sound() { }

    public void PlaySlash2Sound() { }

    public void SpawnRadiantNails() { }

    public void PlayParryCounterSound() { }

    public void MakeGrub() { }
}
