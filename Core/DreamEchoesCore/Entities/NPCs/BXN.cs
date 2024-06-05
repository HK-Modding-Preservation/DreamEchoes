using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs;

internal class BXN : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle(DreamEchoesCore.Instance.Translate(DreamEchoesCore.BXN_NAME, ""));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.BXN_WORDS, ""));
    }
}