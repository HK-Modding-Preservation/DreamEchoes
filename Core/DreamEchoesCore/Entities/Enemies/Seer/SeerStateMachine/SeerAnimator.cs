using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerAnimator : RingLib.Components.Animator
{
    public GameObject JumpEffectPrefab;
    public AudioClip DashSound;
    public AudioClip Slash1Sound;
    public AudioClip Slash2Sound;

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
        HugRadiantNail.SpawnHugRadiantNail(gameObject);
    }
}
