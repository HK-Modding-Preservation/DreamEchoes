using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs;

public class Yile : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle("伊乐");

        yield return Speak(".▽.");

        yield return Speak("◐—◑");

        yield return Speak("(¦3[▓▓]");
    }
}