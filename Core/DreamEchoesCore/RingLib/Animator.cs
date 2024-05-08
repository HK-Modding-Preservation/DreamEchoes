using UnityEngine;

namespace DreamEchoesCore.RingLib;

internal class Animator : MonoBehaviour
{
    private UnityEngine.Animator animator;
    private Dictionary<string, float> clipLengths = [];
    private AudioSource audioSource;
    private void Start()
    {
        animator = GetComponent<UnityEngine.Animator>();
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            clipLengths[clip.name] = clip.isLooping ? float.MaxValue : clip.length;
        }
        audioSource = GetComponent<AudioSource>();
    }
    public float PlayAnimation(string clipName)
    {
        if (!clipLengths.ContainsKey(clipName))
        {
            Log.LogError(GetType().Name, "Animation not found");
            return float.MaxValue;
        }
        animator.Play(clipName, -1, 0);
        return clipLengths[clipName];
    }
    protected void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
