/*
using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// Main class for implementing NPC game objects.
    /// </summary>
    public class NPCController : MonoBehaviour
    {
        public ConversationScript[] conversations;
        public bool isManInBlack; // Toggle for ManInBlack specific behavior
        public float speed = 2.0f;
        public float maxSpeed = 3.0f;
        public StoryItem requiredStoryItem; // Reference to the required story item
        public MC_Controller mc; // Manually set the MC in the Unity Editor

        protected Quest activeQuest = null;
        protected Quest[] quests;

        protected GameModel model = Schedule.GetModel<GameModel>();
        private Animator animator;
        private Rigidbody2D rigidbody2D;
        private bool isMoving = true; // Flag to check if the NPC is moving
        private bool collidedWithMC = false; // Flag to check if the NPC is colliding with the MC

        protected virtual void OnEnable()
        {
            quests = gameObject.GetComponentsInChildren<Quest>();
        }

        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            model = Schedule.GetModel<GameModel>(); // Initialize the GameModel

            // Set Rigidbody2D to be kinematic to prevent being knocked away
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        void Update()
        {
            if (isManInBlack)
            {
                // Check if the required story item has been triggered and NPC is still allowed to move
                if (isMoving && requiredStoryItem != null && model.HasSeenStoryItem(requiredStoryItem.ID) && mc != null && !collidedWithMC)
                {
                    // Move towards the MC
                    MoveTowardsMC();
                }
            }
        }

        void FixedUpdate()
        {
            // Movement logic is now handled in MoveTowardsMC method
        }

        private void MoveTowardsMC()
        {
            float step = Mathf.Min(speed, maxSpeed) * Time.deltaTime;
            Vector3 targetPosition = mc.transform.position;
            Vector3 currentPosition = rigidbody2D.position;

            float newX = Mathf.MoveTowards(currentPosition.x, targetPosition.x, step);
            float newY = Mathf.MoveTowards(currentPosition.y, targetPosition.y, step);

            // Update the animator based on direction
            float horizontal = targetPosition.x - currentPosition.x;
            float vertical = targetPosition.y - currentPosition.y;

            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                if (horizontal > 0)
                {
                    animator.SetFloat("Move X", 1);
                    animator.SetFloat("Move Y", 0);
                }
                else if (horizontal < 0)
                {
                    animator.SetFloat("Move X", -1);
                    animator.SetFloat("Move Y", 0);
                }
            }
            else
            {
                if (vertical > 0)
                {
                    animator.SetFloat("Move X", 0);
                    animator.SetFloat("Move Y", 1);
                }
                else if (vertical < 0)
                {
                    animator.SetFloat("Move X", 0);
                    animator.SetFloat("Move Y", -1);
                }
            }

            // Update position
            rigidbody2D.MovePosition(new Vector2(newX, newY));

            if (Mathf.Abs(currentPosition.x - targetPosition.x) < 0.1f && Mathf.Abs(currentPosition.y - targetPosition.y) < 0.1f)
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", 0);
                isMoving = false; // Stop moving when close to the target
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (isManInBlack && collision.gameObject.GetComponent<MC_Controller>() != null)
            {
                // Stop movement when colliding with the MC
                isMoving = false;
                collidedWithMC = true;
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", 0);

                // Trigger conversation
                var c = GetConversation();
                if (c != null)
                {
                    var ev = Schedule.Add<Events.ShowConversation>();
                    ev.conversation = c;
                    ev.npc = this;
                    ev.gameObject = gameObject;
                    ev.conversationItemKey = "";
                }
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (isManInBlack && collision.gameObject.GetComponent<MC_Controller>() != null)
            {
                // Resume movement when no longer colliding with the MC
                isMoving = true;
                collidedWithMC = false;
            }
        }

        public void CompleteQuest(Quest q)
        {
            if (activeQuest != q) throw new System.Exception("Completed quest is not the active quest.");
            foreach (var i in activeQuest.requiredItems)
            {
                model.RemoveInventoryItem(i.item, i.count);
            }
            activeQuest.RewardItemsToPlayer();
            activeQuest.OnFinishQuest();
            activeQuest = null;
        }

        public void StartQuest(Quest q)
        {
            if (activeQuest != null) throw new System.Exception("Only one quest should be active.");
            activeQuest = q;
        }

        protected ConversationScript GetConversation()
        {
            if (activeQuest == null)
                return conversations[0];
            foreach (var q in quests)
            {
                if (q == activeQuest)
                {
                    if (q.IsQuestComplete())
                    {
                        CompleteQuest(q);
                        return q.questCompletedConversation;
                    }
                    return q.questInProgressConversation;
                }
            }
            return null;
        }
    }
}

*/