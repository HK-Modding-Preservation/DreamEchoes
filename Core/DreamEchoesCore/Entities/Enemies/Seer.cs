using System.Collections;
using WeaverCore.Assets.Components;

namespace DreamEchoes.Entities.Enemies;

public class Seer : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle("Seer");
        yield return Speak("Hello, little one!");
    }
}
