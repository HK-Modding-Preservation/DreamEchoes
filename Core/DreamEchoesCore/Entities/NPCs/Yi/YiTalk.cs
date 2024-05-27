using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs.Yi;

internal class YiTalk : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YILE_NAME));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.YILE_WORDS));
    }
}
