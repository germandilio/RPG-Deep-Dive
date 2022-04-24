using NaughtyAttributes;
using RPG.GameplayCore.Control;
using UnityEngine;

namespace RPG.DialogueSystem
{
    public class DialogueAISpeaker : MonoBehaviour, IRaycastable
    {
        [Required]
        [SerializeField]
        private Dialogue dialogue;

        [Required]
        [SerializeField]
        private string speakerName;

        public string SpeakerName => speakerName;

        public bool HandleRaycast(PlayerController interactController)
        {
            if (dialogue == null || !enabled) return false;

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
            if (string.IsNullOrEmpty(actionToTrigger)) return;
            
            var triggers = GetComponents<DialogueTrigger>();

            foreach (var trigger in triggers)
            {
                trigger.Trigger(actionToTrigger);
            }
        }
    }
}