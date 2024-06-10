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

    public AudioClip TeleSlashWord;
    public GameObject TeleSlashGrub;

    public AudioClip LaserFire;
    public AudioClip LaserWord;

    public AudioClip RunSound;
    public AudioClip LandSound;

    public Renderer laserRenderer;

    public GameObject LaserHit;

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

    public void MakeGrub()
    {
        var grubTemplate = TeleSlashGrub;
        var prefabPos = grubTemplate.transform.position;
        prefabPos.x = transform.parent.position.x;

        var prefab = transform.parent.GetComponent<SeerStateMachine>().traitorWave;
        var wave = Object.Instantiate(prefab);
        wave.transform.position = new Vector3(prefabPos.x, prefabPos.y - 5, 1);
        wave.GetComponent<Rigidbody2D>().velocity = new Vector2(12, 0);
        wave.transform.localScale = new Vector3(2, 1, 1);

        if (transform.parent.localScale.x > 0)
        {
            var scale = wave.transform.localScale;
            scale.x *= -1;
            wave.transform.localScale = scale;
            var velocity = wave.GetComponent<Rigidbody2D>().velocity;
            velocity.x *= -1;
            wave.GetComponent<Rigidbody2D>().velocity = velocity;
        }

        /*
        var grub = Instantiate(grubTemplate, prefabPos, Quaternion.identity);
        var scale = grub.transform.localScale;
        if (transform.parent.localScale.x < 0)
        {
            scale.x *= -1;
        }
        grub.transform.localScale = scale;
        grub.SetActive(true);
        grub.GetComponent<Rigidbody2D>().velocity = new Vector2(15 * (scale.x > 0 ? -1 : 1), 0);
        grub.AddComponent<GrubFSM>();
        */
    }

    public void PlayLaserFire()
    {
        PlaySound(LaserFire);
    }

    public void PlayRunSound()
    {
        PlaySoundLoop(RunSound);
    }

    public void StopRunSound()
    {
        StopSoundLoop();
    }

    public void PlayLandSound()
    {
        PlaySound(LandSound);
    }

    public void UpdateLaserCutoff(float y)
    {
        laserRenderer.material.SetFloat("_CutoffY", y);
    }

    public void SetHalfCollider()
    {
        var seerStateMachine = transform.parent.GetComponent<SeerStateMachine>();
        seerStateMachine.SetHalfCollider();
    }

    public void SetFullCollider()
    {
        var seerStateMachine = transform.parent.GetComponent<SeerStateMachine>();
        seerStateMachine.SetFullCollider();
    }

    public void TreeSummon()
    {
        var seerStateMachine = transform.parent.GetComponent<SeerStateMachine>();
        seerStateMachine.speak.PlayOneShot(Slash1Words);
        PlaySlash2Sound();
    }
}
