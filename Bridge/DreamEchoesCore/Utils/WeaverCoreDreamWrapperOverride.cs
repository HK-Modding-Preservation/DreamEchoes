using UnityEngine;

namespace DreamEchoesCore.Utils;

internal class WeaverCoreDreamWrapperOverride : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The scene to be transported to")]
    string destinationScene;

    [SerializeField]
    [Tooltip("The scene to return back to")]
    string returnScene;

    [SerializeField]
    [Tooltip("The TransitionPoint the player will spawn at when they go to the destination scene")]
    string transitionGateName = "door1";

    [SerializeField]
    [Tooltip("The delay before the player gets transported to the new scene")]
    float warpDelay = 1.75f;

    [SerializeField]
    [Tooltip("Are charms usable in the destination scene?")]
    bool canUseCharms = true;
}
