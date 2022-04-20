using UnityEngine;
using UnityEngine.Events;

namespace RPG.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private DialogueAction action;

        [SerializeField]
        private UnityEvent triggered;

        public void Trigger(DialogueAction actionToTrigger)
        {
            if (actionToTrigger == action)
                triggered?.Invoke();
        }
    }
}