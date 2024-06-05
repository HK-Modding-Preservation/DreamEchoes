using DreamEchoesCore.Utils;
using System.Collections;
using UnityEngine;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs.Yi;

internal class YiTalk : Conversation
{
    public AudioClip FirstMet1;
    public AudioClip FirstMet2;

    public AudioClip GetFlower;
    public AudioClip RejFlower;

    public AudioClip SendFlower;

    private enum YiState
    {
        FirstMet_1,
        FirstMet_2,
    }

    private YiState state = YiState.FirstMet_1;

    protected override IEnumerator DoConversation()
    {
        DisplayTitle(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_NAME, ""));

        var audioSource = GetComponent<AudioSource>();

        if (DreamEchoesCore.Instance.SaveSettings.sentYiFlower)
        {
            audioSource.PlayOneShot(SendFlower);

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_SEND_FLOWER1, ""));

            yield return PresentYesNoQuestion(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_SEND_FLOWER2, ""));

            if (DialogBoxResult == YesNoResult.Yes)
            {
                yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_SEND_FLOWER3, ""));
                yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_SEND_FLOWER5, ""));
                DreamEchoesCore.Instance.SaveSettings.yiflw = true;
            }
            else
            {
                yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_SEND_FLOWER4, ""));
            }
            DreamEchoesCore.Instance.SaveSettings.yileft = true;
            var weavernpc = GetComponent<WeaverNPCOverride>();
            weavernpc.CanTalk = false;
            yield break;
        }

        if (PlayerData.instance.hasXunFlower && !PlayerData.instance.xunFlowerBroken && !DreamEchoesCore.Instance.SaveSettings.sentYiFlower)
        {
            audioSource.PlayOneShot(GetFlower);

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_GFWORDS_1, ""));

            yield return PresentYesNoQuestion(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_GFWORDS_2, ""));

            if (DialogBoxResult == YesNoResult.Yes)
            {
                audioSource.PlayOneShot(RejFlower);
                DreamEchoesCore.Instance.SaveSettings.sentYiFlower = true;
                yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_GFWORDS_3, ""));
            }

            yield break;
        }

        if (state == YiState.FirstMet_1)
        {
            audioSource.PlayOneShot(FirstMet1);

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_1, ""));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_2, ""));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_3, ""));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_4, ""));

            state = YiState.FirstMet_2;
        }
        else if (state == YiState.FirstMet_2)
        {
            audioSource.PlayOneShot(FirstMet2);

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_5, ""));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_6, ""));

            yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_7, ""));
        }
    }
}
