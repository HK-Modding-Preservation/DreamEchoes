using System.Collections;
using UnityEngine;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs.Yi;

internal class YiTalk : Conversation
{
    public AudioClip FirstMet1;
    public AudioClip FirstMet2;

    private enum YiState
    {
        FirstMet_1,
        FirstMet_2,
    }

    private YiState state = YiState.FirstMet_1;

    protected override IEnumerator DoConversation()
    {
        DisplayTitle(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_NAME));

        var audioSource = GetComponent<AudioSource>();

        if (state == YiState.FirstMet_1)
        {
            audioSource.PlayOneShot(FirstMet1);

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_1));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_2));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_3));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_4));

            state = YiState.FirstMet_2;
        }
        else if (state == YiState.FirstMet_2)
        {
            audioSource.PlayOneShot(FirstMet2);

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_5));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_6));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_7));
        }
    }
}
