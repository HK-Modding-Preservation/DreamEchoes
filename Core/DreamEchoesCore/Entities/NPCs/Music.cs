using UnityEngine;

namespace DreamEchoesCore.Entities.NPCs;

internal class Music : MonoBehaviour
{
    public MusicCue musicCue;

    private void Start()
    {
        GameManager.instance.AudioManager.ApplyMusicCue(musicCue, 0, 0, false);
    }
}