using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoesCore.Entities.NPCs;

internal class Ring : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle("近环");

        yield return Speak("左特感叹号表情包.jpg");

        yield return PresentYesNoQuestion("左特问号表情包.jpg");

        yield return Speak("左特躺平表情包.jpg");

        yield return Speak("粉猫点头表情包.gif");
    }
}