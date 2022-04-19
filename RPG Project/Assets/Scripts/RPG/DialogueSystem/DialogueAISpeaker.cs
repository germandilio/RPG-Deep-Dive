using RPG.GameplayCore.Control;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class DialogueAISpeaker : MonoBehaviour, IRaycastable
    {
        [SerializeField]
        private Dialogue dialogue;

        public bool HandleRaycast(PlayerController interactController)
        {
            if (dialogue == null) return false;

            if (Input.GetMouseButtonDown(0))
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                var playerDialogue = player.GetComponent<PlayerDialogueAPI>();
                playerDialogue.StartDialogue(this, dialogue);
                return true;
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public void TriggerActions(string actionToTrigger)
        {
            var triggers = FindObjectsOfType<DialogueTrigger>();

            foreach (var trigger in triggers)
            {
                trigger.Trigger(actionToTrigger);
            }
        }
    }
}