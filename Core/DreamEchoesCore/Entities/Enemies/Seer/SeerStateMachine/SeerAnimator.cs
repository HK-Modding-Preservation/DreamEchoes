using HutongGames.PlayMaker.Actions;
using RingLib.Utils;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class SeerAnimator : RingLib.Components.Animator
{
    public SeerStateMachine SeerStateMachine;
    public GameObject JumpEffectPrefab;
    public AudioClip DashSound;
    public AudioClip Slash1Sound;
    public AudioClip Slash2Sound;
    private GameObject hugRadiantNailPrefab;

    protected override void AnimatorStart()
    {
        var absoluteRadiance = DreamEchoesCore.GetPreloaded("GG_Radiance", "Boss Control/Absolute Radiance");
        var fsm = absoluteRadiance.LocateMyFSM("Attack Commands");
        var state = fsm.GetState("CW Spawn");
        var action = state.GetAction<SpawnObjectFromGlobalPool>(0);
        hugRadiantNailPrefab = action.gameObject.Value;
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
        var radiantNailPlaceholders = transform.Find("RadiantNails");
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
            radiantNail.AddComponent<HugRadiantNail>().Speed = SeerStateMachine.Config.HugRadiantNailSpeed;
            radiantNail.SetActive(true);
        }
    }
}
