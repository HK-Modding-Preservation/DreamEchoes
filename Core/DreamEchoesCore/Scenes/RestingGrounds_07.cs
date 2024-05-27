using RingLib;
using UnityEngine;

namespace DreamEchoesCore.Scenes;

internal class RestingGrounds_07
{
    public static void Initialize(string sceneName)
    {
        if (sceneName != "RestingGrounds_07")
        {
            return;
        }

        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initializing RestingGrounds_07");

        foreach (var rootGameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (rootGameObject.name == "moth_hut")
            {
                var shadow = rootGameObject.transform.Find("rg_bits_02_0051_a").gameObject;
                GameObject.Destroy(shadow);
            }
            else if (rootGameObject.name == "Dream Moth")
            {
                var dreamDialog = rootGameObject.transform.Find("Dream Dialogue").gameObject;
                GameObject.Destroy(dreamDialog);
            }
        }

        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initialized RestingGrounds_07");
    }
}
