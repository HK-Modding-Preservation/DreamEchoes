using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs.Yi;

internal class YiTalk : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_NAME));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_1));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_2));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_3));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_4));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_5));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_6));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YI_WORDS_7));
    }
}
