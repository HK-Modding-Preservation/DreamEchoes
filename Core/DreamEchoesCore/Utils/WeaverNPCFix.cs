using UnityEngine;

namespace DreamEchoesCore.Utils;

internal class WeaverNPCFix : MonoBehaviour
{
    private void Start()
    {
        var textMeshPro = GetComponentInChildren<TMPro.TextMeshPro>(true);
        if (textMeshPro == null)
        {
            RingLib.Log.LogError(GetType().Name, "TextMeshPro not found");
            return;
        }
        if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
        {
            textMeshPro.text = "聆听";
        }
        else
        {
            textMeshPro.text = "LISTEN";
        }
    }
}
