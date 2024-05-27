using UnityEngine;

namespace DreamEchoesCore.Utils;

internal class WeaverNPCOverride : MonoBehaviour
{
    public enum PlayerTalkSide
    {
        /// <summary>
        /// The player can be either to the left or to the right of the NPC to talk to them
        /// </summary>
        Either,
        /// <summary>
        /// The player must be to the left of the NPC to talk to them
        /// </summary>
        Left,
        /// <summary>
        /// The player must be to the right of the NPC to talk to them
        /// </summary>
        Right
    }

    [SerializeField]
    [Tooltip("Where should the player be positioned in order to talk to the NPC?")]
    PlayerTalkSide talkSide = PlayerTalkSide.Either;

    [SerializeField]
    [Tooltip("The distance the player should be away from the NPC when talking to them")]
    float talkDistance = 2f;

    [SerializeField]
    [Tooltip("Is the player able to talk to this NPC?")]
    bool canTalk = false;

    [SerializeField]
    [Tooltip("Should the NPC be looking towards the player when in range?")]
    bool facePlayerWhenInRange = true;

    [SerializeField]
    [Tooltip("If set to true, the GameObject's layer will be set to the \"Hero Detector\" layer upon Awake")]
    bool updateLayer = true;
}
