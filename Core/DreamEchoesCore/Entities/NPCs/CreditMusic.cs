using UnityEngine;
using WeaverCore;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Entities.NPCs
{
    internal class CreditMusic : MonoBehaviour
    {
        private WeaverMusicCue creditMusic;

        private void Start()
        {
            creditMusic = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("CreditMusic");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            GameManager.instance.AudioManager.ApplyMusicCue(creditMusic, 0, 0, false);
        }
    }
}
