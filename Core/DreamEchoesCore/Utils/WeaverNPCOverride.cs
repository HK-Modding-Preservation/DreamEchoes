﻿using System;
using System.Collections;
using UnityEngine;
using WeaverCore;
using WeaverCore.Assets.Components;
using WeaverCore.Components;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Utils
{
    /// <summary>
    /// A custom NPC that the player can talk to
    /// </summary>
    public class WeaverNPCOverride : MonoBehaviour
    {
        /// <summary>
        /// Which side of the NPC can the player be on when talking to them?
        /// </summary>
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


        /// <summary>
        /// Is the player in range of this NPC to start a conversation?
        /// </summary>
        public bool PlayerInRange { get; private set; }
        /// <summary>
        /// Is the player currently talking to this NPC?
        /// </summary>
        public bool Talking { get; private set; } = false;

        bool startingConversation = false;

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

        /// <summary>
        /// Is the player able to talk to this NPC?
        /// </summary>
        public bool CanTalk { get => canTalk; set => canTalk = value; }

        /// <summary>
        /// Should the NPC be looking towards the player when in range?
        /// </summary>
        public bool FacePlayerWhenInRange { get => facePlayerWhenInRange; set => facePlayerWhenInRange = value; }

        /// <summary>
        /// Where should the player be positioned in order to talk to the NPC?
        /// </summary>
        public PlayerTalkSide TalkSide { get => talkSide; set => talkSide = value; }

        /// <summary>
        /// The distance the player should be away from the NPC when talking to them
        /// </summary>
        public float TalkDistance { get => talkDistance; set => talkDistance = value; }

        GameObject Ranges;
        WeaverArrowPrompt prompt;
        EventManager eventManager;
        Coroutine mainRoutine;

        [NonSerialized]
        bool facingRight = false;

        private void Start()
        {
            //WeaverLog.Log("BENCH = " + gameObject);
            //WeaverLog.Log("BENCH START A");
            if (updateLayer)
            {
                //WeaverLog.Log("BENCH START B");
                gameObject.layer = LayerMask.NameToLayer("Hero Detector");
            }
            //WeaverLog.Log("BENCH START C");
            eventManager = GetComponent<EventManager>();
            //WeaverLog.Log("BENCH START D");
            if (eventManager == null)
            {
                //WeaverLog.Log("BENCH START E");
                eventManager = gameObject.AddComponent<EventManager>();
            }
            //WeaverLog.Log("BENCH START F");
            prompt = GetComponentInChildren<WeaverArrowPrompt>(true);
            //WeaverLog.Log("BENCH START G");
            prompt.HideInstant();
            //WeaverLog.Log("BENCH START H");
            canTalk = true;
            //WeaverLog.Log("BENCH START I");
            Ranges = transform.Find("Ranges")?.gameObject;
            //WeaverLog.Log("BENCH START J");
            eventManager.OnReceivedEvent += OnEventReceived;
            //WeaverLog.Log("BENCH START K");
            mainRoutine = StartCoroutine(InitRoutine());
            //sWeaverLog.Log("BENCH START L");
        }

        private void OnEventReceived(string eventName, GameObject source)
        {
            if (eventName == "DREAM DIALOGUE START" && !Talking && !startingConversation)
            {
                if (mainRoutine != null)
                {
                    StopCoroutine(mainRoutine);
                    mainRoutine = null;
                }
                prompt.Hide();
            }
            else if (eventName == "DREAM DIALOGUE END" && !Talking && !startingConversation)
            {
                mainRoutine = StartCoroutine(IdleRoutine());
            }
            else if (eventName == "CONVO CANCEL")
            {
                if (mainRoutine != null)
                {
                    StopCoroutine(mainRoutine);
                    mainRoutine = null;
                }
                mainRoutine = StartCoroutine(IdleRoutine());
            }
            else if (eventName == "NPC CONTROL OFF")
            {
                prompt.Hide();
            }
            else if (eventName == "CONVO END" && Talking)
            {
                StartCoroutine(EndConvo());
            }
            else if (eventName == "HERO DAMAGED" && Talking)
            {
                StartCoroutine(ResetConvo());
            }
        }

        IEnumerator InitRoutine()
        {
            yield return new WaitForSeconds(1f);
            yield return IdleRoutine();
        }

        IEnumerator IdleRoutine()
        {
            while (true)
            {
                while (!PlayerInRange)
                {
                    if (facePlayerWhenInRange)
                    {
                        FacePlayer(transform.position + new Vector3(-9999f, 0f));
                    }
                    yield return null;
                }
                //yield return new WaitUntil(() => PlayerInRange);
                if (canTalk)
                {
                    prompt.Show();
                    while (true)
                    {
                        if (!PlayerInRange)
                        {
                            prompt.Hide();
                            break;
                        }
                        if (HeroController.instance.CanTalk() && (PlayerInput.up.WasPressed || PlayerInput.down.WasPressed))
                        {
                            yield return StartTalking();
                            yield break;
                        }
                        if (facePlayerWhenInRange)
                        {
                            FacePlayer(Player.Player1.transform.position);
                        }
                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        IEnumerator PositionChecker()
        {
            var playerPos = Vector3.zero;
            while (true)
            {
                var newPos = HeroController.instance.transform.position;
                playerPos = newPos;
                yield return null;
            }
        }

        IEnumerator StartTalking()
        {
            StartCoroutine(PositionChecker());
            startingConversation = true;
            if (Ranges != null)
            {
                Ranges.SetActive(true);
            }
            PlayerData.instance.SetBool("disablePause", true);
            HeroController.instance.RelinquishControl();
            HeroController.instance.StopAnimationControl();
            prompt.Hide();

            var playerPos = Player.Player1.transform.position;
            var talkPosition = GetTalkPositionX(playerPos);

            if (facePlayerWhenInRange)
            {
                FacePlayer(playerPos.With(x: talkPosition));
            }

            yield return MovePlayerToPosition(talkPosition);
            startingConversation = false;
            Talking = true;
            HeroUtilities.PlayPlayerClip("Idle");
            EventManager.SendEventToGameObject("CONVO START", gameObject, gameObject);
            EventManager.BroadcastEvent("NPC CONVO START", gameObject);

            var conversation = GetComponent<Conversation>();
            if (conversation == null)
            {
                throw new Exception($"Error: No Conversation Component is attached to object {gameObject.name}. One is needed in order to talk to the WeaverNPC");
            }
            yield return conversation.StartConversationRoutine(false);
            conversation.HideConversationBox();
            yield return EndConvo();
        }

        IEnumerator MovePlayerToPosition(float talkPos)
        {
            bool playerFacingRight = true;
            if (HeroController.instance.transform.localScale.x > 0)
            {
                playerFacingRight = false;
            }

            var heroPosX = HeroController.instance.transform.position.x;

            if (talkPos > heroPosX && !playerFacingRight)
            {
                yield return TurnPlayerRight();
            }
            else if (talkPos < heroPosX && playerFacingRight)
            {
                yield return TurnPlayerLeft();
            }

            if (talkPos > heroPosX)
            {
                yield return MovePlayerRight(talkPos);
            }
            else if (talkPos < heroPosX)
            {
                yield return MovePlayerLeft(talkPos);
            }

            if (HeroController.instance.transform.localScale.x > 0)
            {
                playerFacingRight = false;
            }
            else
            {
                playerFacingRight = true;
            }

            if (HeroController.instance.transform.position.x >= transform.position.x && playerFacingRight)
            {
                yield return TurnPlayerLeft();
            }
            else if (HeroController.instance.transform.position.x < transform.position.x && !playerFacingRight)
            {
                yield return TurnPlayerRight();
            }
        }

        IEnumerator TurnPlayerRight()
        {
            var heroRB = HeroController.instance.GetComponent<Rigidbody2D>();
            heroRB.velocity = default;
            HeroController.instance.FaceRight();
            yield return HeroUtilities.PlayPlayerClipTillDone("Turn");
        }

        IEnumerator TurnPlayerLeft()
        {
            var heroRB = HeroController.instance.GetComponent<Rigidbody2D>();
            heroRB.velocity = default;
            HeroController.instance.FaceLeft();
            yield return HeroUtilities.PlayPlayerClipTillDone("Turn");
        }

        IEnumerator MovePlayerRight(float destX)
        {
            HeroController.instance.FaceRight();
            HeroUtilities.PlayPlayerClip("Walk");
            var heroRB = HeroController.instance.GetComponent<Rigidbody2D>();
            heroRB.velocity = new Vector2(6f, 0f);
            for (float t = 0; t < 3f; t += Time.deltaTime)
            {
                if (HeroController.instance.transform.position.x >= destX)
                {
                    break;
                }
                heroRB.velocity = new Vector2(6f, 0f);
                yield return null;
            }
            heroRB.velocity = new Vector2(0f, 0f);
        }

        IEnumerator MovePlayerLeft(float destX)
        {
            HeroController.instance.FaceLeft();
            HeroUtilities.PlayPlayerClip("Walk");
            var heroRB = HeroController.instance.GetComponent<Rigidbody2D>();
            heroRB.velocity = new Vector2(-6f, 0f);
            for (float t = 0; t < 3f; t += Time.deltaTime)
            {
                if (HeroController.instance.transform.position.x <= destX)
                {
                    break;
                }
                heroRB.velocity = new Vector2(-6f, 0f);
                yield return null;
            }
            heroRB.velocity = new Vector2(0f, 0f);
        }

        IEnumerator EndConvo()
        {
            if (Ranges != null)
            {
                Ranges.SetActive(true);
            }
            Talking = false;
            PlayerData.instance.SetBool("disablePause", false);
            HeroController.instance.RegainControl();
            HeroController.instance.StartAnimationControl();
            HeroController.instance.PreventCastByDialogueEnd();
            yield return new WaitForSeconds(0.5f);
            mainRoutine = StartCoroutine(IdleRoutine());
        }

        IEnumerator ResetConvo()
        {
            EventManager.SendEventToGameObject("RESET CONVO", gameObject, gameObject);
            EventManager.BroadcastEvent("NPC TITLE DOWN", gameObject);
            var dialogManager = GameObject.Find("DialogueManager");
            if (dialogManager != null)
            {
                EventManager.SendEventToGameObject("BOX DOWN", dialogManager, gameObject);
            }
            yield return EndConvo();
        }

        float GetTalkPositionX(Vector3 playerPosition)
        {
            switch (TalkSide)
            {
                case PlayerTalkSide.Either:
                    if (playerPosition.x >= transform.position.x)
                    {
                        return transform.position.x + TalkDistance;
                    }
                    else
                    {
                        return transform.position.x - TalkDistance;
                    }
                case PlayerTalkSide.Left:
                    return transform.position.x - TalkDistance;
                case PlayerTalkSide.Right:
                    return transform.position.x + TalkDistance;
                default:
                    return transform.position.x;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "HeroBox" || collision.GetComponent<HeroController>() != null)
            {
                PlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "HeroBox" || collision.GetComponent<HeroController>() != null)
            {
                PlayerInRange = false;
            }
        }


        bool currentlyFacingRight = false;
        bool playingTurnAnimation = false;

        /// <summary>
        /// Used to play an animation to "Turn" towards the player
        /// </summary>
        /// <param name="playerPos">The player's current position</param>
        protected virtual void FacePlayer(Vector3 playerPos)
        {
            if (!playingTurnAnimation)
            {
                var playerIsRight = playerPos.x >= transform.position.x;

                if (playerIsRight && !currentlyFacingRight)
                {
                    //FACE TO THE RIGHT
                    StartCoroutine(PlayTurnAnimation(true));
                }
                else if (!playerIsRight && currentlyFacingRight)
                {
                    //FACE TO THE LEFT
                    StartCoroutine(PlayTurnAnimation(false));
                }
            }
        }

        float GetClipDuration(WeaverAnimationData.Clip clip)
        {
            if (clip.WrapMode == WeaverAnimationData.WrapMode.LoopSection)
            {
                return clip.LoopStart * (1f / clip.FPS);
            }
            else
            {
                return clip.Duration;
            }
        }

        IEnumerator PlayTurnAnimation(bool turnRight)
        {
            if (TryGetComponent<WeaverAnimationPlayer>(out var anim))
            {
                WeaverAnimationData.Clip clip;
                if (turnRight)
                {
                    if (anim.AnimationData.TryGetClip("Turn Right", out clip) || anim.AnimationData.TryGetClip("Face Right", out clip))
                    {
                        playingTurnAnimation = true;
                        float duration = GetClipDuration(clip);
                        //var duration = clip.Duration;
                        anim.PlayAnimation(clip.Name);
                        currentlyFacingRight = true;
                        yield return new WaitForSeconds(duration);
                        //yield return anim.PlayAnimationTillDone(clip.Name,true);
                        playingTurnAnimation = false;
                    }
                }
                else
                {
                    if (anim.AnimationData.TryGetClip("Turn Left", out clip) || anim.AnimationData.TryGetClip("Face Left", out clip))
                    {
                        playingTurnAnimation = true;
                        float duration = GetClipDuration(clip);
                        currentlyFacingRight = false;
                        anim.PlayAnimation(clip.Name);
                        yield return new WaitForSeconds(duration);
                        //yield return anim.PlayAnimationTillDone(clip.Name, true);
                        playingTurnAnimation = false;
                    }
                }
            }
        }
    }
}
