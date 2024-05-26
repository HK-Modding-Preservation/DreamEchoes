using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs;

internal class Ring : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle(DreamEchoesCore.Instance.Translate(DreamEchoesCore.RING_NAME));

        yield return Speak(DreamEchoesCore.Instance.Translate(DreamEchoesCore.RING_WORDS));
    }
}