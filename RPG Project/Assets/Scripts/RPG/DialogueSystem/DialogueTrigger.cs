using UnityEngine;
using UnityEngine.Events;

namespace RPG.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private string action;

        [SerializeField]
        private UnityEvent onTrigger;

        public void Trigger(string actionToTrigger)
        {
            if (actionToTrigger == null) return;

            if (actionToTrigger == action)
                onTrigger?.Invoke();
        }
    }
}