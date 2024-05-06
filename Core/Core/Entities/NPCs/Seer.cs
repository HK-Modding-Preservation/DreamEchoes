namespace DreamEchoes.Entities.NPCs;

public class Seer : Conversation
{
    protected override IEnumerator DoConversation()
    {
        DisplayTitle("Seer");
        yield return Speak("Hello, little one!");
    }
}
