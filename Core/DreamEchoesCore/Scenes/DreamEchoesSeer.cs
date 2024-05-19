using DreamEchoesCore.Misc;
using RingLib;
using UnityEngine;

namespace DreamEchoesCore.Scenes;

internal class DreamEchoesSeer
{
    public static void Initialize(string sceneName)
    {
        if (sceneName != "DreamEchoesSeer")
        {
            return;
        }
        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initializing DreamEchoesSeer");
        foreach (var (key, value) in Preload.Names)
        {
            if (key != "Dream_Backer_Shrine")
            {
                continue;
            }
            var prefab = DreamEchoesCore.GetPreloaded(key, value);
            var instance = Object.Instantiate(prefab);
            foreach (var spriteRender in instance.GetComponentsInChildren<SpriteRenderer>(true))
            {
                var color = spriteRender.color;
                color.r = Mathf.Min(color.r + 0.2f, 1);
                spriteRender.color = color;
            }
            instance.SetActive(true);
        }
        var statuePrefab = DreamEchoesCore.GetPreloaded("Mines_34", "mine_summit_statue");
        var statueInstance = Object.Instantiate(statuePrefab);
        statueInstance.transform.position = new Vector3(90, 31, 26);
        statueInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        statueInstance.SetActive(true);
        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initialized DreamEchoesSeer");
    }
}
